using System;
using System.Runtime.InteropServices.Expando;

namespace Docear4Word
{
	public class JSArrayWrapper<T>: IJSArrayWrapper<T> where T: JSObjectWrapper
	{
		readonly JSObjectWrapper owner;
		readonly string propertyName;
		JSObjectWrapper arrayWrapper;

		public JSArrayWrapper(JSObjectWrapper owner, string propertyName)
		{
			this.owner = owner;
			this.propertyName = propertyName;
		}

//Temporary Hack
public JSArrayWrapper(IJSContext context): this(new JSObjectWrapper(context), "Fred") {}

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
			set
			{
				GetArrayWrapper().JSObject = value;
//				arrayWrapper.NotifyOwnerToAddProperty(arrayWrapper);
			}
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
			GetArrayWrapper().SetProperty(Length.ToString(), item.JSObject);
		}

		public T AddNew()
		{
			var result = Context.CreateWrappedJSObject<T>();

			Add(result);

			return result;
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