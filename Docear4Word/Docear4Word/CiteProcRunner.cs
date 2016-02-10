using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Docear4Word.Annotations;
using Docear4Word.BibTex;

using Word;
using System.Windows.Forms;

namespace Docear4Word
{
	public delegate BibTexDatabase GetDatabaseDelegate();

	[ComVisible(true)]
	public class CiteProcRunner: JavaScriptRunner
	{
		const string CreateEngineCommand = "CreateEngine";
		const string SetOutputFormatCommand = "setOutputFormat";
		const string RebuildProcessorStateCommand = "rebuildProcessorState";
		const string AppendCitationClusterCommand = "appendCitationCluster";
		const string ProcessCitationClusterCommand = "processCitationCluster";
		const string MakeBibliographyCommand = "makeBibliography";
		const string UpdateItemsCommand = "updateItems";

		static readonly string MergedScript;
		public static string ProcessorVersion { get; private set; }

		static CiteProcRunner()
		{
			var jsonScript = File.ReadAllText(Path.Combine(FolderHelper.ApplicationRootDirectory, @"JavaScript\JSON.js"));
			var xmlDomScript = File.ReadAllText(Path.Combine(FolderHelper.ApplicationRootDirectory, @"JavaScript\xmldom.js"));
			var citeProcScript = File.ReadAllText(Path.Combine(FolderHelper.ApplicationRootDirectory, @"JavaScript\citeproc.js"));
			var sysScript = File.ReadAllText(Path.Combine(FolderHelper.ApplicationRootDirectory, @"JavaScript\Sys.js"));

			MergedScript = BuildScript(jsonScript, xmlDomScript, citeProcScript, sysScript);
		}

		readonly Dictionary<string, JSRawCitationItem> jsRawCitationItemCache = new Dictionary<string, JSRawCitationItem>();
		readonly GetDatabaseDelegate databaseProvider;
		readonly StyleInfo style;
		readonly object engine;

		CiteProcRunner(): base(MergedScript)
		{            
			if (ProcessorVersion == null)
			{
                object obj = Call("getCSLProcessorVersion");
                if(obj != null)
                    ProcessorVersion = obj.ToString();				
			}
		}

		public CiteProcRunner(StyleInfo style, GetDatabaseDelegate databaseProvider): this()
		{
			this.databaseProvider = databaseProvider;
			this.style = style;

			try
			{
				engine = Call(CreateEngineCommand, this.style.GetXml());                
				if (engine == null) throw new InvalidOperationException();
			}
			catch
			{
				throw new FailedToCreateCiteProcEngineForStyleException(style);
			}
		}

		public string GetCitationStringByIndex(int index)
		{
			return Call("getcitationStringByIndex", engine, index) as string;
		}

		object CallMethod(string name, params object[] args)
		{
			return engine.GetType().InvokeMember(name, BindingFlags.InvokeMethod, null, engine, args);
		}

		public void SetOutputFormat(string name)
		{
			CallMethod(SetOutputFormatCommand, name);
		}

		public void ResetProcessorState()
		{
			CallMethod(RebuildProcessorStateCommand, null, null);

			jsRawCitationItemCache.Clear();
		}

		/// <summary>
		/// RestoreProcessState doesn't always return all strings.
		/// Therefore we get them all manually.
		/// </summary>
		/// <param name="jsCitations">An array of JS citations.</param>
		/// <returns></returns>
/*
		public JSProcessCitationResult RestoreProcessorState(object[] jsCitations)
		{
			if (jsCitations == null) throw new ArgumentNullException("jsCitations");

			var jsCitationArray = CreateJSArray(jsCitations);

			CallMethod(RebuildProcessorStateCommand, jsCitationArray, null);

			var items = new JSProcessCitationIndexStringPair[jsCitations.Length];

			for(var i = 0; i < jsCitations.Length; i++)
			{
				items[i] = new JSProcessCitationIndexStringPair
				           	{
				           		Index = i,
				           		String = GetCitationStringByIndex(i)
				           	};
			}

			return new JSProcessCitationResult(items);
		}
*/

		public JSProcessCitationResult RestoreProcessorState(object[] jsCitations)
		{
			if (jsCitations == null) throw new ArgumentNullException("jsCitations");

			var jsCitationArray = CreateJSArray(jsCitations);

			var jsResult = CallMethod(RebuildProcessorStateCommand, jsCitationArray, null);

			return CreateJSProcessCitationResult(jsResult);
		}

		public JSProcessCitationResult ProcessCitation(JSInlineCitation citation, object citationsPre, object citationsPost)
		{
			var jsResult = CallMethod(ProcessCitationClusterCommand, citation.JSObject, citationsPre, citationsPost);

			return CreateJSProcessCitationResult(jsResult);
		}

		JSProcessCitationResult CreateJSProcessCitationResult(object jsResult)
		{
			if (jsResult == null) return null;

			var jsResultArray = ExtractJSArray(jsResult);

			var items = new JSProcessCitationIndexStringPair[jsResultArray.Count];

			for(var i = 0; i < items.Length; i++)
			{
				var jsItem = ExtractJSArray(jsResultArray[i]);

				items[i] = new JSProcessCitationIndexStringPair
				           	{
								Index = i,
								ID = (string) jsItem[0],
				           		NoteIndex = (int) jsItem[1],
				           		String = (string) jsItem[2]
				           	};
			}

			return new JSProcessCitationResult(items);
		}

		public string AppendCitation(JSInlineCitation citation)
		{
			var jsResult = CallMethod(AppendCitationClusterCommand, citation.JSObject, true);
			
			var result = CreateJSProcessCitationResult(jsResult);

			return result.Items[0].String;
		}

		public string[] UpdateItems(string[] idList, bool suppressOutput = false)
		{
			var javaStringArrayInput = CreateJSArray(idList);
			var javaStringArrayOutput = CallMethod(UpdateItemsCommand, javaStringArrayInput, suppressOutput, false);

			return ConvertToArray<string>(javaStringArrayOutput);
		}

		public BibliographyResult MakeBibliography()
		{
			var bib = CallMethod(MakeBibliographyCommand);

			if (bib is Boolean)
			{
				if (!(bool) bib)
				{
					return null;
				}
			}

			var results = ConvertToArray<object>(bib);
			var jsParameters = new JSObjectWrapper(this, results[0]);

			return new BibliographyResult(jsParameters, ExtractJSArray<string>(results[1]));
		}

		EntryAndPagePair CreateEntryAndPagePairByID(string id)
		{
			var hashIndex = id.IndexOf("#");
			string entryName;
			string pageNumberOverride;

			if (hashIndex == -1)
			{
				entryName = id;
				pageNumberOverride = null;
			}
			else
			{
				entryName = id.Substring(0, hashIndex);
				pageNumberOverride = id.Substring(hashIndex + 1);
			}
            
			var entry = databaseProvider()[entryName];
            
			if (entry == null)
			{
				throw new InvalidOperationException(String.Format("Cannot find item named '{0}' in BibTex database", entryName));
			}
            
			return new EntryAndPagePair(entry, pageNumberOverride);
		}

		/// <summary>
		/// Called implicitly by Citeproc.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>The JSObject of the JSRawCitationItem</returns>
		[UsedImplicitly]
		public object RetrieveItemByID(string id)
		{
			return FetchOrCreateJSRawCitationItem(id).JSObject;
		}

		JSRawCitationItem FetchOrCreateJSRawCitationItem(string id)
		{
			JSRawCitationItem result;
            
			if (!jsRawCitationItemCache.TryGetValue(id, out result))
			{
                EntryAndPagePair thePair = CreateEntryAndPagePairByID(id);
                
                result = new BibTexToCSLConverter(this).CreateJSRawCitationItem(thePair);
                
				jsRawCitationItemCache[id] = result;
			}

			return result;
		}

		public void CacheRawCitationItem(JSRawCitationItem rawCitationItem)
		{
			jsRawCitationItemCache[rawCitationItem.ID] = rawCitationItem;
		}

		// Problem:
		// Enter Test data
		// (Delete it) optional
		// Enter Test data again
		// Because we cached it and passed it to citeproc, it has stuff added to it
		// and is no longer suitable to put into the inline text
		// Need to store multiple items per itemSource incl. First raw data
		// That is what gets put into inline text whereas the rest can be reused.
		// Note. This may not be enough and if an identical citation is inserted
		// later and we reuse cached item, may be a problem. RawJSCitation seems untouched
		// and could be reused.
		// DocumentController.CreateInlineCitation currently sets Locator, this should be
		// done in here once and for all.
		// Change ItemData to "item" may solve the problem of duplicate data but the inlined code
		// text still has the sort keys
		// CitationInserter.CreateFieldCodeText is the key - it is using JSON Before it gets sent
		// to CiteProc - Edit/refresh/Reuse however will have had the additional items in there
		// but we only need to get the original JSON information from the cache.
		public JSInlineCitationItem CreateJSInlineCitationItem(EntryAndPagePair itemSource)
		{            
			return new JSInlineCitationItem(this)
			       	{
			       		ID = itemSource.Entry.Name, // Note no '#'!!
			       		Locator = itemSource.PageNumberOverride,
						AuthorOnly = itemSource.AuthorProcessorControl == AuthorProcessorControl.AuthorOnly ? (object) 1 : null,
						SuppressAuthor = itemSource.AuthorProcessorControl == AuthorProcessorControl.SuppressAuthor ? (object) 1 : null,
			       		ItemData = FetchOrCreateJSRawCitationItem(itemSource.ID)
			       	};
		}

		public JSInlineCitation CreateInlineCitationFromFieldJSON(Field field)
		{
			var citationJSON = Helper.ExtractCitationJSON(field);

			return JSInlineCitation.FromJSON(this, citationJSON);
		}

		public object HackCreateWrappedArray(JSCitationIDAndIndexPair[] objects)
		{
			var jsVersion = new object[objects.Length];
			for(var i = 0; i < jsVersion.Length; i++)
			{
				jsVersion[i] = CreateJSArray(new object[] { objects[i].ID, objects[i].Index });
			}

			var result = CreateJSArray(jsVersion);
			return result;
		}
	}

	public class FailedToCreateCiteProcEngineForStyleException: Exception
	{
		readonly StyleInfo style;

		public FailedToCreateCiteProcEngineForStyleException(StyleInfo style): base("A CiteProc engine could not be created from the style.")
		{
			this.style = style;
		}

		public StyleInfo Style
		{
			get { return style; }
		}
	}
}