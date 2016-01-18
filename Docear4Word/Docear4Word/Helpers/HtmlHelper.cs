using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Docear4Word
{
	public static class HtmlHelper
	{
		static readonly char[] EntityEndingChars = new[] { ';', '&' };

		static readonly String[] EntitiesList = new[]
		                                        	{
		                                        		"\x0022-quot",
		                                        		"\x0026-amp",
		                                        		"\x003c-lt",
		                                        		"\x003e-gt",
		                                        		"\x00a0-nbsp",
		                                        		"\x00a1-iexcl",
		                                        		"\x00a2-cent",
		                                        		"\x00a3-pound",
		                                        		"\x00a4-curren",
		                                        		"\x00a5-yen",
		                                        		"\x00a6-brvbar",
		                                        		"\x00a7-sect",
		                                        		"\x00a8-uml",
		                                        		"\x00a9-copy",
		                                        		"\x00aa-ordf",
		                                        		"\x00ab-laquo",
		                                        		"\x00ac-not",
		                                        		"\x00ad-shy",
		                                        		"\x00ae-reg",
		                                        		"\x00af-macr",
		                                        		"\x00b0-deg",
		                                        		"\x00b1-plusmn",
		                                        		"\x00b2-sup2",
		                                        		"\x00b3-sup3",
		                                        		"\x00b4-acute",
		                                        		"\x00b5-micro",
		                                        		"\x00b6-para",
		                                        		"\x00b7-middot",
		                                        		"\x00b8-cedil",
		                                        		"\x00b9-sup1",
		                                        		"\x00ba-ordm",
		                                        		"\x00bb-raquo",
		                                        		"\x00bc-frac14",
		                                        		"\x00bd-frac12",
		                                        		"\x00be-frac34",
		                                        		"\x00bf-iquest",
		                                        		"\x00c0-Agrave",
		                                        		"\x00c1-Aacute",
		                                        		"\x00c2-Acirc",
		                                        		"\x00c3-Atilde",
		                                        		"\x00c4-Auml",
		                                        		"\x00c5-Aring",
		                                        		"\x00c6-AElig",
		                                        		"\x00c7-Ccedil",
		                                        		"\x00c8-Egrave",
		                                        		"\x00c9-Eacute",
		                                        		"\x00ca-Ecirc",
		                                        		"\x00cb-Euml",
		                                        		"\x00cc-Igrave",
		                                        		"\x00cd-Iacute",
		                                        		"\x00ce-Icirc",
		                                        		"\x00cf-Iuml",
		                                        		"\x00d0-ETH",
		                                        		"\x00d1-Ntilde",
		                                        		"\x00d2-Ograve",
		                                        		"\x00d3-Oacute",
		                                        		"\x00d4-Ocirc",
		                                        		"\x00d5-Otilde",
		                                        		"\x00d6-Ouml",
		                                        		"\x00d7-times",
		                                        		"\x00d8-Oslash",
		                                        		"\x00d9-Ugrave",
		                                        		"\x00da-Uacute",
		                                        		"\x00db-Ucirc",
		                                        		"\x00dc-Uuml",
		                                        		"\x00dd-Yacute",
		                                        		"\x00de-THORN",
		                                        		"\x00df-szlig",
		                                        		"\x00e0-agrave",
		                                        		"\x00e1-aacute",
		                                        		"\x00e2-acirc",
		                                        		"\x00e3-atilde",
		                                        		"\x00e4-auml",
		                                        		"\x00e5-aring",
		                                        		"\x00e6-aelig",
		                                        		"\x00e7-ccedil",
		                                        		"\x00e8-egrave",
		                                        		"\x00e9-eacute",
		                                        		"\x00ea-ecirc",
		                                        		"\x00eb-euml",
		                                        		"\x00ec-igrave",
		                                        		"\x00ed-iacute",
		                                        		"\x00ee-icirc",
		                                        		"\x00ef-iuml",
		                                        		"\x00f0-eth",
		                                        		"\x00f1-ntilde",
		                                        		"\x00f2-ograve",
		                                        		"\x00f3-oacute",
		                                        		"\x00f4-ocirc",
		                                        		"\x00f5-otilde",
		                                        		"\x00f6-ouml",
		                                        		"\x00f7-divide",
		                                        		"\x00f8-oslash",
		                                        		"\x00f9-ugrave",
		                                        		"\x00fa-uacute",
		                                        		"\x00fb-ucirc",
		                                        		"\x00fc-uuml",
		                                        		"\x00fd-yacute",
		                                        		"\x00fe-thorn",
		                                        		"\x00ff-yuml",
		                                        		"\x0152-OElig",
		                                        		"\x0153-oelig",
		                                        		"\x0160-Scaron",
		                                        		"\x0161-scaron",
		                                        		"\x0178-Yuml",
		                                        		"\x0192-fnof",
		                                        		"\x02c6-circ",
		                                        		"\x02dc-tilde",
		                                        		"\x0391-Alpha",
		                                        		"\x0392-Beta",
		                                        		"\x0393-Gamma",
		                                        		"\x0394-Delta",
		                                        		"\x0395-Epsilon",
		                                        		"\x0396-Zeta",
		                                        		"\x0397-Eta",
		                                        		"\x0398-Theta",
		                                        		"\x0399-Iota",
		                                        		"\x039a-Kappa",
		                                        		"\x039b-Lambda",
		                                        		"\x039c-Mu",
		                                        		"\x039d-Nu",
		                                        		"\x039e-Xi",
		                                        		"\x039f-Omicron",
		                                        		"\x03a0-Pi",
		                                        		"\x03a1-Rho",
		                                        		"\x03a3-Sigma",
		                                        		"\x03a4-Tau",
		                                        		"\x03a5-Upsilon",
		                                        		"\x03a6-Phi",
		                                        		"\x03a7-Chi",
		                                        		"\x03a8-Psi",
		                                        		"\x03a9-Omega",
		                                        		"\x03b1-alpha",
		                                        		"\x03b2-beta",
		                                        		"\x03b3-gamma",
		                                        		"\x03b4-delta",
		                                        		"\x03b5-epsilon",
		                                        		"\x03b6-zeta",
		                                        		"\x03b7-eta",
		                                        		"\x03b8-theta",
		                                        		"\x03b9-iota",
		                                        		"\x03ba-kappa",
		                                        		"\x03bb-lambda",
		                                        		"\x03bc-mu",
		                                        		"\x03bd-nu",
		                                        		"\x03be-xi",
		                                        		"\x03bf-omicron",
		                                        		"\x03c0-pi",
		                                        		"\x03c1-rho",
		                                        		"\x03c2-sigmaf",
		                                        		"\x03c3-sigma",
		                                        		"\x03c4-tau",
		                                        		"\x03c5-upsilon",
		                                        		"\x03c6-phi",
		                                        		"\x03c7-chi",
		                                        		"\x03c8-psi",
		                                        		"\x03c9-omega",
		                                        		"\x03d1-thetasym",
		                                        		"\x03d2-upsih",
		                                        		"\x03d6-piv",
		                                        		"\x2002-ensp",
		                                        		"\x2003-emsp",
		                                        		"\x2009-thinsp",
		                                        		"\x200c-zwnj",
		                                        		"\x200d-zwj",
		                                        		"\x200e-lrm",
		                                        		"\x200f-rlm",
		                                        		"\x2013-ndash",
		                                        		"\x2014-mdash",
		                                        		"\x2018-lsquo",
		                                        		"\x2019-rsquo",
		                                        		"\x201a-sbquo",
		                                        		"\x201c-ldquo",
		                                        		"\x201d-rdquo",
		                                        		"\x201e-bdquo",
		                                        		"\x2020-dagger",
		                                        		"\x2021-Dagger",
		                                        		"\x2022-bull",
		                                        		"\x2026-hellip",
		                                        		"\x2030-permil",
		                                        		"\x2032-prime",
		                                        		"\x2033-Prime",
		                                        		"\x2039-lsaquo",
		                                        		"\x203a-rsaquo",
		                                        		"\x203e-oline",
		                                        		"\x2044-frasl",
		                                        		"\x20ac-euro",
		                                        		"\x2111-image",
		                                        		"\x2118-weierp",
		                                        		"\x211c-real",
		                                        		"\x2122-trade",
		                                        		"\x2135-alefsym",
		                                        		"\x2190-larr",
		                                        		"\x2191-uarr",
		                                        		"\x2192-rarr",
		                                        		"\x2193-darr",
		                                        		"\x2194-harr",
		                                        		"\x21b5-crarr",
		                                        		"\x21d0-lArr",
		                                        		"\x21d1-uArr",
		                                        		"\x21d2-rArr",
		                                        		"\x21d3-dArr",
		                                        		"\x21d4-hArr",
		                                        		"\x2200-forall",
		                                        		"\x2202-part",
		                                        		"\x2203-exist",
		                                        		"\x2205-empty",
		                                        		"\x2207-nabla",
		                                        		"\x2208-isin",
		                                        		"\x2209-notin",
		                                        		"\x220b-ni",
		                                        		"\x220f-prod",
		                                        		"\x2211-sum",
		                                        		"\x2212-minus",
		                                        		"\x2217-lowast",
		                                        		"\x221a-radic",
		                                        		"\x221d-prop",
		                                        		"\x221e-infin",
		                                        		"\x2220-ang",
		                                        		"\x2227-and",
		                                        		"\x2228-or",
		                                        		"\x2229-cap",
		                                        		"\x222a-cup",
		                                        		"\x222b-int",
		                                        		"\x2234-there4",
		                                        		"\x223c-sim",
		                                        		"\x2245-cong",
		                                        		"\x2248-asymp",
		                                        		"\x2260-ne",
		                                        		"\x2261-equiv",
		                                        		"\x2264-le",
		                                        		"\x2265-ge",
		                                        		"\x2282-sub",
		                                        		"\x2283-sup",
		                                        		"\x2284-nsub",
		                                        		"\x2286-sube",
		                                        		"\x2287-supe",
		                                        		"\x2295-oplus",
		                                        		"\x2297-otimes",
		                                        		"\x22a5-perp",
		                                        		"\x22c5-sdot",
		                                        		"\x2308-lceil",
		                                        		"\x2309-rceil",
		                                        		"\x230a-lfloor",
		                                        		"\x230b-rfloor",
		                                        		"\x2329-lang",
		                                        		"\x232a-rang",
		                                        		"\x25ca-loz",
		                                        		"\x2660-spades",
		                                        		"\x2663-clubs",
		                                        		"\x2665-hearts",
		                                        		"\x2666-diams"
		                                        	};

		static readonly Dictionary<string, char> EntityLookup;

		static HtmlHelper()
		{
			EntityLookup = new Dictionary<string, char>(EntitiesList.Length);

			foreach (var pair in EntitiesList)
			{
				EntityLookup[pair.Substring(2)] = pair[0];
			}
		}

		static char LookupEntity(string entity)
		{
			char result;

			if (!EntityLookup.TryGetValue(entity, out result))
			{
				result = (char) 0;
			}

			return result;
		}

		public static string DecodeHtml(string html)
		{
			if (html == null || html.IndexOf('&') == -1) return html;

			var builder = new StringBuilder(html.Length);
			var writer = new StringWriter(builder);

			DecodeHtmlCode(html, writer);

			return builder.ToString();
		}

		public static void DecodeHtml(string html, TextWriter writer)
		{
			if (html == null) return;

			if (html.IndexOf('&') == -1)
			{
				writer.Write(html);
				return;
			}

			DecodeHtmlCode(html, writer);
		}

		static void DecodeHtmlCode(string html, TextWriter output)
		{
			for (var i = 0; i < html.Length; i++)
			{
				var ch = html[i];

				if (ch == '&')
				{
					// We found a '&'. Now look for the next ';' or '&'. The idea is that
					// if we find another '&' before finding a ';', then this is not an entity, 
					// and the next '&' might start a real entity (VSWhidbey 275184)
					var index = html.IndexOfAny(EntityEndingChars, i + 1);

					if (index > 0 && html[index] == ';')
					{
						var entity = html.Substring(i + 1, index - i - 1);

						if (entity.Length > 1 && entity[0] == '#')
						{
							try
							{
								// The # syntax can be in decimal or hex, e.g.
								//      &#229;  --> decimal 
								//      &#xE5;  --> same char in hex
								// See http://www.w3.org/TR/REC-html40/charset.html#entities
								ch = entity[1] == 'x' || entity[1] == 'X'
								     	? (char) Int32.Parse(entity.Substring(2), NumberStyles.AllowHexSpecifier)
								     	: (char) Int32.Parse(entity.Substring(1));

								i = index; // already looked at everything until semicolon 
							}
							catch (FormatException)
							{
								i++; // if the number isn't valid, ignore it
							}
							catch (ArgumentException)
							{
								i++; // if there is no number, ignore it. 
							}
						}
						else
						{
							i = index; // already looked at everything until semicolon

							var entityChar = LookupEntity(entity);

							if (entityChar != (char) 0)
							{
								ch = entityChar;
							}
							else
							{
								output.Write('&');
								output.Write(entity);
								output.Write(';');
								continue;
							}
						}
					}
				}

				output.Write(ch);
			}
		}

		public static string CreateHtmlClipboardData(string html)
		{
			const string Header =
				@"
Version: 1.0
StartHTML: {0:000000}
EndHTML: {1:000000}
StartFragment: {2:000000}
EndFragment: {3:000000}
";

			const string HTMLSuffix = @"
<!--EndFragment-->
</body>
</html>
";

			var encoding = Encoding.UTF8;

			var htmlPrefix =
				@"
!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//TR""
<html>
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset={0}"" />
</head>
<body>
<!--StartFragment-->
";
			htmlPrefix = string.Format(htmlPrefix, encoding.WebName);

			var sb = new StringBuilder();

			// Get lengths of chunks
			var headerLength = encoding.GetByteCount(Header) - 16; // extra formatting characters {0:000000}
			var prefixLength = encoding.GetByteCount(htmlPrefix);
			var htmlLength = encoding.GetByteCount(html);
			var suffixLength = encoding.GetByteCount(HTMLSuffix);

			// Determine locations of chunks
			var startHtml = headerLength;
			var startFragment = startHtml + prefixLength;
			var endFragment = startFragment + htmlLength;
			var endHtml = endFragment + suffixLength;

			// Build the data
			sb.AppendFormat(Header, startHtml, endHtml, startFragment, endFragment);
			sb.Append(htmlPrefix);
			sb.Append(html);
			sb.Append(HTMLSuffix);
//MessageBox.Show(sb.ToString());
//Console.WriteLine(sb.ToString());
//			File.WriteAllText(@"F:\out.tmp", sb.ToString());
			return sb.ToString();
		}

/*
		public static string CreateHtmlClipboardData(string html)
		{
			var sb = new StringBuilder();
			Encoding encoding = Encoding.GetEncoding("utf-8");
			string Header =
				@"
Version: 1.0
StartHTML: {0:000000}
EndHTML: {1:000000}
StartFragment: {2:000000}
EndFragment: {3:000000}
";
			string HtmlPrefix =
				@"
!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//TR""
html
head
meta http-equiv=Content-Type content=""text/html; charset={0}""
head
body
!--StartFragment--
";
			HtmlPrefix = string.Format(HtmlPrefix, encoding.WebName);

			string HtmlSuffix = @"
<!--EndFragment-->
</body>
</html>
";

// Get lengths of chunks
			int HeaderLength = encoding.GetByteCount(Header);
			HeaderLength -= 16; // extra formatting characters {0:000000}
			int PrefixLength = encoding.GetByteCount(HtmlPrefix);
			int HtmlLength = encoding.GetByteCount(html);
			int SuffixLength = encoding.GetByteCount(HtmlSuffix);

// Determine locations of chunks
			int StartHtml = HeaderLength;
			int StartFragment = StartHtml + PrefixLength;
			int EndFragment = StartFragment + HtmlLength;
			int EndHtml = EndFragment + SuffixLength;

// Build the data
			sb.AppendFormat(Header, StartHtml, EndHtml, StartFragment, EndFragment);
			sb.Append(HtmlPrefix);
			sb.Append(html);
			sb.Append(HtmlSuffix);
			MessageBox.Show(sb.ToString());
//Console.WriteLine(sb.ToString());
			return sb.ToString();
		}
*/

		public static string EncodeHighChars(string text)
		{
			if (text == null) return null;

			var pos = GetIndexOfHighChar(text, 0);
			if (pos == -1)
			{
				return text;
			}

			var sb = new StringBuilder();
			var startPos = 0;

			while(true)
			{
				if (pos > startPos)
				{
					sb.Append(text, startPos, pos - startPos);
				}

				sb.Append("&#");
				sb.Append(((int) text[pos]).ToString(NumberFormatInfo.InvariantInfo));
				sb.Append(";");

				startPos = pos + 1;
				if (startPos == text.Length) break;

				pos = GetIndexOfHighChar(text, startPos);
				if (pos == -1)
				{
					sb.Append(text, startPos, text.Length - startPos);
					break;
				}
			}

			return sb.ToString();
		}

		static int GetIndexOfHighChar(string s, int pos)
		{
			for(; pos < s.Length; pos++)
			{
				var ch = s[pos];
				if (ch >= 160/* && ch < 256*/) return pos;
			}

			return -1;
		}
	}
}