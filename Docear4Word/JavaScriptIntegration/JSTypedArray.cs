using System;

namespace Docear4Word
{
	public class JSTypedArray<T>: IJSArrayWrapper<T> where T: JSObjectWrapper
	{
		const string LengthName = "length";

		readonly IJSContext context;
		readonly JSObjectWrapper arrayWrapper;

		public event NotifyChildActivated Activated = child => { };

		public JSTypedArray(IJSContext context, object existingJSObject = null)
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
			get
			{
				if (index >= Length) throw new ArgumentOutOfRangeException();

				var jsObject = arrayWrapper.GetProperty(index.ToString());

				var result = context.CreateWrappedJSObject<T>();
				result.JSObject = jsObject;

				return result;
				//return (T) arrayWrapper.GetProperty(index.ToString());
			}
			set
			{
				if (index >= Length) throw new ArgumentOutOfRangeException();

				arrayWrapper.SetProperty(index.ToString(), value);
			}
		}

		public void Add(T item)
		{
			arrayWrapper.SetProperty(Length.ToString(), item.JSObject);
		}

		public T AddNew()
		{
			var result = context.CreateWrappedJSObject<T>();

			Add(result);

			return result;
		}

	}
}