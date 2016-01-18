using System;

using Docear4Word.Annotations;

namespace Docear4Word
{
	[UsedImplicitly]
	public class JSInlineCitationProperties: JSObjectWrapper
	{
		const string NoteIndexName = "noteIndex";

		public JSInlineCitationProperties(IJSContext context, object jsObject = null): base(context) {}

		public int NoteIndex
		{
			get { return (int) GetProperty(NoteIndexName); }
			set { SetProperty(NoteIndexName, value); }
		}
	}
}