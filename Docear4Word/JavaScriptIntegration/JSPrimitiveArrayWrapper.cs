using System;

namespace Docear4Word
{
	public class JSPrimitiveArrayWrapper<T>: IJSArrayWrapper<T>
	{
		readonly JSObjectWrapper owner;
		readonly string propertyName;
		JSObjectWrapper arrayWrapper;

		public JSPrimitiveArrayWrapper(JSObjectWrapper owner, string propertyName)
		{
			this.owner = owner;
			this.propertyName = propertyName;
		}

		public IJSContext Context
		{
			get { return owner.Context; }
			set { }
		}

		public object JSObject
		{
			get
			{
				return GetArrayWrapper().JSObject;
			}
			set { }
		}

		public int Length
		{
			get { return GetArrayWrapper().GetProperty("length", 0); }
		}

		public T this[int index]
		{
			get { return (T) GetArrayWrapper().GetProperty(index.ToString()); }
			set
			{
				if (index >= Length) throw new ArgumentOutOfRangeException();

				GetArrayWrapper().SetProperty(index.ToString(), value);
			}
		}

		public void Add(T item)
		{
			GetArrayWrapper().SetProperty(Length.ToString(), item);
		}

		JSObjectWrapper GetArrayWrapper()
		{
			if (arrayWrapper == null)
			{
				arrayWrapper = new JSObjectWrapper(Context, Context.CreateJSArray());
				owner.SetProperty(propertyName, arrayWrapper.JSObject);
			}

			return arrayWrapper;
		}

	}
}