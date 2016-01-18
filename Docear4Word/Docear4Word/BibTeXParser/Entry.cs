using System;
using System.Collections.Generic;

namespace Docear4Word.BibTex
{
	public class Entry
	{
		readonly string entryType;
		readonly string name;
		readonly BibtexClassificationType classification;
		readonly Dictionary<string, TagEntry> tags;

		public Entry(string entryType, string name, BibtexClassificationType classification)
		{
			this.entryType = entryType;

			// Apostrophe is a reserved character in XML (and not a valid key character anyway) so we remove it here
			// which will allow it to be in the database
			this.name = name.Replace("'", ""); 
			this.classification = classification;

			tags = new Dictionary<string, TagEntry>(StringComparer.OrdinalIgnoreCase);
		}

		public string EntryType
		{
			get { return entryType; }
		}

		public string Name
		{
			get { return name; }
		}

		public BibtexClassificationType Classification
		{
			get { return classification; }
		}

		internal void AddTag(TagEntry tagEntry)
		{
			tags[tagEntry.Name] = tagEntry;
		}

		public List<string> GetTagNames()
		{
			var result = new List<string>(tags.Count);

			foreach(var tagName in tags.Keys)
			{
				result.Add(tagName);
			}

			return result;
		}

		public Dictionary<string, TagEntry> GetTagDictionary()
		{
			var result = new Dictionary<string, TagEntry>(StringComparer.OrdinalIgnoreCase);

			foreach(var tag in Tags)
			{
				result.Add(tag.Name, tag);
			}

			return result;
		}

		public TagEntry this[string tagName, TagEntry defaultValue = null]
		{
			get
			{
				TagEntry result;

				return tags.TryGetValue(tagName, out result)
				       	? result
				       	: defaultValue /*?? TagEntry.Empty*/;
			}
		}

		public IEnumerable<TagEntry> Tags
		{
			get
			{
				foreach (var tag in tags.Values)
				{
					yield return tag;
				}
			}
		}
	}
}