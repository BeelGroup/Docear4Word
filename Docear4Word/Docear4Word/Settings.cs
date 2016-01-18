using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using Docear4Word.BibTex;

namespace Docear4Word
{
	[ComVisible(false)]
	public class Settings
	{
		const string CustomDatabaseFilenameRegistryValueName = "CustomDatabaseFilename";
		const string DatabaseSourceRegistryValueName = "DatabaseSource";
		const string AllowPerDocumentDatabasesRegistryValueName = "AllowPerDocumentDatabases";

		const string DocearDefaultDatabaseSource = "Docear";
		const string CustomDatabaseSource = "Custom";

		const string FieldMappingsDebugLevelName = "FieldMappingsDebugLevel";

		const string RefreshUpdatesCitationsFromDatabaseName = "RefreshUpdatesCitationsFromDatabase";

		public const string DatabaseEnvironmentVariableName = "docear_bibtex_current";

		public static readonly Settings Instance = new Settings();

		BibTexDatabase lastLoadedDatabase;

		Settings()
		{}

		public bool AllowPerDocumentDatabases
		{
			get { return RegistryHelper.ReadApplicationSwitch(AllowPerDocumentDatabasesRegistryValueName); }
		}

		public int FieldMappingsDebugLevel
		{
			get { return RegistryHelper.ReadApplicationLevel(FieldMappingsDebugLevelName); }
		}

		public string CustomDatabaseFilename
		{
			get
			{
				return RegistryHelper.ReadApplicationString(CustomDatabaseFilenameRegistryValueName);
			}
			set
			{
				RegistryHelper.WriteApplicationString(CustomDatabaseFilenameRegistryValueName, value);
			}
		}

		public bool UseDocearDefaultDatabase
		{
			get
			{
				return RegistryHelper.ReadApplicationString(DatabaseSourceRegistryValueName) != CustomDatabaseSource;
			}
			set
			{
				RegistryHelper.WriteApplicationString(DatabaseSourceRegistryValueName,
				                                      value
				                                      	? DocearDefaultDatabaseSource
				                                      	: CustomDatabaseSource
					);
			}
		}

		public bool RefreshUpdatesCitationsFromDatabase
		{
			get
			{
				return RegistryHelper.ReadApplicationSwitch(RefreshUpdatesCitationsFromDatabaseName, true);
			}
			set
			{
				RegistryHelper.WriteApplicationSwitch(RefreshUpdatesCitationsFromDatabaseName, value);
			}
		}

		public string GetDefaultDatabaseFolder()
		{
			try
			{
				return Path.GetDirectoryName(GetDefaultDatabaseFilename());
			}
			catch
			{
				return string.Empty;
			}
		}

		public string GetDefaultDatabaseFilename()
		{
			return Instance.UseDocearDefaultDatabase
			       	? Environment.GetEnvironmentVariable(DatabaseEnvironmentVariableName, EnvironmentVariableTarget.User)
			       	: Instance.CustomDatabaseFilename;
		}

		public BibTexDatabase GetDefaultDatabase()
		{
			return ReadDatabaseFromFile(GetDefaultDatabaseFilename())
			       	? lastLoadedDatabase
			       	: null;
		}

		bool ReadDatabaseFromFile(string filename)
		{
			if (string.IsNullOrEmpty(filename)) return false;

			var fileInfo = new FileInfo(filename);

			// If we are looking at the same file and it hasn't been updated then just reuse it
			if (lastLoadedDatabase != null && fileInfo.FullName == lastLoadedDatabase.Filename && fileInfo.LastWriteTime == lastLoadedDatabase.Timestamp)
			{
				Debug.Assert(lastLoadedDatabase != null);
				return true;
			}

			try
			{
				lastLoadedDatabase = BibTexHelper.LoadBibTexDatabase(filename);

				return lastLoadedDatabase != null;
			}
			catch
			{}

			return false;
		}
	}
}