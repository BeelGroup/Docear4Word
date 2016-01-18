using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Docear4Word.BibTex;

using Word;

namespace Docear4Word
{
	public static class Helper
	{
		public static DirectoryInfo AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);

				return new FileInfo(path).Directory;
			}
		}

		public static bool IsPersonTag(string name)
		{
			switch (name.ToLowerInvariant())
			{
				case "author":
					return true;

				default:
					return false;
			}
		}

		public static bool IsDateTag(string name)
		{
			return false;
		}

		public static int ParseMonth(string text)
		{
			switch (text.ToLowerInvariant())
			{
				case "jan": case "january": return 1;
				case "feb": case "februay": return 2;
				case "mar": case "march": return 3;
				case "apr": case "april": return 4;
				case "may": return 5;
				case "jun": case "june": return 6;
				case "jul": case "july": return 7;
				case "aug": case "august": return 8;
				case "sep": case "september": return 9;
				case "oct": case "october": return 10;
				case "nov": case "november": return 11;
				case "dec": case "december": return 12;

				default:
					return -1;
			}
		}

		public static void LogUnexpectedException(string message, Exception ex)
		{
			try
			{
				var logText = string.Format("{0} [{2} / Word]\r\n{3}\r\n{1}\r\n\r\n", DateTime.UtcNow, ex, OSVersionInfo.FullVersionString, message);

				File.AppendAllText(FolderHelper.DocearErrorLogFilename, logText);
			}
			catch
			{}
		}

		public static void ShowCorruptBibtexDatabaseMessage(string filename)
		{
            Uri visitURI = new Uri("http://www.docear.org/faqs/why-can-my-bibtex-file-not-be-loaded/");

            string MessageTemplate = "The file \"{0}\"" + Environment.NewLine + "could not be loaded." + Environment.NewLine + Environment.NewLine +
                "{1}" + Environment.NewLine + Environment.NewLine +
                "For more information, please visit " + visitURI.AbsoluteUri + Environment.NewLine + Environment.NewLine + "Would you like to visit the web page?";

			var content = BibTexHelper.LastTemplateParseException == null
				? "Please ensure that the content and encoding is valid."
				: string.Format("Unexpected character at line {0}, column {1}" + Environment.NewLine + "Please check the file at or around this location.", BibTexHelper.LastTemplateParseException.Line, BibTexHelper.LastTemplateParseException.Column);

            if (MessageBox.Show(string.Format(MessageTemplate, filename, content), "Docear4Word: BibTex database could not be loaded", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(visitURI.AbsoluteUri);
            }
            
		}

		public static string ExtractCitationJSON(Field field)
		{
			return ExtractCitationJSON(field.Code.Text);
		}

		public static string ExtractCitationJSON(string fieldCodeText)
		{
			var indexOfOpeningBrace = GetCitationJSONStart(fieldCodeText);
			var citationJSON = fieldCodeText.Substring(indexOfOpeningBrace);

			return citationJSON;
		}

		public static int GetCitationJSONStart(string fieldCodeText)
		{
			return fieldCodeText.IndexOf('{');
		}

		static readonly Dictionary<string, int> Counter = new Dictionary<string, int>();

		public static BibtexClassificationType GetClassificationForType(string entryType)
		{
			if (!Counter.ContainsKey(entryType))
			{
				Counter.Add(entryType, 1);
			}
			else
			{
				Counter[entryType] = Counter[entryType] + 1;
			}

			switch (entryType.ToLowerInvariant())
			{
				case "article": return BibtexClassificationType.Article;

				case "proceedings": return BibtexClassificationType.Proceedings;
				case "manual": return BibtexClassificationType.Manual;
				case "book": return BibtexClassificationType.Book;
				case "periodical": return BibtexClassificationType.Periodical;
	
				case "booklet": return BibtexClassificationType.Booklet;

				case "inbook": return BibtexClassificationType.InBook;
				case "incollection": return BibtexClassificationType.InCollection;

				case "inproceedings": return BibtexClassificationType.InProceedings;
				case "conference": return BibtexClassificationType.Conference;

				case "mastersthesis": return BibtexClassificationType.MastersThesis;
				case "phdthesis": return BibtexClassificationType.PhdThesis;

				case "techreport": return BibtexClassificationType.TechReport;

				case "patent": return BibtexClassificationType.Patent;

				case "electronic": return BibtexClassificationType.Electronic;

				case "misc": return BibtexClassificationType.Misc;
				case "other": return BibtexClassificationType.Other;

				case "standard": return BibtexClassificationType.Standard;

				case "unpublished": return BibtexClassificationType.Unpublished;

				default:
					return BibtexClassificationType.Misc;
			}
		}

		public static void DumpCounter()
		{
			var entries = new List<KeyValuePair<string, int>>(Counter);
			entries.Sort((pair1, pair2) => -pair1.Value.CompareTo(pair2.Value));

			foreach(var entry in entries)
			{
				Console.WriteLine("{0}: {1}", entry.Key, entry.Value);
			}
		}

		public static List<EntryAndPagePair> ExtractSources(JSInlineCitation inlineCitation, BibTexDatabase currentDatabase)
		{
			var result = new List<EntryAndPagePair>(inlineCitation.CitationItems.Length);

			for(var i = 0; i < inlineCitation.CitationItems.Length; i++)
			{
				var id = inlineCitation.CitationItems[i].ID;
				if (string.IsNullOrEmpty(id)) continue;

				// This shouldn't be needed but the pre-release version included
				// the '#<Page>' suffix here as well as in the item ID
				// We remove it here; use the Locator and the document
				// will correct itself on the next Refresh
				var hashIndex = id.IndexOf('#');
				if (hashIndex != -1)
				{
					id = id.Substring(0, hashIndex);
				}

				var entry = currentDatabase[id];

				// If an entry cannot be found, we skip it
				if (entry == null) continue;

				var authorProcessorControl = GetAuthorProcessorControl(inlineCitation.CitationItems[i]);

				result.Add(new EntryAndPagePair(entry, inlineCitation.CitationItems[i].Locator)
				           {
					           AuthorProcessorControl = authorProcessorControl
				           });
			}

			return result;
		}

		public static AuthorProcessorControl GetAuthorProcessorControl(JSInlineCitationItem inlineCitationItem)
		{
			var result = AuthorProcessorControl.Standard;

			if (inlineCitationItem.AuthorOnly != null)
			{
				result = AuthorProcessorControl.AuthorOnly;
			}
			if (inlineCitationItem.SuppressAuthor != null)
			{
				result = AuthorProcessorControl.SuppressAuthor;
			}

			return result;
		}
	}
}