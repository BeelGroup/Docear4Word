using System;

namespace Docear4Word
{
	public class JSTypedPrimitiveArray<T>: IJSArrayWrapper<T>
	{
		const string LengthName = "length";

		readonly IJSContext context;
		readonly JSObjectWrapper arrayWrapper;

		public event NotifyChildActivated Activated = child => { };

		public JSTypedPrimitiveArray(IJSContext context, object existingJSObject = null)
		{
			this.context = context;

			arrayWrapper = new JSObjectWrapper(context, existingJSObject ?? Context.CreateEmptyJSArray());

			if (existingJSObject == null)
			{
				arrayWrapper.Activated += OnElementActivated;
			}
		}

		void OnElementActivated(object child)
		{
			Activated(this);
		}

		public IJSContext Context
		{
			get { return context; }
		}

		public object JSObject
		{
			get { return arrayWrapper.JSObject; }
			set { arrayWrapper.JSObject = value; }
		}

		public int Length
		{
			get { return arrayWrapper.GetProperty(LengthName, 0); }
		}

		public T this[int index]
		{
			get { return (T) arrayWrapper.GetProperty(index.ToString()); }
			set
			{
				if (index >= Length) throw new ArgumentOutOfRangeException();

				arrayWrapper.SetProperty(index.ToString(), value);
			}
		}

		public void Add(T item)
		{
			arrayWrapper.SetProperty(Length.ToString(), item);
		}
	}
}