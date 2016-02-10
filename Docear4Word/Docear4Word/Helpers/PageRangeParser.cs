using System;
using System.Text.RegularExpressions;

namespace Docear4Word
{
	public class PageRangeParser
	{
        static readonly Regex Parser = new Regex(@"(\d+)(?:\s*[-¡V]{1,2}\s*(\d+)\s*(?:\(\d+\))?)?");

		readonly string originalPages;
		readonly string originalNumPages;
		readonly string page;
		readonly string pageFirst;
		readonly string numberOfPages;

		public PageRangeParser(string originalPages, string originalNumPages)
		{
			this.originalPages = originalPages;
			this.originalNumPages = originalNumPages;

			// Assume no match so we can return quickly
			page = originalPages;
			numberOfPages = originalNumPages;
			if (string.IsNullOrEmpty(originalPages)) return;

			var matches = Parser.Matches(originalPages);

			// Only parse for exactly one match
			if (matches.Count != 1) return;

			var match = matches[0];
			var componentCount = match.Groups.Count - 1;

			// Get the first page (quick return if not valid)
			int firstPage;
			int.TryParse(match.Groups[1].Value, out firstPage);
			if (firstPage == 0) return;

			pageFirst = firstPage.ToString();

			// Quick return if nothing more to do;
			if (componentCount == 1)
			{
				page = firstPage.ToString();
				return;
			}

			// Get the second page (quick return if not valid)
			int secondPage;
			int.TryParse(match.Groups[2].Value, out secondPage);
			if (secondPage == 0) return;

			// Did the database entry have the page count in braces (and not specified set in the numberOfPages tag?
			if (componentCount == 3 && string.IsNullOrEmpty(numberOfPages))
			{
				// Yes, so use it if valid (calculate it otherwise)
				int pageCount;
				int.TryParse(match.Groups[3].Value, out pageCount);
				numberOfPages = pageCount != 0
				                	? pageCount.ToString()
				                	: (secondPage - firstPage + 1).ToString();

				// And use this format
				page = string.Format("{0}-{1} ({2})", firstPage, secondPage, numberOfPages);

				return;
			}

			// Calculate the number of pages (non-negative!)
			if (string.IsNullOrEmpty(numberOfPages) && firstPage > secondPage)
			{
				numberOfPages = (secondPage - firstPage + 1).ToString();
			}

			// And use simple range format
			page = string.Format("{0}-{1}", firstPage, secondPage);
		}

		public string OriginalPages
		{
			get { return originalPages; }
		}

		public string OriginalNumPages
		{
			get { return originalNumPages; }
		}

		public string Page
		{
			get { return page; }
		}

		public string PageFirst
		{
			get { return pageFirst; }
		}

		public string NumberOfPages
		{
			get { return numberOfPages; }
		}

		public override string ToString()
		{
			return string.Format("'{0}', '{1}' => '{2}', '{3}', '{4}'", originalPages, originalNumPages, page, pageFirst, numberOfPages);
		}
	}
}