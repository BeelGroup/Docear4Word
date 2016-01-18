using System.Runtime.InteropServices;

namespace Docear4Word
{
	[ComVisible(false)]
	public class BibliographyResult: JSObjectWrapper
	{
		const string MaxOffsetName = "maxoffset";
		const string EntrySpacingName = "entryspacing";
		const string LineSpacingName = "linespacing";
		const string HangingIndentName = "hangingindent";
		const string SecondFieldAlignName = "second-field-align";
		const string BibStartName = "bibstart";
		const string BibEndName = "bibend";

		public BibliographyResult()
		{
			Entries = new string[0];
		}

		public BibliographyResult(JSObjectWrapper parameters, string[] entries)
		{
			MaxOffset = parameters.GetProperty(MaxOffsetName, 0);
			EntrySpacing = parameters.GetProperty(EntrySpacingName, 1);
			LineSpacing = parameters.GetProperty(LineSpacingName, 1);
			HangingIndent = parameters.GetProperty(HangingIndentName, 0) == 2;
			SecondFieldAlign = parameters.GetProperty(SecondFieldAlignName, string.Empty) ?? string.Empty;
			Prefix = parameters.GetProperty(BibStartName, string.Empty);
			Suffix = parameters.GetProperty(BibEndName, string.Empty);

			Entries = entries;

/*
			for (var i = 0; i < entries.Length; i++)
			{
				entries[i] = HtmlHelper.DecodeHtml(entries[i]);
			}
*/
		}

		public int MaxOffset { get; private set; }

		public int EntrySpacing { get; private set; }

		public int LineSpacing { get; private set; }

		public bool HangingIndent { get; private set; }

		public string SecondFieldAlign { get; private set; }

		public string Prefix { get; private set; }

		public string Suffix { get; private set; }

		public string[] Entries { get; private set; }
	}
}