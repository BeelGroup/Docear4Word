using System;

namespace Docear4Word
{
	public static class CSLNames
	{
		// This is wrong!! Find something else!
		public const string Book = "book";

		// Standard variables
		public const string Abstract = "abstract";
		public const string Annote = "annote";
		public const string Archive = "archive";
		public const string ArchiveLocation = "archive-location";
		public const string ArchivePlace = "archive-place";
		public const string Authority = "authority";
		public const string CallNumber = "call-number";
		public const string Categories = "categories";
		public const string CitationLabel = "citation-label";
		public const string CitationNumber = "citation-number";
		public const string CollectionTitle = "collection-title";
		public const string ContainerTitle = "container-title";
		public const string ContainerTitleShort = "container-title-short";
		public const string Dimensions = "dimensions";
		public const string DOI = "DOI"; // Digital Object Identifier
		public const string Event = "event";
		public const string EventPlace = "event-place";
		public const string Family = "family";
		public const string FirstReferenceNoteNumber = "first-reference-note-number";
		public const string Given = "given";
		public const string Genre = "genre";
		public const string ID = "id";
		public const string ISBN = "ISBN";
		public const string ISSN = "ISSN";
		public const string JournalAbbrevation = "journalAbbreviation";
		public const string Jurisdiction = "Jurisdiction";
		public const string Keyword = "keyword";
		public const string Language = "language";
		public const string Locator = "locator";
		public const string Medium = "medium";
		public const string Note = "note";
		public const string OriginalPublisher = "original-publisher";
		public const string OriginalPublisherPlace = "original-publisher-place";
		public const string OriginalTitle = "original-title";
		public const string Page = "page";
		public const string PageFirst = "page-first";
		public const string PMID = "PMID";
		public const string PMCID = "PMCID";
		public const string Publisher = "publisher";
		public const string PublisherPlace = "publisher-place";
		public const string References = "references";
		public const string Section = "section";
		public const string ShortTitle = "shortTitle";
		public const string Source = "source";
		public const string Status = "status";
		public const string Title = "title";
		public const string Type = "type";
		//public const string TitleShort = "title-short";
		public const string URL = "URL";
		public const string Version = "version";
		public const string YearSuffix = "year-suffix";

		// Added from schema
		public const string DroppingParticle = "dropping-particle";
		public const string NonDroppingParticle = "non-dropping-particle";
		public const string Suffix = "suffix";
		public const string CommaSuffix = "comma-suffix";
		public const string StaticOrdering = "static-ordering";
		public const string Literal = "literal";
		public const string ParseNames = "parse-names";
		public const string DateParts = "date-parts";
		public const string Season = "season";
		public const string Circa = "circa";
		public const string Raw = "raw";


		// Name variables
		public const string Author = "author";
		public const string CollectionEditor = "collection-editor";
		public const string Composer = "composer";
		public const string ContainerAuthor = "container-author";
		public const string Editor = "editor";
		public const string EditorialDirector = "editorial-director";
		public const string Illustrator = "illustrator";
		public const string Interviewer = "interviewer";
		public const string OriginalAuthor = "originalAuthor";
		public const string Recipient = "recipient";
		public const string Translator = "translator";

		// Date variables
		public const string Accessed = "accessed";
		public const string Container = "container";
		public const string EventDate = "event-date";
		public const string Issued = "issued";
		public const string OriginalDate = "original-date";
		public const string Submitted = "submitted";

		// Number variables
		public const string ChapterNumber = "chapter-number";
		public const string CollectionNumber = "collection-number";
		public const string Edition = "edition";
		public const string Issue = "issue";
		public const string Number = "number";
		public const string NumberOfPages = "number-of-pages";
		public const string NumberOfVolumes = "number-of-volumes";
		public const string Volume = "volume";

		public static string[] NameVariables
			= new[]
			  	{
			  		Author,
			  		Editor,
			  		Translator,
			  		//Contributor,
			  		CollectionEditor,
			  		Composer,
			  		ContainerAuthor,
			  		EditorialDirector,
			  		Interviewer,
			  		OriginalAuthor,
			  		Recipient
			  	};

		public static string[] NumericVariables
			= new[]
			  	{
			  		ChapterNumber,
			  		CollectionNumber,
			  		Edition,
			  		Issue,
			  		Locator,
			  		Number,
			  		NumberOfPages,
			  		NumberOfVolumes,
			  		Volume,
			  		CitationNumber
			  	};

		public static string[] DateVariables
			= new[]
			  	{
			  		//LocatorDate,
			  		Issued,
			  		EventDate,
			  		Accessed,
			  		Container,
			  		OriginalDate
			  	};

		public static string[] MultiFieldVariables
			= new[]
			  	{
			  		Publisher,
			  		PublisherPlace,
			  		EventPlace
			  	};
	}
}