using System;

namespace Docear4Word
{
	public class JSInlineCitation: JSObjectWrapper
	{
		const string SchemaName = "schema";
		const string CitationIDName = "citationID";
		const string CitationItemsName = "citationItems";
		const string PropertiesName = "properties";
		const string SchemaValue = "https://raw.github.com/citation-style-language/schema/master/csl-citation.json";
		const string PreviouslyFormattedCitationName = "previouslyFormattedCitation";

		public static JSInlineCitation FromJSON(IJSContext context, string json)
		{
			var jsObject = context.CreateJSObjectFromJSON(json);

			var result = new JSInlineCitation(context, jsObject)
			             	{
			             		FieldCodeJSON = json
			             	};
			return result;
		}

		JSInlineCitation(IJSContext context, object jsObject): base(context, jsObject)
		{}

		public JSInlineCitation(IJSContext context): base(context)
		{
			Schema = SchemaValue;
			CitationID = Guid.NewGuid().ToString("b");
		}

		public string FieldCodeJSON { get; set; }

		public string Schema
		{
			get { return (string) GetProperty(SchemaName); }
			set { SetProperty(SchemaName, value);}
		}

		public object CitationID
		{
			get { return GetProperty(CitationIDName); }
			set { SetProperty(CitationIDName, value); }
		}

		public JSTypedArray<JSInlineCitationItem> CitationItems
		{
			get { return GetTypedArray<JSInlineCitationItem>(CitationItemsName); }
		}

		public string PreviouslyFormattedCitation
		{
			get { return (string) GetProperty(PreviouslyFormattedCitationName); }
			set { SetProperty(PreviouslyFormattedCitationName, value);}
		}

		public JSInlineCitationProperties Properties
		{
			get { return GetOwnedObject<JSInlineCitationProperties>(PropertiesName); }
		}
	}
}