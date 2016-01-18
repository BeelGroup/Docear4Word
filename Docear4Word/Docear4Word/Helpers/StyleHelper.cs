using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Docear4Word
{
	public static class StyleHelper
	{
		public const string FindMoreCitationStylesTitle = "Find more Citation Styles...";
		public const string FindMoreCitationStylesUrl = @"http://docear.org/software/docear4word/citation-styles";
		public const string StyleXmlNamespace = @"http://purl.org/net/xbiblio/csl";

		const string TitleXPath = "//x:style/x:info/x:title";
		const string SummaryXPath = "//x:style/x:info/x:summary";

		public static readonly StyleInfo FindMoreCitationStyles = new StyleInfo
		                                                          	{
		                                                          		FileInfo = null, 
																		Title = FindMoreCitationStylesTitle
		                                                          	};

		public static List<StyleInfo> GetStylesInfo(string path = null)
		{
			try
			{
				var result = new List<StyleInfo>();

				var doc = new XmlDocument();
				var xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
				xmlNamespaceManager.AddNamespace("x", StyleXmlNamespace);

				foreach (var styleFileInfo in GetStyleFiles(path))
				{
					try
					{
						doc.Load(styleFileInfo.FullName);

						var styleInfo = new StyleInfo
						                	{
						                		FileInfo = styleFileInfo
						                	};

						var node = doc.SelectSingleNode(TitleXPath, xmlNamespaceManager);
						styleInfo.Title = node == null ? string.Empty : node.InnerText;

						node = doc.SelectSingleNode(SummaryXPath, xmlNamespaceManager);
						styleInfo.Summary = node == null ? string.Empty : node.InnerText;

						result.Add(styleInfo);
					}
					catch {}
				}

				return result;
			}
			catch
			{
				return new List<StyleInfo>();
			}
		}

		public static StyleInfo LoadFromFile(string filename)
		{
			var styleFileInfo = new FileInfo(filename);

			var doc = new XmlDocument();
			var xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
			xmlNamespaceManager.AddNamespace("x", StyleXmlNamespace);

			var styleInfo = new StyleInfo
						        {
						            FileInfo = styleFileInfo
						        };

			var node = doc.SelectSingleNode(TitleXPath, xmlNamespaceManager);
			styleInfo.Title = node == null ? string.Empty : node.InnerText;

			node = doc.SelectSingleNode(SummaryXPath, xmlNamespaceManager);
			styleInfo.Summary = node == null ? string.Empty : node.InnerText;

			return styleInfo;
		}

		static IEnumerable<FileInfo> GetStyleFiles(string path)
		{
			try
			{
				var styleDirectory = new DirectoryInfo(path ?? FolderHelper.DocearStylesFolder);

				if (!styleDirectory.Exists)
				{
					styleDirectory.Create();
				}

				return styleDirectory.GetFiles("*.csl");
			}
			catch
			{
				return new FileInfo[0];
			}
		}
	}
}