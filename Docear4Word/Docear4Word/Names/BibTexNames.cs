using System;

namespace Docear4Word
{
	public static class BibTexNames
	{
		public const string Address = "address"; // Used
		public const string Annote = "annote"; // Used
		public const string Author = "author";  // Used
		public const string BookTitle = "booktitle"; // Not sure about conversions from here
		public const string Chapter = "chapter"; // Used
		public const string CrossRef = "crossref";
		public const string Edition = "edition"; // Used
		public const string Editor = "editor"; // Used
		public const string EPrint = "eprint";
		public const string HowPublished = "howpublished";
		public const string Institution = "institution";
		public const string Journal = "journal";  // Used
		public const string Key = "key"; // Not Applicable
		public const string Month = "month"; // Used
		public const string Note = "note"; // Used
		public const string Number = "number"; // Used
		public const string Organization = "organization"; // Used
		public const string Pages = "pages"; // Used
		public const string Publisher = "publisher"; // Used
		public const string School = "school"; // Used
		public const string Series = "series";
		public const string Title = "title"; // Used
		public const string Type = "type";
		public const string URL = "url"; // Used
		public const string Volume = "volume"; // Used
		public const string Year = "year"; // Used

		public const string Abstract = "abstract"; // Used
		public const string Acmid = "acmid";
		public const string Affiliation = "affiliation";
		public const string Assignee = "assignee";
		public const string BibSource = "bibsource"; // Used
		public const string CiteSeeUrl = "citeseeurl";
		public const string CiteULikeArticleId ="citeulike-article-id";
		public const string Comment = "comment";
		public const string Copyright = "copyright";
		public const string Day = "day";
		public const string DayFiled = "dayfiled";
		public const string DOI = "doi"; // Used
		public const string Editors = "Editors";
		public const string File = "file";
		public const string InterHash = "interhash";
		public const string ISBN = "isbn"; // Used
		public const string ISSN = "issn"; // Used
		public const string Issue = "issue"; // Used
		public const string IssueDate = "issue_date";
		public const string Keyword = "keyword"; // Used
		public const string Keywords = "keywords"; // Used
		public const string Language = "language";
		public const string Location = "location";
		public const string MonthFiled = "monthfiled";
		public const string Nationality = "nationality";
		public const string NumPages = "numpages"; // Used
		public const string OAI2Identifier = "oai2identifier";
		public const string OptEditor = "opteditor";
		public const string OptPages = "optpages";
		public const string OptPublisher = "optpublisher";
		public const string OptUrl = "opturl";
		public const string OptVolume = "optVolume";
		public const string Owner = "owner";
		public const string PostedAt = "posted-at";
		public const string Priority = "priority";
		public const string PubMedID = "pubmedid";
		public const string Review = "review";
		public const string Site = "site";
		public const string Size = "size"; // Used
		public const string Timestamp = "timestamp";
		public const string YearFiled = "yearfiled";
		public const string Remark = "note";
		public const string PDF = "pdf";
		public const string ArchivePrefix = "archivePrefix";

		// Added by Joeran
		public const string Status = "status";
		public const string Accessed = "accessed";
		public const string ISBN13 = "isbn-13";
		public const string Revision = "revision";


/*
		public static readonly string[] StandardNames
			= new[]
			  	{
			  		Address,
			  		Annote,
			  		Author,
			  		BookTitle,
			  		Chapter,
			  		CrossRef,
			  		Edition,
			  		Editor,
			  		EPrint,
			  		HowPublished,
			  		Institution,
			  		Journal,
			  		Key,
			  		Month,
			  		Note,
			  		Number,
			  		Organization,
			  		Pages,
			  		Publisher,
			  		School,
			  		Series,
			  		Title,
			  		Type,
			  		URL,
			  		Volume,
			  		Year
			  	};
*/

/*
		public static readonly string[] ExportableNames
			= new[]
			  	{
			  		Address,
			  		Annote,
			  		Author,
			  		BookTitle,
			  		Chapter,
			  		CrossRef,
			  		Edition,
			  		Editor,
			  		EPrint,
			  		HowPublished,
			  		Institution,
			  		Journal,
			  		Key,
			  		Month,
			  		Note,
			  		Number,
			  		Organization,
			  		Pages,
			  		Publisher,
			  		School,
			  		Series,
			  		Title,
			  		Type,
			  		URL,
			  		Volume,
			  		Year,

			  		Abstract,
			  		Day,
			  		DOI,
			  		File,
			  		ISBN,
			  		ISSN,
			  		Issue,
			  		Keyword,
			  		Keywords,
			  		Location,
			  		NumPages,
			  		Site,
			  		Size,
			  		PDF
			  	};
*/

/*
		public static bool IsExportableName(string name)
		{
			foreach(var exportableName in ExportableNames)
			{
				if (name == exportableName) return true;
			}

			return false;
		}
*/

/*
		public static List<string> GetExportableNames(IEnumerable<string> names)
		{
			var result = new List<string>();

			foreach (var name in names)
			{
				if (IsExportableName(name))
				{
					result.Add(name);
				}
			}

			return result;
		}
*/

	}
}