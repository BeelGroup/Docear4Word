using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.Expando;

namespace Docear4Word
{
	public class JSObjectWrapper: IJSObjectWrapper
	{
		IExpando jsObject;
		IJSContext context;

		public event NotifyChildActivated Activated = child => { };

		readonly Dictionary<string, object> wrappers = new Dictionary<string, object>();

/*
		public JSObjectWrapper(IJSContext context)
		{
			if (context == null) throw new ArgumentNullException("context");

			this.context = context;
			jsObject = (IExpando) context.CreateJSObject();
		}
*/

		public JSObjectWrapper(IJSContext context, object jsObject = null)
		{
			if (context == null) throw new ArgumentNullException("context");

			this.context = context;
			this.jsObject = (IExpando) (jsObject ?? context.CreateJSObject());
		}

protected JSObjectWrapper()
{
}

		public IJSContext Context
		{
			get { return context; }
			set { context = value; }
		}

		public object JSObject
		{
			get { return jsObject; }
			set { jsObject = (IExpando) value; }
		}

		public bool ContainsProperty(string name)
		{
			return GetPropertyInfo(name, false) != null;
		}

		PropertyInfo GetPropertyInfo(string name, bool create)
		{
			if (name == null) throw new ArgumentNullException("name");

			var result = jsObject.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			if (result == null && create)
			{
				result = jsObject.AddProperty(name);

				Activated(this);
			}

			return result;
		}

		public bool IsEmpty()
		{
			var properties = jsObject.GetProperties(BindingFlags.Public);

			return properties.Length == 0;
		}

		protected JSTypedPrimitiveArray<T> GetTypedPrimitiveArray<T>(string name)
		{
			object result;

			if (!wrappers.TryGetValue(name, out result))
			{
				var existingJSObject = GetProperty(name);

				var newItem = new JSTypedPrimitiveArray<T>(context, existingJSObject);
				result = wrappers[name] = newItem;

				if (existingJSObject == null)
				{
					newItem.Activated += OnOwnedObjectActived;
				}
			}

			return (JSTypedPrimitiveArray<T>) result;
		}
		 
		protected JSTypedArray<T> GetTypedArray<T>(string name) where T: JSObjectWrapper
		{
			object result;

			if (!wrappers.TryGetValue(name, out result))
			{
				var existingJSObject = GetProperty(name);

				var newItem = new JSTypedArray<T>(context, existingJSObject);
				result = wrappers[name] = newItem;

				if (existingJSObject == null)
				{
					newItem.Activated += OnOwnedObjectActived;
				}
			}

			return (JSTypedArray<T>) result;
		}
		 
		protected void SetOwnedObject<T>(string name, T value) where T: JSObjectWrapper
		{
			object current;

			if (wrappers.TryGetValue(name, out current))
			{
				var existingWrapper = (T) current;
				existingWrapper.Activated -= OnOwnedObjectActived;
				wrappers.Remove(name);
			}

			if (value == null)
			{
				//Clear Property
			}
			else
			{
				wrappers[name] = value;
				SetProperty(name, value.JSObject);
			}
		}

		protected T GetOwnedObject<T>(string name) where T: JSObjectWrapper
		{
			object result;

			if (!wrappers.TryGetValue(name, out result))
			{
				var existingJSObject = GetProperty(name);

				var ctor = typeof(T).GetConstructor(new[] { typeof(IJSContext), typeof(object) } );
				if (ctor == null) throw new InvalidOperationException("Could not find ctor for " + typeof(T));

				var newItem = (T) ctor.Invoke(new[] { Context, existingJSObject } );
				result = wrappers[name] = newItem;

				if (existingJSObject == null)
				{
					newItem.Activated += OnOwnedObjectActived;
				}
			}

			return (T) result;
		}

		void OnOwnedObjectActived(object child)
		{
			foreach(var keyValuePair in wrappers)
			{
				if (keyValuePair.Value == child)
				{
					var wrappedChild = (IJSWrapper) child;
					wrappedChild.Activated -= OnOwnedObjectActived;
					var propertyName = keyValuePair.Key;
if (GetProperty(propertyName) != null) throw new InvalidOperationException();
					SetProperty(propertyName, wrappedChild.JSObject);
				}
			}
		}

/*
		T CreateOwnedObjectWrapper<T>(string name) 
		{
			var ctor = typeof(T).GetConstructor(new[] { typeof(JSObjectWrapper), typeof(string) });
			if (ctor == null) throw new InvalidOperationException("Could not find ctor for " + typeof(T));

			var result = ctor.Invoke(new object[] { this, name });

			return (T) result;
		}

		protected T GetOwnedObject<T>(string name) where T: JSObjectWrapper
		{
			object result;

			if (!wrappers.TryGetValue(name, out result))
			{
				result = CreateOwnedObjectWrapper<JSOwnedObjectWrapper<T>>(name);
				wrappers[name] = result;
				

				var existingjsObject = GetProperty(name);
				if (existingjsObject != null)
				{
					((JSOwnedObjectWrapper<T>) result).OwnedItem.JSObject = existingjsObject;
				}
			}

			return ((JSOwnedObjectWrapper<T>) result).OwnedItem;
		}
*/

		public object GetProperty(string name)
		{
			var property = GetPropertyInfo(name, false);
			if (property == null) return null;
			
			return property.GetValue(jsObject, null);
		}

		public T GetProperty<T>(string name, T defaultValue)
		{
			var property = GetPropertyInfo(name, false);
			if (property == null) return defaultValue;


			if (typeof(T) is IList)
			{
				
			}

			var value = property.GetValue(jsObject, null);
			return (T) Convert.ChangeType(value, typeof(T));
			//return (T) (property.GetValue(jsObject, null));

		}

/*
		public void ClearProperty(string name)
		{
			var property = GetPropertyInfo(name, false);
			if (property == null) return;
	

		}
*/

		public void SetProperty<T>(string name, T value)
		{
			var property = GetPropertyInfo(name, true);


			property.SetValue(jsObject, value, null);
		}

/*
		object GetJSObject(object target)
		{
			if (target is JSWrapper)
			{
				return ((JSWrapper) target).JSObject;
			}

			return target;
		}
*/
/*
		public string ToJSON()
		{
			return context.ToJSON(JSObject);
		}
*/
	}

}