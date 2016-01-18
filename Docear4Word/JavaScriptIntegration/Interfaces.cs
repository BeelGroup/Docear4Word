namespace Docear4Word
{
	public interface IJSContext
	{
		object CreateJSObject();
		object CreateEmptyJSArray();

		object CreateJSArray(object[] items);
		T CreateWrappedJSObject<T>() where T: JSObjectWrapper;
		object CreateJSObjectFromJSON(string json);
		string ToJSON(object jsObject, string space = "\t");
	}

	public interface IJSWrapper
	{
		IJSContext Context { get; /*set;*/ }
		object JSObject { get; set; }
		event NotifyChildActivated Activated;
	}

	public interface IJSObjectWrapper: IJSWrapper
	{
		bool ContainsProperty(string name);
		object GetProperty(string name);
		T GetProperty<T>(string name, T defaultValue);
		void SetProperty<T>(string name, T value);
	}

	public interface IJSArrayWrapper<T>: IJSWrapper
	{
		int Length { get; }
		T this[int index] { get; set; }
		void Add(T item);
	}

	public delegate void NotifyChildActivated(object child);
}