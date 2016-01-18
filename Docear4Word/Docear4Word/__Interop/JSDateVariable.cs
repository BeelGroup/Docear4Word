using System;

using Docear4Word.Annotations;

namespace Docear4Word
{
	[UsedImplicitly]
	public class JSDateVariable: JSObjectWrapper
	{
		public JSDateVariable(IJSContext context, object existingJSObject = null): base(context, existingJSObject)
		{}

		public JSTypedPrimitiveArray<object> DateParts
		{
			get { return GetTypedPrimitiveArray<object>(CSLNames.DateParts); }
		}

		public void AddDatePart(object[] items)
		{
			var jsArray = Context.CreateJSArray(items);
			DateParts.Add(jsArray);
		}

		public object Season
		{
			get { return GetProperty(CSLNames.Season); }
			set { SetProperty(CSLNames.Season, value);}
		}

		public object Circa
		{
			get { return GetProperty(CSLNames.Circa); }
			set { SetProperty(CSLNames.Circa, value); }
		}

		public string Literal
		{
			get { return (string) GetProperty(CSLNames.Literal); }
			set { SetProperty(CSLNames.Literal, value); }
		}

		public string Raw
		{
			get { return (string) GetProperty(CSLNames.Raw); }
			set { SetProperty(CSLNames.Raw, value); }
		}
	}
}