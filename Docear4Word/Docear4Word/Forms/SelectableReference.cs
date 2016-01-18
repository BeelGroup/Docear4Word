using System;
using System.Runtime.InteropServices;

namespace Docear4Word
{
	[ComVisible(false)]
	public class SelectableReference
	{
		bool selected;
		string id;
		string title;
		string authors;
		string year;
		string timestamp;
		string pages;

		public string ID
		{
			get { return id; }
			set { id = value; }
		}

		public bool Selected
		{
			get { return selected; }
			set { selected = value; }
		}

		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		public string Authors
		{
			get { return authors; }
			set { authors = value; }
		}

		public string Year
		{
			get { return year; }
			set { year = value; }
		}

		public string Timestamp
		{
			get { return timestamp; }
			set { timestamp = value; }
		}

		public string Pages
		{
			get { return pages; }
			set { pages = value; }
		}
	}
}