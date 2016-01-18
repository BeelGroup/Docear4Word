using System;
using System.Collections.Generic;
using System.Diagnostics;

using Docear4Word.BibTex;

namespace Docear4Word
{
	public class BibTexToCSLConverter
	{
		readonly IJSContext context;

		public BibTexToCSLConverter(IJSContext context)
		{
			this.context = context;
		}

		public JSRawCitationItem CreateJSRawCitationItem(EntryAndPagePair entryAndPagePair)
		{
			if (entryAndPagePair == null) throw new ArgumentNullException("entryAndPagePair");

			var result = new JSRawCitationItem(context)
			             	{
			             		ID = entryAndPagePair.ID,
								Type = GetType(entryAndPagePair.Entry.Classification)
			             	};

			new Converter(result, entryAndPagePair.Entry).ConvertItem();

			return result;
		}

		static string GetType(BibtexClassificationType bibtexClassificationType)
		{
			switch (bibtexClassificationType)
			{
				case BibtexClassificationType.Article:
					return CSLTypes.ArticleJournal;

				case BibtexClassificationType.Proceedings:
				case BibtexClassificationType.Manual:
				case BibtexClassificationType.Book:
				case BibtexClassificationType.Periodical:
					return CSLTypes.Book;

				case BibtexClassificationType.Booklet:
					return CSLTypes.Pamphlet;

				case BibtexClassificationType.InBook:
				case BibtexClassificationType.InCollection:
					return CSLTypes.Chapter;

				case BibtexClassificationType.InProceedings:
				case BibtexClassificationType.Conference:
					return CSLTypes.PaperConference;

				case BibtexClassificationType.MastersThesis:
				case BibtexClassificationType.PhdThesis:
					return CSLTypes.Thesis;

				case BibtexClassificationType.TechReport:
					return CSLTypes.Report;

				case BibtexClassificationType.Patent:
					return CSLTypes.Patent;

				case BibtexClassificationType.Electronic:
					return CSLTypes.WebPage;

				case BibtexClassificationType.Misc:
				case BibtexClassificationType.Other:
					return CSLTypes.Article;

				case BibtexClassificationType.Standard:
					return CSLTypes.Legislation;

				case BibtexClassificationType.Unpublished:
					return CSLTypes.Manuscript;

				default:
					return CSLTypes.Book;
			}
		}

		class Converter
		{
			readonly NameSplitter nameSplitter = new NameSplitter();
			readonly JSRawCitationItem item;
			readonly Dictionary<string, TagEntry> tags;

			public Converter(JSRawCitationItem item, Entry entry)
			{
				this.item = item;

				tags = entry.GetTagDictionary();
			}

			public void ConvertItem()
			{
				ApplyNameTag(item.Author, BibTexNames.Author);

				ApplyTag(CSLNames.Annote, BibTexNames.Annote);

				ApplyTag(CSLNames.Edition, BibTexNames.Edition);

				ApplyTag(CSLNames.Abstract, BibTexNames.Abstract);

				ApplyTag(CSLNames.DOI, BibTexNames.DOI);

				ApplyTag(CSLNames.Note, BibTexNames.Note);

				ApplyTag(CSLNames.Volume, BibTexNames.Volume);

				ApplyTag(CSLNames.Keyword, BibTexNames.Keywords);

				ApplyTag(CSLNames.URL, BibTexNames.URL);

				ApplyTag(CSLNames.Status, BibTexNames.Status);

				ApplyTag(CSLNames.Accessed, BibTexNames.Accessed);

				ApplyTag(CSLNames.ISSN, BibTexNames.ISSN);

				ApplyTag(CSLNames.ISBN, BibTexNames.ISBN, BibTexNames.ISBN13);

				ApplyTag(CSLNames.Title, BibTexNames.Title, BibTexNames.Chapter);

				ApplyTag(CSLNames.ContainerTitle, BibTexNames.Journal, BibTexNames.BookTitle);
				
				ApplyTag(CSLNames.CollectionTitle, BibTexNames.Series);

				ApplyNameTag(item.Editor, BibTexNames.Editor);

				switch (item.Type)
				{
					case CSLTypes.Report: // BibtexClassificationType.TechReport

						ApplyTag(CSLNames.Publisher, BibTexNames.Publisher, BibTexNames.Institution, BibTexNames.School, BibTexNames.Organization);
						break;

					case CSLTypes.Thesis:
						ApplyTag(CSLNames.Publisher, BibTexNames.Publisher, BibTexNames.School, BibTexNames.Institution, BibTexNames.Organization);
						break;

					default:
						ApplyTag(CSLNames.Publisher, BibTexNames.Publisher, BibTexNames.Organization, BibTexNames.Institution, BibTexNames.School);
						break;
				}

				ApplyTag(CSLNames.Version, BibTexNames.Revision);

				var numberTag = ExtractTag(BibTexNames.Number);
				var issueTag = ExtractTag(BibTexNames.Issue);

				if (numberTag != null && issueTag != null)
				{
					item.SetProperty(CSLNames.Number, numberTag.Display);
					item.SetProperty(CSLNames.Issue, issueTag.Display);
				}
				else if (numberTag != null)
				{
					item.SetProperty(CSLNames.Number, numberTag.Display);
					item.SetProperty(CSLNames.Issue, numberTag.Display);
				}
				else if (issueTag != null)
				{
					item.SetProperty(CSLNames.Number, issueTag.Display);
					item.SetProperty(CSLNames.Issue, issueTag.Display);
				}

				ApplyPagesTag();

				ApplyTag(CSLNames.PublisherPlace, BibTexNames.Location, BibTexNames.Address);

				// We don't support BibTexNames.IssuedDate yet
				ApplyDateTag(item.Issued);

				if (Settings.Instance.FieldMappingsDebugLevel > 0)
				{
					if (item.Annote == null) item.Annote = "*Annote*";
					if (item.CollectionEditor.Length == 0) ApplyNameTag(item.CollectionEditor, "*Collection* *Editor*");
					if (item.CollectionTitle == null) item.CollectionTitle = "*CollectionTitle*";
					if (item.ContainerAuthor.Length == 0) ApplyNameTag(item.ContainerAuthor, "*Container* *Author*");
					if (item.ContainerTitle == null) item.ContainerTitle = "*ContainerTitle*";
					if (item.Editor.Length == 0) ApplyNameTag(item.Editor, "*Just* *Editor*");
					//if (item.EventDate == null) item.EventDate = "*EventDate*";
				if (item.EventDate.DateParts.Length == 0) ApplyDebugDateTag(item.EventDate, "*EventDate*");
					if (item.EventPlace == null) item.EventPlace = "*EventPlace*";
					if (item.Issue == null) item.Issue = "*Issue*";
					//if (item.Issued == null) item.Issued = "*Issued*";
				if (item.Issued.DateParts.Length == 0) ApplyDebugDateTag(item.Issued, "*Issued*");
					if (item.Keyword == null) item.Keyword = "*Keywords*";
					if (item.Note == null) item.Note = "*Note*";
					if (item.Number == null) item.Number = "*Number*";
					if (item.PublisherPlace == null) item.PublisherPlace = "*PublisherPlace*";
					if (item.Section == null) item.Section = "*Section*";

					if (Settings.Instance.FieldMappingsDebugLevel > 1)
					{
						if (item.Abstract == null) item.Abstract = "*Abstract*";
						if (item.Accessed.DateParts.Length == 0) ApplyDebugDateTag(item.Accessed, "*Accessed*");
						if (item.Archive == null) item.Archive = "*Archive*";
						if (item.ArchiveLocation == null) item.ArchiveLocation = "*ArchiveLocation*";
						if (item.ArchivePlace == null) item.ArchivePlace = "*ArchivePlace*";
						if (item.Author.Length == 0) ApplyNameTag(item.Author, "*Just* *Author*");
 						// Authority not implemented
						if (item.CallNumber == null) item.CallNumber = "*CallNumber*";
						// Categories not implemented
						if (item.ChapterNumber == null) item.ChapterNumber = "*ChapterNumber*"; // Numeric!
						// CitationNumber omitted as it is used internally in CiteProc
						if (item.CitationLabel == null) item.CitationLabel = "*CitationLabel*";
						if (item.CollectionNumber == null) item.CollectionNumber = "*CollectionNumber*"; // Numeric!
						if (item.CollectionTitle == null) item.CollectionTitle = "*CollectionTitle*";
						if (item.Container.DateParts.Length == 0) ApplyDebugDateTag(item.Container, "*Container*");
						// ContainerTitleShort not implemented
						// Contributor (name!) not implemented
						if (item.Composer.Length == 0) ApplyNameTag(item.Composer, "*Just* *Composer*");
						// Dimensions not implemented
						// Director (name!) not implemented
						if (item.DOI == null) item.DOI = "*DOI*";
						if (item.Edition == null) item.Edition = "*Edition*"; // Numeric! (Object in code)
						if (item.EditorialDirector.Length == 0) ApplyNameTag(item.EditorialDirector, "*Just* *EditorialDirector*");
						if (item.Event == null) item.Event = "*Event*";
						// FirstReferenceNoteNumber omitted as it is used internally in CiteProc
						if (item.Genre == null) item.Genre = "*Genre*";
						if (item.Interviewer.Length == 0) ApplyNameTag(item.Interviewer, "*Just* *Interviewer*");
						if (item.ISBN == null) item.ISBN = "*ISBN*";
						// ISSN Not implemented
						if (item.JournalAbbreviation == null) item.JournalAbbreviation = "*JournalAbbreviation*";
						if (item.Jurisdiction == null) item.Jurisdiction = "*Jurisdiction*";
						if (item.Language == null) item.Language = "*Language*";
						// Locater omitted as it is used internally in CiteProc
						if (item.Medium == null) item.Medium = "*Medium*";
						if (item.NumberOfPages == null) item.NumberOfPages = "*NumberOfPages*"; // Numeric!
						if (item.NumberOfVolumes == null) item.NumberOfVolumes = "*NumberOfVolumes*"; // Numeric!
						if (item.OriginalAuthor.Length == 0) ApplyNameTag(item.OriginalAuthor, "*Just* *OriginalAuthor*");
						if (item.OriginalDate.DateParts.Length == 0) ApplyDebugDateTag(item.OriginalDate, "*OriginalDate*");
						if (item.OriginalPublisher == null) item.OriginalPublisher = "*OriginalPublisher*";
						if (item.OriginalTitle == null) item.OriginalTitle = "*OriginalTitle*";
						if (item.Page == null) item.Page = "*Page*";
						if (item.PageFirst == null) item.PageFirst = "*PageFirst*";
						// PMID not implemented
						// PMCID not implemented
						if (item.Publisher == null) item.Publisher = "*Publisher*";
						if (item.Recipient.Length == 0) ApplyNameTag(item.Recipient, "*Just* *Recipient*");
						if (item.References == null) item.References = "*References*";
						if (item.ShortTitle == null) item.ShortTitle = "*ShortTitle*";
						// Source not implemented
						if (item.Status == null) item.Status = "*Status*";
						if (item.Submitted.DateParts.Length == 0) ApplyDebugDateTag(item.Submitted, "*Submitted*");
						if (item.Title == null) item.Title = "*Title*";
						if (item.Translator.Length == 0) ApplyNameTag(item.Translator, "*Just* *Translator*");
						if (item.URL == null) item.URL = "*URL*";
						if (item.Version == null) item.Version = "*Version*";
						if (item.Volume == null) item.Volume = "*Volume*"; // Numeric!
						if (item.YearSuffix == null) item.YearSuffix = "*YearSuffix*";
					}
				}
			}

			void ApplyPagesTag()
			{
				ApplyTag(CSLNames.NumberOfPages, BibTexNames.NumPages);

				var pagesTag = ExtractTag(BibTexNames.Pages);
				if (pagesTag == null || string.IsNullOrEmpty(pagesTag.Display)) return;

				var parser = new PageRangeParser(pagesTag.Display, item.NumberOfPages);
				if (parser.Page == "-") return;
//Debug.WriteLine(parser.ToString());

				if (parser.Page != null) item.Page = parser.Page;
				if (parser.PageFirst != null) item.PageFirst = parser.PageFirst;
				if (parser.NumberOfPages != null) item.NumberOfPages = parser.NumberOfPages;
			}

			void ApplyTagMulti(string[] cslFieldNames, params string[] bibTexTagNames)
			{
				var found = false;

				foreach(var bibTexTagName in bibTexTagNames)
				{
					var tag = ExtractTag(bibTexTagName);
					if (found || tag == null) continue;

					Debug.Assert(tag != null);

					found = true;

					foreach (var cslFieldName in cslFieldNames)
					{
						item.SetProperty(cslFieldName, tag.Display);
					}
				}
			}

			void ApplyTag(string cslFieldName, params string[] bibTexTagNames)
			{
				var found = false;

				foreach(var bibTexTagName in bibTexTagNames)
				{
					var tag = ExtractTag(bibTexTagName);
					if (found || tag == null) continue;

					Debug.Assert(tag != null);

					found = true;

					item.SetProperty(cslFieldName, tag.Display);
				}
			}

			bool HasAllTags(params string[] bibTexTagNames)
			{
				foreach(var bibtexTagName in bibTexTagNames)
				{
					if (!HasTag(bibtexTagName)) return false;
				}

				return true;
			}

			bool HasTag(string bibTexTagName)
			{
				return tags.ContainsKey(bibTexTagName);
			}

			TagEntry ExtractTag(string bibTexTagName)
			{
				TagEntry result;

				if (tags.TryGetValue(bibTexTagName, out result))
				{
					tags.Remove(bibTexTagName);
					return result;
				}

				return null;
			}

			string ExtractTagDisplayValue(string name)
			{
				var tagEntry = ExtractTag(name);

				return tagEntry == null ? null : tagEntry.Display;
			}

			void ApplyDateTag(JSDateVariable dateProperty)
			{
				ApplyDateTagMulti(new [] { dateProperty });
			}

			void ApplyDateTagMulti(IEnumerable<JSDateVariable> dateProperties)
			{
				var yearString = ExtractTagDisplayValue(BibTexNames.Year) ?? string.Empty;
				var monthString = ExtractTagDisplayValue(BibTexNames.Month) ?? string.Empty;
				var dayString = ExtractTagDisplayValue(BibTexNames.Day) ?? string.Empty;

				ApplyDateTagMultiCore(dateProperties, yearString, monthString, dayString);
			}

			void ApplyDebugDateTag(JSDateVariable dateProperty, string literal)
			{
				ApplyDateTagMultiCore(new[] { dateProperty }, "2012", "August", "16");

				dateProperty.Literal = literal;
			}

			static void ApplyDateTagMultiCore(IEnumerable<JSDateVariable> dateProperties, string yearString, string monthString, string dayString)
			{
				if (yearString.Length == 0 || yearString.Length > 4) return;

				int year;
				if (!int.TryParse(yearString, out year)) return;

				var validItems = new List<object>
				                 	{
				                 		year
				                 	};

				var month = Helper.ParseMonth(monthString);
				if (month != -1) validItems.Add(month);

				int day;
				if (int.TryParse(dayString, out day)) validItems.Add(day);

				foreach(var dateProperty in dateProperties)
				{
					dateProperty.AddDatePart(validItems.ToArray());
				}
			}

			void ApplyNameTag(JSTypedArray<JSNameVariable> nameProperty, string name)
			{
				ApplyNameTagMulti(new[] { nameProperty }, name);	
			}

			void ApplyNameTagMulti(IEnumerable<JSTypedArray<JSNameVariable>> nameProperties, string name)
			{
				var tag = ExtractTag(name);
				if (tag == null) return;

				// Special case for a literal name
				if (tag.Verbatim.StartsWith("{") && tag.Verbatim.EndsWith("}"))
				{
					foreach (var nameProperty in nameProperties)
					{
						var newName = nameProperty.AddNew();

						newName.Literal = tag.Display;
					}

					return;
				}

				ApplyNameTagMultiCore(nameProperties, tag.Verbatim);
			}

			void ApplyNameTagMultiCore(IEnumerable<JSTypedArray<JSNameVariable>> nameProperties, string name)
			{
				var nameComponents = nameSplitter.Split(new LatexMiniParser(name, true).Parse());

				foreach (var nameProperty in nameProperties)
				{
					foreach (var nameComponent in nameComponents)
					{
						var newName = nameProperty.AddNew();

						newName.Family = nameComponent.Family;
						newName.Given = nameComponent.Given;
						newName.DroppingParticle = nameComponent.DroppingParticle;
						newName.NonDroppingParticle = nameComponent.NonDroppingParticle;
						newName.Suffix = nameComponent.Suffix;
						newName.CommaSuffix = nameComponent.CommaSuffix;
						newName.StaticOrdering = nameComponent.StaticOrdering;
					}
				}
			}
		}
	}
}