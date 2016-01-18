using System;
using System.Text;

using Word;

namespace Docear4Word
{
	public class BibliographyRangeFormatter: RangeFormatter
	{
		readonly BibliographyResult bibliographyResult;
		readonly string bibliographyHtml;
		

		public BibliographyRangeFormatter(BibliographyResult bibliographyResult)
		{
			this.bibliographyResult = bibliographyResult;

			var sb = new StringBuilder();

			foreach(var entry in bibliographyResult.Entries)
			{
				if (entry.StartsWith("---"))
				{
/*
					var x = entry.Substring(3);
					if (x.StartsWith(".")) x = x.Substring(1);
					x = x.TrimStart();
					sb[sb.Length - 1] = '\b';
					sb.Append(x);
					continue;
*/
				}

				//sb.Append(entry);
				sb.Append(entry.Replace("\n\n", "\n"));
			}

			bibliographyHtml = sb.ToString();

			bibliographyHtml = HtmlHelper.EncodeHighChars(bibliographyHtml);

		}
/*
		public BibliographyRangeFormatter(BibliographyResult bibliographyResult)
		{
			this.bibliographyResult = bibliographyResult;

			var sb = new StringBuilder();

			foreach(var entry in bibliographyResult.Entries)
			{
				sb.Append(entry);
			}

			bibliographyHtml = sb.ToString();

			bibliographyHtml = HtmlHelper.EncodeHighChars(bibliographyHtml);

		}
*/

		public BibliographyResult BibliographyResult
		{
			get { return bibliographyResult; }
		}

		public string BibliographyHtml
		{
			get { return bibliographyHtml; }
		}

		public void CreateBibliography(Range range)
		{
			range.Text = string.Empty;

			FormatBibliographyParagraph(range.ParagraphFormat);

			AssignHtml(range, bibliographyHtml);
		}

		void FormatBibliographyParagraph(ParagraphFormat format)
		{
			format.Reset();

			format.FirstLineIndent = 0;
			format.LeftIndent = 0;

			if (bibliographyResult.HangingIndent)
			{
				format.TabHangingIndent(1);
			}
			else
			{
				var isMarginAlign = bibliographyResult.SecondFieldAlign == "margin";
				var isFlushAlign = bibliographyResult.SecondFieldAlign == "flush";

				if (isFlushAlign || isMarginAlign)
				{
					var tabOffset = 4 + 6 * bibliographyResult.MaxOffset;

					// Use a tab to set the hanging indent
					// then remove it
					format.TabStops.Add(tabOffset);
					format.TabHangingIndent(1);
					format.TabStops.ClearAll();

					if (isMarginAlign)
					{
						format.FirstLineIndent -= tabOffset;
						format.LeftIndent -= tabOffset;
					}
				}
			}

			//if (bibliographyResult.LineSpacing != 1)
			//{
				format.LineSpacingRule = WdLineSpacing.wdLineSpaceMultiple;
				format.LineSpacing = format.Application.LinesToPoints(bibliographyResult.LineSpacing);
			//}

			format.SpaceAfter = format.LineSpacing * bibliographyResult.EntrySpacing;
		}
	}
}