using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Docear4Word
{
	[ComVisible(false)]
	public class StyleInfo
	{
		public FileInfo FileInfo { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }

		public string GetXml()
		{
			return File.ReadAllText(FileInfo.FullName);
		}

		public override string ToString()
		{
			return Title;
		}
	}
}