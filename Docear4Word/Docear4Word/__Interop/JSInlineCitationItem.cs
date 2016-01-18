using System;

using Docear4Word.Annotations;

namespace Docear4Word
{
	[UsedImplicitly]
	public class JSInlineCitationItem: JSObjectWrapper
	{
		const string IDName = "id";
		const string ItemDataName = "item";
		const string PrefixName = "prefix";
		const string SuffixName = "suffix";
		const string LocatorName = "locator";
		const string LabelName = "label";
		const string SuppressAuthorName = "suppress-author";
		const string AuthorOnlyName = "author-only";
		const string URIsName = "uris";

		public JSInlineCitationItem(IJSContext context): base(context)
		{}

		public string RawXml { get; set; }

		public string ID
		{
			get { return (string) GetProperty(IDName); }
			set { SetProperty(IDName, value); }
		}

		public string Prefix
		{
			get { return (string) GetProperty(PrefixName); }
			set { SetProperty(PrefixName, value); }
		}

		public string Suffix
		{
			get { return (string) GetProperty(SuffixName); }
			set { SetProperty(SuffixName, value); }
		}

		public string Locator
		{
			get { return (string) GetProperty(LocatorName); }
			set { SetProperty(LocatorName, value); }
		}

		public string Label
		{
			get { return (string) GetProperty(LabelName); }
			set { SetProperty(LabelName, value); }
		}

		public object SuppressAuthor
		{
			get { return GetProperty(SuppressAuthorName); }
			set { SetProperty(SuppressAuthorName, value); }
		}

		public object AuthorOnly
		{
			get { return GetProperty(AuthorOnlyName); }
			set { SetProperty(AuthorOnlyName, value); }
		}

		public JSRawCitationItem ItemData
		{
			get { return GetOwnedObject<JSRawCitationItem>(ItemDataName); }
			set
			{
				SetOwnedObject(ItemDataName, value);
				//ID = value.ID;
			}
		}

		public JSTypedPrimitiveArray<string> URIs
		{
			get { return GetTypedPrimitiveArray<string>(URIsName); }
		}
	}
}