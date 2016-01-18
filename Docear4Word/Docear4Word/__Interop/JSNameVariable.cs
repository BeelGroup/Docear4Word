using System;

using Docear4Word.Annotations;

namespace Docear4Word
{
	[UsedImplicitly]
	public class JSNameVariable: JSObjectWrapper
	{

		public JSNameVariable()
		{
		}

		public JSNameVariable(IJSContext context): base(context)
		{
		}

		public string Family
		{
			get { return (string) GetProperty(CSLNames.Family); }
			set { SetProperty(CSLNames.Family, value); }
		}

		public string Given
		{
			get { return (string) GetProperty(CSLNames.Given); }
			set { SetProperty(CSLNames.Given, value); }
		}

		public string DroppingParticle
		{
			get { return (string) GetProperty(CSLNames.DroppingParticle); }
			set { SetProperty(CSLNames.DroppingParticle, value); }
		}

		public string NonDroppingParticle
		{
			get { return (string) GetProperty(CSLNames.NonDroppingParticle); }
			set { SetProperty(CSLNames.NonDroppingParticle, value); }
		}

		public string Suffix
		{
			get { return (string) GetProperty(CSLNames.Suffix); }
			set { SetProperty(CSLNames.Suffix, value); }
		}

		public object CommaSuffix
		{
			get { return GetProperty(CSLNames.CommaSuffix); }
			set { SetProperty(CSLNames.CommaSuffix, value); }
		}

		public object StaticOrdering
		{
			get { return GetProperty(CSLNames.StaticOrdering); }
			set { SetProperty(CSLNames.StaticOrdering, value); }
		}

		public string Literal
		{
			get { return (string) GetProperty(CSLNames.Literal); }
			set { SetProperty(CSLNames.Literal, value); }
		}

		public object ParseNames
		{
			get { return GetProperty(CSLNames.ParseNames); }
			set { SetProperty(CSLNames.ParseNames, value); }
		}
	}
}