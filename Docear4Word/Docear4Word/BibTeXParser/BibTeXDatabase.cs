using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Docear4Word.BibTex
{
	[ComVisible(false)]
	public class BibTexDatabase: IEnumerable<Entry>
	{
		readonly Dictionary<string, string> abbreviations;
		readonly List<Entry> entries;
		readonly Dictionary<string, int> entryLookup;

		public BibTexDatabase()
		{
			abbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			entries = new List<Entry>();
			entryLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
		}

		BibTexDatabase(Dictionary<string, string> abbreviations)
		{
			this.abbreviations = abbreviations;
			entries = new List<Entry>();
			entryLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
		}

		public string Filename { get; set; }

		public DateTime Timestamp { get; set; }

		internal void AddAbbreviation(string key, string value)
		{
			abbreviations[key] = value;
		}

		internal void AddEntry(Entry entry)
		{
			if (entryLookup.ContainsKey(entry.Name)) return;

			entries.Add(entry);

			entryLookup[entry.Name] = entries.Count - 1;
		}

		public List<string> GetEntryNames()
		{
			var result = new List<string>(entryLookup.Count);

			foreach(var entryName in entryLookup.Keys)
			{
				result.Add(entryName);
			}

			return result;
		}

		public Entry this[string key]
		{
			get
			{
				int index;

				return entryLookup.TryGetValue(key, out index)
				       	? entries[index]
				       	: null;
			}
		}

		public Entry FindMatch(string key)
		{
			var result = this[key];

			if (result == null)
			{
				foreach (var entry in entries)
				{
					if (string.Compare(entry.Name, key, StringComparison.OrdinalIgnoreCase) == 0)
					{
						result = entry;
						break;
					}
				}
			}

			return result;
		}

		public string GetAbbreviation(string key, string defaultValue = "Abbreviation Not Found")
		{
			string result;

			return abbreviations.TryGetValue(key, out result)
			       	? result
			       	: defaultValue;
		}

		public int AbbreviationCount
		{
			get { return abbreviations.Count; }
		}

		public int EntryCount
		{
			get { return entries.Count; }
		}

		public IEnumerable<Entry> Entries
		{
			get { return entries; }
		}

		public IEnumerator<Entry> GetEnumerator()
		{
			return entries.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Entry this[int index]
		{
			get { return entries[index]; }
		}

		public BibTexDatabase CreateSubsetDatabase(params string[] keys)
		{
			var result = new BibTexDatabase(abbreviations);

			foreach (var key in keys)
			{
				var entry = this[key];
				if (entry == null) continue;

				result.AddEntry(entry);
			}

			return result;
		}

		public List<Entry> FindAll(Predicate<Entry> match)
		{
			return entries.FindAll(match);
		}
	}
}