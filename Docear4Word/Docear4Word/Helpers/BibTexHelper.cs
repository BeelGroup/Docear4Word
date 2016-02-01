using System;
using System.IO;
using System.Text;

using Docear4Word.BibTex;

namespace Docear4Word
{
	public static class BibTexHelper
	{
		public static TemplateParseException LastTemplateParseException;

		public static BibTexDatabase LoadBibTexDatabase(string filename)
		{
			LastTemplateParseException = null;

			try
			{
				var text = LoadFile(filename);

                BibTexParser parser = new BibTexParser(new BibTexLexer(text));

				var result = parser.Parse();

				var fileInfo = new FileInfo(filename);

				result.Filename = fileInfo.FullName;
				result.Timestamp = fileInfo.LastWriteTime;

				return result;
			}
			catch(Exception ex)
			{
				LastTemplateParseException = ex as TemplateParseException;

				Helper.LogUnexpectedException("Failed loading Bibtex database from '" + filename + "'", ex);

				return null;
			}
		}

		static string LoadFile(string filename)
		{
			var text = File.ReadAllText(filename);

			try
			{
				var indexOfFirstEntry = text.IndexOf('@');
				if (indexOfFirstEntry == -1) return text;

				var textBeforeFirstEntry = text.Substring(0, indexOfFirstEntry);
				var encodingIndex = textBeforeFirstEntry.LastIndexOf("Encoding: ");
				if (encodingIndex == -1) return text;

				var encodingText = textBeforeFirstEntry.Substring(encodingIndex + 10);
				var firstSpaceOrEndOfLineIndex = encodingText.IndexOfAny(new[] { ' ', '\r', '\n' });
				if (firstSpaceOrEndOfLineIndex != -1)
				{
					encodingText = encodingText.Substring(0, firstSpaceOrEndOfLineIndex);
				}

				encodingText = encodingText.Replace('_', '-');
				encodingText = encodingText.ToUpperInvariant();

				switch (encodingText)
				{
					case "UTF8":
						encodingText = "UTF-8";
						break;

					case "UTF16":
						encodingText = "UTF-16";
						break;

					case "UTF32":
						encodingText = "UTF-32";
						break;

					case "CP1252":
						encodingText = "Windows-1252";
						break;
				}

				var encoding = Encoding.GetEncoding(encodingText);

				return File.ReadAllText(filename, encoding);
			}
			catch
			{
				return text;
			}
		}

		public static BibTexDatabase CreateBibTexDatabase(string text)
		{
			var parser = new BibTexParser(new BibTexLexer(text));

			return parser.Parse();
		}
	}
}