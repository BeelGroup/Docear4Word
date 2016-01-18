using System;

using Docear4Word.BibTex;

namespace Docear4Word
{
	public class EntryAndPagePair
	{
		readonly Entry entry;
		readonly string pageNumberOverride;
		readonly string id;
		readonly string authors;

		public EntryAndPagePair(Entry entry, string pageNumberOverride = null)
		{
			this.entry = entry;

			// Ensure page number override is trimmed and, if empty, reset to null
			if (pageNumberOverride != null)
			{
				pageNumberOverride = pageNumberOverride.Trim();

				if (pageNumberOverride.Length == 0) pageNumberOverride = null;
			}

			this.pageNumberOverride = pageNumberOverride;

			id = this.pageNumberOverride == null
			     	? entry.Name
			     	: entry.Name + "#" + pageNumberOverride;

			authors = entry["author", TagEntry.Empty].Display.Trim();
		}

		public Entry Entry
		{
			get { return entry; }
		}

		public string EntryName
		{
			get { return entry.Name; }
		}

		public string PageNumberOverride
		{
			get { return pageNumberOverride; }
		}

		// This is the one with a '#' if there is a PageNumberOverride
		public string ID
		{
			get { return id; }
		}

		public string Authors
		{
			get { return authors; }
		}

		public AuthorProcessorControl AuthorProcessorControl { get; set;}
	}

	public enum AuthorProcessorControl
	{
		Standard,
		AuthorOnly,
		SuppressAuthor,
		SplitAuthor // Only used for a first insertion
	}
}