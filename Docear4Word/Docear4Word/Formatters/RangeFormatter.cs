using System.Runtime.InteropServices;

using Word;

namespace Docear4Word
{
	public delegate void RangeProcessor(Range range);

	public delegate void TagProcessor(string tag, Range range);

	[ComVisible(false)]
	public class RangeFormatter
	{
		const string BoldTag = "b";
		const string ItalicTag = "i";
		const string UnderlineTag = "u";
		const string SubscriptTag = "sub";
		const string SuperscriptTag = "sup";
		const string ObliqueTag = "em";
		const string SmallCapsTag = "smallcaps";
		const string ParagraphTag = "p";

		const string BlockTag = "csl-block";
		const string LeftMarginTag = "csl-left-margin";
		const string RightInlineTag = "csl-right-inline";
		const string IndentTag = "csl-indent";

		const string SecondFieldAlignTag = "second-field-align";
		const string HangingIndentTag = "hanging-indent";

		public void AssignHtml(Range targetRange, string html)
		{
			html = HtmlHelper.DecodeHtml(html);

			// Quick return for empty markup
			if (html.Length == 0)
			{
				targetRange.Text = html;
				return;
			}

			targetRange.Text = html;
			targetRange.NoProofing = 1;

			var outerRange = targetRange.Duplicate;

			RemoveTextFormatting(outerRange);

			var longestFirstFieldLength = 0;

			ProcessTags(LeftMarginTag, outerRange, (tag, range) =>
			                                       	{
			                                       	}
				);

			ProcessTags(RightInlineTag, outerRange, (tag, range) =>
			                                        	{
			                                        		range.Text = "\t" + range.Text.Trim();
			                                  	}
				);

			ProcessTags(BlockTag, outerRange, (tag, range) =>
			                                  	{
			                                  		range.ParagraphFormat.LeftIndent = 40;
			                                  	}
				);

			ProcessTags(IndentTag, outerRange, (tag, range) =>
			                                  	{
	                                        		range.Text = "\t" + range.Text.Trim();
			                                  	}
				);



			ProcessTags(BoldTag, outerRange, (tag, range) => range.Bold = 1);
			ProcessTags(ItalicTag, outerRange, (tag, range) => range.Italic = 1);
			ProcessTags(ObliqueTag, outerRange, (tag, range) => range.Italic = 1);
			ProcessTags(UnderlineTag, outerRange, (tag, range) => range.Underline = WdUnderline.wdUnderlineSingle);
			ProcessTags(SuperscriptTag, outerRange, (tag, range) => range.Font.Superscript = 1);
			ProcessTags(SubscriptTag, outerRange, (tag, range) => range.Font.Subscript = 1);
			ProcessTags(SmallCapsTag, outerRange, (tag, range) => range.Font.SmallCaps = 1);

			var secondFieldAlignFound = ProcessTags(SecondFieldAlignTag, outerRange, (tag, range) =>
			        {
						longestFirstFieldLength++;			                                                                  		
			        }
				);

			var hangingIndent = ProcessTags(HangingIndentTag, outerRange, (tag, range) =>
			        {
						outerRange.ParagraphFormat.FirstLineIndent = 0;
						outerRange.ParagraphFormat.LeftIndent = 0;
						outerRange.ParagraphFormat.TabHangingIndent(1);
			        }
				);

			if (secondFieldAlignFound || hangingIndent)
			{
				outerRange.ParagraphFormat.FirstLineIndent = 0;
				outerRange.ParagraphFormat.LeftIndent = 0;
				outerRange.ParagraphFormat.RightIndent = 0;
			}

			if (secondFieldAlignFound && longestFirstFieldLength > 0)
			{
				outerRange.ParagraphFormat.TabStops.ClearAll();
				outerRange.ParagraphFormat.TabStops.Add(4 + 6 * longestFirstFieldLength);
			}

			ProcessTags(ParagraphTag, outerRange, (tag, range) => range.InsertParagraph());
		}

		static void DeleteChars(Range range, int offset)
		{
			DeleteCharsAt(range, 0, offset);
		}

		static void DeleteCharsAt(Range range, int offset, int length)
		{
			var clone = range.Duplicate;

			clone.Start = range.Start + offset;
			clone.End = range.Start + offset + length;
			clone.Text = string.Empty;
		}

		static bool ProcessTags(string tag, Range tagRange, TagProcessor tagProcessor, bool isSingle = false)
		{
			var startTag = "<" + tag + ">";
			var endTag = "</" + tag + ">";

			var found = false;
			var range = tagRange.Duplicate;
			int startTagOffset;

			while(range.Start < range.End && (startTagOffset = range.Text.IndexOf(startTag)) != -1)
			{
				found = true;

				// Remove start tag
				range.Start += startTagOffset;
				DeleteChars(range, startTag.Length);

				if (!isSingle)
				{
					// Remove end tag
					var endTagOffset = range.Text.IndexOf(endTag);
					DeleteCharsAt(range, endTagOffset, endTag.Length);
					range.End = range.Start + endTagOffset;
				}
				else
				{
					range.End = range.Start;
				}

				// Process at tag position
				tagProcessor(tag, range);

				range.Start = tagRange.Start;
				range.End = tagRange.End;
			}

			return found;
		}

		static void RemoveTextFormatting(Range range)
		{
			range.Bold = 0;
			range.Italic = 0;
			range.Underline = WdUnderline.wdUnderlineNone;
			range.Font.Subscript = 0;
			range.Font.Superscript = 0;
			range.Font.SmallCaps = 0;
		}
	}
}