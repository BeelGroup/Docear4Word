using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Docear4Word.Forms
{
	[ComVisible(false)]
	public partial class DialogFooter: UserControl
	{
		public DialogFooter()
		{
			InitializeComponent();
		}

		private void llDocearHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var link = (string) ((Control) sender).Tag;

			try
			{
				Process.Start(link);
			}
			catch {}
		}
	}
}
