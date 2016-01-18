using Microsoft.Win32;

namespace Docear4Word
{
	public static class RegistryHelper
	{
		internal const string ApplicationRegistrySubKey = "Docear4Word";
		internal const string OwnerRegistrySubKey = @"";
		internal const string ApplicationRegistryKey = @"SOFTWARE\" + OwnerRegistrySubKey + ApplicationRegistrySubKey;

		public static string ReadApplicationString(string name, string defaultValue = null)
		{
			using(var registryKey = Registry.CurrentUser.OpenSubKey(ApplicationRegistryKey, false))
			{
				if (registryKey == null)
				{
					var subKey = Registry.CurrentUser.CreateSubKey(ApplicationRegistryKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
					if (subKey != null)
					{
						subKey.SetValue(name, "", RegistryValueKind.String);
					}

					return defaultValue;
				}

				return (string) registryKey.GetValue(name, defaultValue);
			}
		}

		public static void WriteApplicationString(string name, string value)
		{
			using(var registryKey = Registry.CurrentUser.OpenSubKey(ApplicationRegistryKey, false))
			{
				RegistryKey subKey;

				subKey = registryKey == null
				         	? Registry.CurrentUser.CreateSubKey(ApplicationRegistryKey, RegistryKeyPermissionCheck.ReadWriteSubTree)
				         	: Registry.CurrentUser.OpenSubKey(ApplicationRegistryKey, RegistryKeyPermissionCheck.ReadWriteSubTree);

				if (subKey == null) return;

				subKey.SetValue(name, value, RegistryValueKind.String);
			}
		}

		public static bool ReadApplicationSwitch(string name, bool defaultValue = false)
		{
			return ReadApplicationString(name, defaultValue ? "1" : "0") == "1";
		}

		public static void WriteApplicationSwitch(string name, bool value)
		{
			WriteApplicationString(name, value ? "1" : "0");
		}

		public static int ReadApplicationLevel(string name, int defaultValue = 0)
		{
			int result;

			return int.TryParse(ReadApplicationString(name, defaultValue.ToString()), out result)
			       	? result
			       	: defaultValue;
		}

		public static void WriteApplicationLevel(string name, int level)
		{
			WriteApplicationString(name, level.ToString());
		}
	}
}