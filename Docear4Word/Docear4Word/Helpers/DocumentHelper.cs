using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using Office;

using Word;

namespace Docear4Word
{
	[ComVisible(false)]
	public enum CustomPropertyType
	{
		Integer = 1,
		Boolean = 2,
		Date = 3,
		String = 4,
		Float = 5
	}

	public static class DocumentHelper
	{
		[DebuggerStepThrough]
		public static string GetCustomStringProperty(DocumentProperties documentProperties, string name, string defaultValue = null)
		{
			try
			{
				return (string) documentProperties[name].Value;
			}
			catch
			{
				return defaultValue;
			}
		}

		[DebuggerStepThrough]
		public static bool SetCustomProperty(DocumentProperties documentProperties, string name, CustomPropertyType type, object value)
		{
			DeleteCustomProperty(documentProperties, name);

			return AddCustomProperty(documentProperties, name, type, value);
		}

		[DebuggerStepThrough]
		public static bool DeleteCustomProperty(DocumentProperties documentProperties, string propertyID)
		{
			try
			{
				documentProperties[propertyID].Delete();

				return true;
			}
			catch
			{
				return false;
			}
		}

		[DebuggerStepThrough]
		public static bool AddCustomProperty(DocumentProperties documentProperties, string name, CustomPropertyType type, object value)
		{
			try
			{
				documentProperties.Add(name, false, type, value);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}