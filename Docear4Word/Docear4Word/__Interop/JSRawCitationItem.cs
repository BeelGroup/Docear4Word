using System;

using Docear4Word.Annotations;

namespace Docear4Word
{
	/// <summary>
	/// This is the one created by BibTextToCSLConverter
	/// It will also be (possibly wrapped in a Memento and) stored inline
	/// </summary>
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public class JSRawCitationItem: JSObjectWrapper
	{
		public JSRawCitationItem()
		{}

		public JSRawCitationItem(IJSContext context): base(context)
		{}

		public JSRawCitationItem(IJSContext context, object jsObject = null): base(context, jsObject)
		{}

		public string Type
		{
			get { return (string) GetProperty(CSLNames.Type); }
			set { SetProperty(CSLNames.Type, value);}
		}

		public string ID
		{
			get { return (string) GetProperty(CSLNames.ID); }
			set { SetProperty(CSLNames.ID, value);}
		}

		public JSTypedPrimitiveArray<string> Categories
		{
			get { return GetTypedPrimitiveArray<string>(CSLNames.Categories); }
		}

		public string Language
		{
			get { return (string) GetProperty(CSLNames.Language); }
			set { SetProperty(CSLNames.Language, value);}
		}

		public string JournalAbbreviation
		{
			get { return (string) GetProperty(CSLNames.JournalAbbrevation); }
			set { SetProperty(CSLNames.JournalAbbrevation, value);}
		}

		public string ShortTitle
		{
			get { return (string) GetProperty(CSLNames.ShortTitle); }
			set { SetProperty(CSLNames.ShortTitle, value);}
		}

		//TODO: Name types can also be a literal string!!!
		public JSTypedArray<JSNameVariable> Author
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.Author); }
		}

		public JSTypedArray<JSNameVariable> CollectionEditor
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.CollectionEditor); }
		}

		public JSTypedArray<JSNameVariable> Composer
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.Composer); }
		}

		public JSTypedArray<JSNameVariable> ContainerAuthor
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.ContainerAuthor); }
		}

		public JSTypedArray<JSNameVariable> Editor
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.Editor); }
		}

		public JSTypedArray<JSNameVariable> EditorialDirector
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.EditorialDirector); }
		}

		public JSTypedArray<JSNameVariable> Interviewer
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.Interviewer); }
		}

		public JSTypedArray<JSNameVariable> OriginalAuthor
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.OriginalAuthor); }
		}

		public JSTypedArray<JSNameVariable> Recipient
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.Recipient); }
		}

		public JSTypedArray<JSNameVariable> Translator
		{
			get { return GetTypedArray<JSNameVariable>(CSLNames.Translator); }
		}

		public JSDateVariable Accessed
		{
			get { return GetOwnedObject<JSDateVariable>(CSLNames.Accessed); }
		}

		public JSDateVariable Container
		{
			get { return GetOwnedObject<JSDateVariable>(CSLNames.Container); }
		}

		public JSDateVariable EventDate
		{
			get { return GetOwnedObject<JSDateVariable>(CSLNames.EventDate); }
		}

		public JSDateVariable Issued
		{
			get { return GetOwnedObject<JSDateVariable>(CSLNames.Issued); }
		}

		public JSDateVariable OriginalDate
		{
			get { return GetOwnedObject<JSDateVariable>(CSLNames.OriginalDate); }
		}

		public JSDateVariable Submitted
		{
			get { return GetOwnedObject<JSDateVariable>(CSLNames.Submitted); }
		}

		public string Abstract
		{
			get { return (string) GetProperty(CSLNames.Abstract); }
			set { SetProperty(CSLNames.Abstract, value);}
		}

		public string Annote
		{
			get { return (string) GetProperty(CSLNames.Annote); }
			set { SetProperty(CSLNames.Annote, value);}
		}

		public string Archive
		{
			get { return (string) GetProperty(CSLNames.Archive); }
			set { SetProperty(CSLNames.Archive, value);}
		}

		public string ArchiveLocation
		{
			get { return (string) GetProperty(CSLNames.ArchiveLocation); }
			set { SetProperty(CSLNames.ArchiveLocation, value);}
		}

		public string ArchivePlace
		{
			get { return (string) GetProperty(CSLNames.ArchivePlace); }
			set { SetProperty(CSLNames.ArchivePlace, value);}
		}

		public string Authority
		{
			get { return (string) GetProperty(CSLNames.Authority); }
			set { SetProperty(CSLNames.Authority, value);}
		}

		public string CallNumber
		{
			get { return (string) GetProperty(CSLNames.CallNumber); }
			set { SetProperty(CSLNames.CallNumber, value);}
		}

		public string ChapterNumber
		{
			get { return (string) GetProperty(CSLNames.ChapterNumber); }
			set { SetProperty(CSLNames.ChapterNumber, value);}
		}

		public string CitationNumber
		{
			get { return (string) GetProperty(CSLNames.CitationNumber); }
			set { SetProperty(CSLNames.CitationNumber, value);}
		}

		public string CitationLabel
		{
			get { return (string) GetProperty(CSLNames.CitationLabel); }
			set { SetProperty(CSLNames.CitationLabel, value);}
		}

		public string CollectionNumber
		{
			get { return (string) GetProperty(CSLNames.CollectionNumber); }
			set { SetProperty(CSLNames.CollectionNumber, value);}
		}

		public string CollectionTitle
		{
			get { return (string) GetProperty(CSLNames.CollectionTitle); }
			set { SetProperty(CSLNames.CollectionTitle, value);}
		}

		public string ContainerTitle
		{
			get { return (string) GetProperty(CSLNames.ContainerTitle); }
			set { SetProperty(CSLNames.ContainerTitle, value);}
		}

		public string DOI
		{
			get { return (string) GetProperty(CSLNames.DOI); }
			set { SetProperty(CSLNames.DOI, value);}
		}

		public object Edition
		{
			get { return GetProperty(CSLNames.Edition); }
			set { SetProperty(CSLNames.Edition, value);}
		}

		public string Event
		{
			get { return (string) GetProperty(CSLNames.Event); }
			set { SetProperty(CSLNames.Event, value);}
		}

		public string EventPlace
		{
			get { return (string) GetProperty(CSLNames.EventPlace); }
			set { SetProperty(CSLNames.EventPlace, value);}
		}

		public string FirstReferenceNoteNumber
		{
			get { return (string) GetProperty(CSLNames.FirstReferenceNoteNumber); }
			set { SetProperty(CSLNames.FirstReferenceNoteNumber, value);}
		}

		public string Genre
		{
			get { return (string) GetProperty(CSLNames.Genre); }
			set { SetProperty(CSLNames.Genre, value);}
		}

		public string ISBN
		{
			get { return (string) GetProperty(CSLNames.ISBN); }
			set { SetProperty(CSLNames.ISBN, value);}
		}

		public object Issue
		{
			get { return GetProperty(CSLNames.Issue); }
			set { SetProperty(CSLNames.Issue, value);}
		}

		public string Jurisdiction
		{
			get { return (string) GetProperty(CSLNames.Jurisdiction); }
			set { SetProperty(CSLNames.Jurisdiction, value);}
		}

		public string Keyword
		{
			get { return (string) GetProperty(CSLNames.Keyword); }
			set { SetProperty(CSLNames.Keyword, value);}
		}

		public string Locator
		{
			get { return (string) GetProperty(CSLNames.Locator); }
			set { SetProperty(CSLNames.Locator, value);}
		}

		public string Medium
		{
			get { return (string) GetProperty(CSLNames.Medium); }
			set { SetProperty(CSLNames.Medium, value);}
		}

		public string Note
		{
			get { return (string) GetProperty(CSLNames.Note); }
			set { SetProperty(CSLNames.Note, value);}
		}

		public object Number
		{
			get { return GetProperty(CSLNames.Number); }
			set { SetProperty(CSLNames.Number, value);}
		}

		public string NumberOfPages
		{
			get { return (string) GetProperty(CSLNames.NumberOfPages); }
			set { SetProperty(CSLNames.NumberOfPages, value);}
		}

		public object NumberOfVolumes
		{
			get { return GetProperty(CSLNames.NumberOfVolumes); }
			set { SetProperty(CSLNames.NumberOfVolumes, value);}
		}

		public string OriginalPublisher
		{
			get { return (string) GetProperty(CSLNames.OriginalPublisher); }
			set { SetProperty(CSLNames.OriginalPublisher, value);}
		}

		public string OriginalPublisherPlace
		{
			get { return (string) GetProperty(CSLNames.OriginalPublisherPlace); }
			set { SetProperty(CSLNames.OriginalPublisherPlace, value);}
		}

		public string OriginalTitle
		{
			get { return (string) GetProperty(CSLNames.OriginalTitle); }
			set { SetProperty(CSLNames.OriginalTitle, value);}
		}

		public string Page
		{
			get { return (string) GetProperty(CSLNames.Page); }
			set { SetProperty(CSLNames.Page, value);}
		}

		public string PageFirst
		{
			get { return (string) GetProperty(CSLNames.PageFirst); }
			set { SetProperty(CSLNames.PageFirst, value);}
		}

		public string Publisher
		{
			get { return (string) GetProperty(CSLNames.Publisher); }
			set { SetProperty(CSLNames.Publisher, value);}
		}

		public string PublisherPlace
		{
			get { return (string) GetProperty(CSLNames.PublisherPlace); }
			set { SetProperty(CSLNames.PublisherPlace, value);}
		}

		public string References
		{
			get { return (string) GetProperty(CSLNames.References); }
			set { SetProperty(CSLNames.References, value);}
		}

		public string Section
		{
			get { return (string) GetProperty(CSLNames.Section); }
			set { SetProperty(CSLNames.Section, value);}
		}

		public string Status
		{
			get { return (string) GetProperty(CSLNames.Status); }
			set { SetProperty(CSLNames.Status, value);}
		}

		public string Title
		{
			get { return (string) GetProperty(CSLNames.Title); }
			set { SetProperty(CSLNames.Title, value);}
		}

		public string URL
		{
			get { return (string) GetProperty(CSLNames.URL); }
			set { SetProperty(CSLNames.URL, value);}
		}

		public string Version
		{
			get { return (string) GetProperty(CSLNames.Version); }
			set { SetProperty(CSLNames.Version, value);}
		}

		public object Volume
		{
			get { return GetProperty(CSLNames.Volume); }
			set { SetProperty(CSLNames.Volume, value);}
		}

		public string YearSuffix
		{
			get { return (string) GetProperty(CSLNames.YearSuffix); }
			set { SetProperty(CSLNames.YearSuffix, value);}
		}
	}
}