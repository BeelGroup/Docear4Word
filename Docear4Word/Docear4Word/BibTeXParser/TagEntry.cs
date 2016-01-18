using System;

namespace Docear4Word.BibTex
{
	public class TagEntry
	{
		public static readonly TagEntry Empty = new TagEntry(string.Empty, string.Empty);

		readonly string name;
		readonly string verbatim;
		string display;

		public TagEntry(string name, string verbatim)
		{
			this.name = name;
			this.verbatim = verbatim;
			//this.display = display;
		}

		public string Name
		{
			get { return name; }
		}

		public string Display
		{
			get { return display ?? (display = new LatexMiniParser(verbatim).Parse().Trim()); }
		}

		public string Verbatim
		{
			get { return verbatim; }
		}

		public override string ToString()
		{
			return Display;
		}
	}
}