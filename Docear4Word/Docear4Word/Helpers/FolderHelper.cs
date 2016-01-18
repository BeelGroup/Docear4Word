using System;
using System.IO;
using System.Reflection;

namespace Docear4Word
{
	public static class FolderHelper
	{
		const string StylePath = @"Docear4Word\Styles";
		const string PersonalDataPath = @"Docear4Word";
		const string ErrorLogFilename = @"Docear4WordErrorLog.txt";

		public static readonly string ApplicationRootDirectory;

		static FolderHelper()
		{
			var codeBase = Assembly.GetExecutingAssembly().CodeBase;
			var uri = new UriBuilder(codeBase);
			var path = Uri.UnescapeDataString(uri.Path);

			var directory = new FileInfo(path).Directory;
			ApplicationRootDirectory = directory != null
			                           	? directory.FullName
			                           	: string.Empty;
		}

		public static string CommonApplicationFolder
		{
			get { return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); }
		}

		public static string LocalApplicationFolder
		{
			get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); }
		}

		public static string PersonalFolder
		{
			get { return Environment.GetFolderPath(Environment.SpecialFolder.Personal); }
		}

		public static string DocearStylesFolder
		{
			get { return Path.Combine(CommonApplicationFolder, StylePath); }
		}

		public static string DocearPersonalDataFolder
		{
			get { return Path.Combine(PersonalFolder, PersonalDataPath); }
		}

		public static string DocearErrorLogFilename
		{
			get { return Path.Combine(DocearPersonalDataFolder, ErrorLogFilename); }
		}

		public static void EnsureFolderExists(string folderName)
		{
			try
			{
				Directory.CreateDirectory(folderName);
			}
			catch
			{}
		}
	}
}