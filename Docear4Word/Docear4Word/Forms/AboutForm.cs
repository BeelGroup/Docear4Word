using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Docear4Word.Forms
{
	[ComVisible(false)]
	public partial class AboutForm: Form
	{
		public AboutForm()
		{
			InitializeComponent();
		}

		void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var text = ((LinkLabel) sender).Text;

			try
			{
				Process.Start(text);
			}
			catch {}
		}

		void AboutForm_Load(object sender, EventArgs e)
		{
			llLicence.Text = Path.Combine(FolderHelper.ApplicationRootDirectory, "licence.txt");
			llCitationStyleFolder.Text = FolderHelper.DocearStylesFolder;
			llBibTexFileFolder.Text = Settings.Instance.GetDefaultDatabaseFolder();
			llPersonalDataFolder.Text = FolderHelper.DocearPersonalDataFolder;
			llApplicationFolder.Text = FolderHelper.ApplicationRootDirectory;

			var assembly = Assembly.GetExecutingAssembly();
			var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

			Text = string.Format("About Docear4Word v{0}.{1}{2}", fileVersionInfo.ProductMajorPart, fileVersionInfo.ProductMinorPart, fileVersionInfo.ProductBuildPart);

			lblCopyright.Text = string.Format("(C) 2012-{0} Docear by Joeran Beel, Stefan Langer, Marcel Genzmehr, and others", DateTime.Today.Year);

			lblCiteProc.Text = lblCiteProc.Text + "\r\n(v" + CiteProcRunner.ProcessorVersion + ")";
		}
	}
}
