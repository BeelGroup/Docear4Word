using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Docear4Word
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public abstract class JavaScriptRunner: IJSContext, IDisposable
    {
        const string HtmlPrefix = @"<!DOCTYPE html><html><head><meta charset='utf-8'><script>";
        //const string HtmlPrefix = @"<script>";
        const string HtmlSuffix = @"</script></head><body></body></html>";
        //const string HtmlSuffix = @"</script>";

    	const string LengthProperty = "length";
    	const string CreateEmptyJSArrayHelperMethod = "createEmptyJSArray";
    	const string CreateJSObjectHelperMethod = "createJSObject";
    	const string CreateJSONFromJSObjectHelperMethod = "createJSONFromJSObject";
    	const string CreateJSArrayHelperMethod = "createJSArray";
    	const string DumpJSObjectHelperMethod = "DumpJSObject";
    	const string CreateJSObjectFromJSONFunction = "createJSObjectFromJSON";

    	WebBrowser wb;
        HtmlDocument doc;
        
		public static string BuildScript(params string[] scripts)
		{
			var sb = new StringBuilder();

            sb.Append(HtmlPrefix);

			var jsRunnerScript = File.ReadAllText(Path.Combine(FolderHelper.ApplicationRootDirectory, @"JavaScript\JSRunner.js"));
			sb.Append(jsRunnerScript);

            foreach(var script in scripts)
            {
                sb.AppendLine();

                sb.Append(script);

                sb.AppendLine();
            }
			
            sb.Append(HtmlSuffix);

            return sb.ToString();
		}

    	protected JavaScriptRunner(string script)
        {
            SetupJavascriptInDoc(script);
        }

        protected void SetupJavascriptInDoc(string script) {
            wb = new WebBrowser
            {
                DocumentText = script,
                ScriptErrorsSuppressed = true,
                ObjectForScripting = this
            };

            EnsureReady();
            doc = wb.Document;
        }

    	protected JavaScriptRunner(params string[] scripts): this(BuildScript(scripts))
        {
        }

		protected void EnsureReady()
		{
			while(wb.ReadyState != WebBrowserReadyState.Complete || wb.IsBusy)
			{
				Application.DoEvents();
			}
		}

    	public bool ScriptErrorsSuppressed
    	{
    		get { return wb.ScriptErrorsSuppressed; }
			set { wb.ScriptErrorsSuppressed = value; }
    	}

    	public object ObjectForScripting
    	{
    		get { return wb.ObjectForScripting; }
			set { wb.ObjectForScripting = value; }
    	}

        public object Call(string functionName, params object[] args)
        {
            object result = doc.InvokeScript(functionName, args);
            return (result==null)?null:result;
        }

        public object Eval(string script)
        {
            return Call("eval", script);
        }

    	public static T[] ConvertToArray<T>(object javaArray)
    	{
    		var t = javaArray.GetType();
    		var count = (int) t.InvokeMember(LengthProperty, BindingFlags.GetProperty, null, javaArray, null);

    		var result = new T[count];

    		for(var i = 0; i < result.Length; i++)
    		{
    			result[i] = (T) t.InvokeMember(i.ToString(), BindingFlags.GetProperty, null, javaArray, null/*new object[] { i }*/);
    		}

    		return result;
    	}

		protected T[] ExtractJSArray<T>(object jsSource)
		{
    		var jsObject = new JSObjectWrapper(this, jsSource);

    		var arrayLength = jsObject.GetProperty(LengthProperty, 0);
    		if (arrayLength == -1) return null;

    		var result = new T[arrayLength];

    		for(var i = 0; i < arrayLength; i++)
    		{
				result[i] = (T) jsObject.GetProperty(i.ToString());
    		}

    		return result;
		}

    	protected List<object> ExtractJSArray(object source)
    	{
    		var jsObject = new JSObjectWrapper(this, source);

    		var arrayLength = jsObject.GetProperty(LengthProperty, 0);
    		if (arrayLength == -1) return null;

    		var result = new List<object>(arrayLength);

    		for(var i = 0; i < arrayLength; i++)
    		{
    			var elementValue = jsObject.GetProperty(i.ToString());

    			result.Add(elementValue);
    		}

    		return result;
    	}
/*
    	protected List<object> ExtractJSArray(object source)
    	{
    		var jsObject = Wrap(source);

    		var arrayLength = GetArrayLength(jsObject);
    		if (arrayLength == -1) return null;

    		var result = new List<object>(arrayLength);

    		for(var i = 0; i < arrayLength; i++)
    		{
    			var elementValue = jsObject.GetProperty(i.ToString());

    			result.Add(elementValue);
    		}

    		return result;
    	}
*/

/*
    	object ToJSObject(object source)
    	{
    		var result = CreateJSObject();
    		if (source == null) return result;

    		return GetJSValue(source);
    	}
*/

/*
    	protected JSWrapper CreateJSObject(object source)
    	{
    		var result = ToJSObject(source);
    		//var json = ToJSON(Unwrap(result));

    		return (JSWrapper) result;
    	}
*/

/*
    	protected bool IsArray(JSWrapper jsObject)
    	{
    		return ((IExpando) jsObject.JSObject).GetMethod("indexOf", BindingFlags.Instance | BindingFlags.NonPublic) != null;
    	}
*/

/*
    	protected T ExtractJSObject<T>(JSWrapper jsObject) where T: new()
    	{
    		//var json = ToJSON(jsObject.JSObject);

    		var result = new T();
    		var properties = typeof(T).GetProperties();
					
    		foreach(var property in properties)
    		{
    			if (!property.CanWrite) continue;

    			var jsPropertyName = GetJSPropertyName(property);

    			if (jsObject.ContainsProperty(jsPropertyName))
    			{
    				var jsValue = jsObject.GetProperty(jsPropertyName);
//if (jsValue is IExpando) continue;
    				property.SetValue(result, jsValue, null);
    			}
    		}

    		return result;
    	}
*/

		public T CreateWrappedJSObject<T>() where T: JSObjectWrapper
		{
			var ctor = typeof(T).GetConstructor(new[] { typeof(IJSContext) } );
			if (ctor == null) throw new InvalidOperationException();

			var result = (T) ctor.Invoke(new[] { this } );

			return result;
		}

		#region Methods from JSRunner.js
		public object CreateJSArray(object[] items)
		{
			var jsArray = Call(CreateJSArrayHelperMethod, items);
			return jsArray;
		}

    	public object CreateEmptyJSArray()
		{
			return Call(CreateEmptyJSArrayHelperMethod);
		}

    	public object CreateJSObjectFromJSON(string json)
    	{
    		return Call(CreateJSObjectFromJSONFunction, json);
    	}

    	public string ToJSON(object jsObject, string space = "\t")
    	{
    		return Call(CreateJSONFromJSObjectHelperMethod, jsObject, space) as string;
    	}

    	public object CreateJSObject()
		{
			return Call(CreateJSObjectHelperMethod);
		}

		//TODO: This doesn't look complete
		public string DumpJSObject(object jsObject)
		{
			return Call(DumpJSObjectHelperMethod, jsObject) as string;
		}
		#endregion Methods from JSRunner.js

	    public void Dispose()
	    {
		    try
		    {
				wb.Dispose();
		    }
			catch
			{}
	    }
    }
}