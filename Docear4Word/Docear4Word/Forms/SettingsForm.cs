using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Docear4Word.Forms
{
	[ComVisible(false)]
	public partial class SettingsForm: Form
	{
		public SettingsForm()
		{
			InitializeComponent();
		}

		void SettingsForm_Load(object sender, EventArgs e)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

			Text = string.Format("Docear4Word v{0}.{1}{2} Settings", fileVersionInfo.ProductMajorPart, fileVersionInfo.ProductMinorPart, fileVersionInfo.ProductBuildPart);

			var environmentVariableFilename = Environment.GetEnvironmentVariable(Settings.DatabaseEnvironmentVariableName, EnvironmentVariableTarget.User);
			lblEnvironmentVariable.Text = string.Format("(currently: {0})", environmentVariableFilename ?? "(not specified)");
		}

		public bool UseDocearDefaultDatabase
		{
			get { return rbDocearDefault.Checked; }
			set
			{
				if (value)
				{
					rbDocearDefault.Checked = true;
				}
				else
				{
					rbCustom.Checked = true;
				}
			}
		}

		public bool RefreshUpdatesCitationsFromDatabase
		{
			get { return chkRefreshUpdatesCitationsFromDatabase.Checked; }
			set { chkRefreshUpdatesCitationsFromDatabase.Checked = value; }
		}

		public string CustomDatabaseFilename
		{
			get { return lblCustomDataFilename.Text; }
			set { lblCustomDataFilename.Text = value; }
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			                     	{
			                     		Title = "Choose BibTex database...",
										ValidateNames = true,
			                     	};

			if (!string.IsNullOrEmpty(CustomDatabaseFilename))
			{
				try
				{
					var fileInfo = new FileInfo(CustomDatabaseFilename);

					openFileDialog.InitialDirectory = fileInfo.DirectoryName;
					openFileDialog.FileName = fileInfo.Name;
				}
				catch
				{}
			}

			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				var newDatabase = BibTexHelper.LoadBibTexDatabase(openFileDialog.FileName);
				if (newDatabase == null)
				{
					Helper.ShowCorruptBibtexDatabaseMessage(openFileDialog.FileName);
					return;
				}

				CustomDatabaseFilename = openFileDialog.FileName;
				UseDocearDefaultDatabase = false;
			}
		}
	}
}
