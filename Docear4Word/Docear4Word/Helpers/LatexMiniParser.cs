using System;
using System.Globalization;
using System.Text;

namespace Docear4Word
{
	public class LatexMiniParser
	{
		const char EOF = (char) 0;
		const string UnicharCommand = "unichar";
		const string VerbCommand = "verb";
		const string AccentPrefixes = @"`'^""~cvu=.Hbd"; // May need to add some http://en.wikibooks.org/wiki/LaTeX/Accents

		readonly string data;
		readonly bool preserveGroupBraces;
		int position;

		public LatexMiniParser(string data, bool preserveGroupBraces = false)
		{
			this.data = data;
			this.preserveGroupBraces = preserveGroupBraces;
		}

		public string Parse()
		{
			var sb = new StringBuilder(data.Length);

			nextChar:
			while(position < data.Length)
			{
				int accentIndex;
				var currentChar = data[position];

				switch (currentChar)
				{
					case '{':
						if (preserveGroupBraces) break;

						// Special test for accented chars
						// As far as I can tell, these should all be preceeded with a slash
						// but the docear database containg some in the form of 
						//{H}{"o}ding
						// so this section deals with those
						//
						// Also {"s} doesn't appear to be standard but appears in
						// Docear.bib so SymbolHelper has added this too
						// {S}tra{"s}burger -> Straßburger
						//
						if (LookAhead(3) == '}')
						{
							accentIndex = AccentPrefixes.IndexOf(LookAhead(1));

							if (accentIndex != -1)
							{
								var accentedChar = SymbolHelper.GetAccentedChar(AccentPrefixes[accentIndex], LookAhead(2));
								if (accentedChar != 0)
								{
									sb.Append(accentedChar);
									position += 4;
									goto nextChar;
								}
							}
						}

						position++;
						goto nextChar;

					case '}':

						if (preserveGroupBraces) break;
						
						position++;
						goto nextChar;

					case '~':
						currentChar = '\xa0'; // Non-breaking space
						break;

					case '-':
						if (LookAhead(1) == '-')
						{
							if (LookAhead(2) == '-')
							{
								currentChar = '—';
								position += 2;
							}
							else
							{
								currentChar = '–';
								position++;
							}
						}
						break;

					case '`':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '“';
							position++;
						}
						else
						{
							currentChar = '‘';
						}
						break;

					case '\'':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '”';
							position++;
						}
						else
						{
							currentChar = '’';
						}
						break;

					case '?':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¿';
						}
						break;

					case '!':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¡';
						}
						break;

					case '$':
						position++;

						if (LookAhead() != '$') goto nextChar;
						break;

					case '\r': case '\n': case '\t': case ' ':
						sb.Append(' ');

						char n;
						do
						{
							position++;

							n = LookAhead();
						} while (n == ' ' || n == '\r' || n == '\n' || n == '\t');
						goto nextChar;

					case '\\':
						var la = LookAhead(1);

						position++;

						if (la == '\\') break;

						if (" &#$%_,{}".IndexOf(la) != -1)
						{
							currentChar = la;
							break;
						}

						accentIndex = AccentPrefixes.IndexOf(la);
						if (accentIndex != -1)
						{
							la = LookAhead(1);

							if (la == '{')
							{
								la = LookAhead(2);
							}

							var accentedChar = SymbolHelper.GetAccentedChar(AccentPrefixes[accentIndex], la);
							if (accentedChar != 0)
							{
								sb.Append(accentedChar);
								position += LookAhead(1) == '{' ? 4 : 2;
								goto nextChar;
							}
						}

						var wordEnd = position;

						while(wordEnd + 1 < data.Length)
						{
							var c = char.ToLowerInvariant(data[wordEnd + 1]);
							if (c < 'a' || c > 'z') break;

							wordEnd++;
						}
						if (wordEnd == data.Length) break;

						var word = data.Substring(position, wordEnd - position + 1);

						if (word == VerbCommand)
						{
							position += VerbCommand.Length;
							var delimiterChar = LookAhead();
							position++;

							var count = 0;

							while(count < data.Length && LookAhead(count) != delimiterChar) count++;
							sb.Append(data, position, count);
							position += count;
							position++;
							goto nextChar;
						}

						if (word == UnicharCommand)
						{
							position += UnicharCommand.Length;
							position++; // {

							var number = string.Empty;
							var isHex = false;

							currentChar = LookAhead();

							while (currentChar != '}' && currentChar != EOF)
							{
								if (char.IsDigit(currentChar))
								{
									number += currentChar;
								}
								else if ((isHex || number.Length > 0) && "ABCDEFabcdef".IndexOf(currentChar) != -1)
								{
									number += currentChar;
									isHex = true;
								}

								position++;

								currentChar = LookAhead();
							}

							position++;

							if (currentChar == EOF) goto nextChar;
							
							int unicode;
							if (int.TryParse(number, isHex ? NumberStyles.HexNumber : NumberStyles.Integer, null, out unicode) && unicode < char.MaxValue)
							{
								sb.Append((char) unicode);
							}
							else
							{
								sb.Append("?");
							}

							goto nextChar;
						}

						var replacement = SymbolHelper.GetReplacementFor(word);
						if (replacement != null)
						{
							sb.Append(replacement);
position += word.Length;
goto nextChar;
						}

//						position += word.Length;
						if (accentIndex != -1) position += word.Length;
						goto nextChar;

				}

				sb.Append(currentChar);
				position++;
			}

			return sb.ToString();
		}

		char LookAhead(int distance = 0)
		{
			var aheadPosition = position + distance;

			if (aheadPosition >= data.Length) return EOF;

			return data[aheadPosition];
		}
	}
/*
	public class LatexMiniParser
	{
		const char EOF = (char) 0;
		const string UnicharCommand = "unichar";
		const string VerbCommand = "verb";
		const string AccentPrefixes = @"`'^""~cvu=.Hbd";

		readonly string data;
		readonly bool preserveGroupBraces;
		int position;

		public LatexMiniParser(string data, bool preserveGroupBraces = false)
		{
			this.data = data;
			this.preserveGroupBraces = preserveGroupBraces;
		}

		public string Parse()
		{
			var sb = new StringBuilder(data.Length);

nextChar:
			while(position < data.Length)
			{
				var currentChar = data[position];

				switch (currentChar)
				{
					case '{':

						if (preserveGroupBraces) break;
						
						position++;
						goto nextChar;

					case '}':

						if (preserveGroupBraces) break;
						
						position++;
						goto nextChar;

					case '~':
						currentChar = '\xa0'; // Non-breaking space
						break;

					case '-':
						if (LookAhead(1) == '-')
						{
							if (LookAhead(2) == '-')
							{
								currentChar = '—';
								position += 2;
							}
							else
							{
								currentChar = '–';
								position++;
							}
						}
						break;

					case '`':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '“';
							position++;
						}
						else
						{
							currentChar = '‘';
						}
						break;

					case '\'':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '”';
							position++;
						}
						else
						{
							currentChar = '’';
						}
						break;

					case '?':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¿';
						}
						break;

					case '!':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¡';
						}
						break;

					case '$':
						position++;

						if (LookAhead() != '$') goto nextChar;
						break;

					case '\r': case '\n': case '\t': case ' ':
						sb.Append(' ');

						char n;
						do
						{
							position++;

							n = LookAhead();
						} while (n == ' ' || n == '\r' || n == '\n' || n == '\t');
						goto nextChar;

					case '\\':
						var la = LookAhead(1);

						position++;

						if (la == '\\') break;

						if (" &#$%_,{}".IndexOf(la) != -1)
						{
							currentChar = la;
							break;
						}

						var accentIndex = AccentPrefixes.IndexOf(la);
						if (accentIndex != -1)
						{
							la = LookAhead(1);

							if (la == '{')
							{
								la = LookAhead(2);
							}

							var accentedChar = SymbolHelper.GetAccentedChar(AccentPrefixes[accentIndex], la);
							if (accentedChar != '\0')
							{
								sb.Append(accentedChar);
								position += LookAhead(1) == '{' ? 4 : 2;
								goto nextChar;
							}
						}

						var wordEnd = position;

						while(wordEnd + 1 < data.Length)
						{
							var c = char.ToLowerInvariant(data[wordEnd + 1]);
							if (c < 'a' || c > 'z') break;

							wordEnd++;
						}
						if (wordEnd == data.Length) break;

						var word = data.Substring(position, wordEnd - position + 1);

						if (word == VerbCommand)
						{
							position += VerbCommand.Length;
							var delimiterChar = LookAhead();
							position++;

							var count = 0;

							while(count < data.Length && LookAhead(count) != delimiterChar) count++;
							sb.Append(data, position, count);
							position += count;
							position++;
							goto nextChar;
						}

						if (word == UnicharCommand)
						{
							position += UnicharCommand.Length;
							position++; // {

							var number = string.Empty;
							var isHex = false;

							currentChar = LookAhead();

							while (currentChar != '}' && currentChar != EOF)
							{
								if (char.IsDigit(currentChar))
								{
									number += currentChar;
								}
								else if ((isHex || number.Length > 0) && "ABCDEFabcdef".IndexOf(currentChar) != -1)
								{
									number += currentChar;
									isHex = true;
								}

								position++;

								currentChar = LookAhead();
							}

							position++;

							if (currentChar == EOF) goto nextChar;
							
							int unicode;
							if (int.TryParse(number, isHex ? NumberStyles.HexNumber : NumberStyles.Integer, null, out unicode) && unicode < char.MaxValue)
							{
								sb.Append((char) unicode);
							}
							else
							{
								sb.Append("?");
							}

							goto nextChar;
						}

						var replacement = SymbolHelper.GetReplacementFor(word);
						if (replacement != null)
						{
							sb.Append(replacement);
position += word.Length;
goto nextChar;
						}

//						position += word.Length;
						if (accentIndex != -1) position += word.Length;
						goto nextChar;

				}

				sb.Append(currentChar);
				position++;
			}

			return sb.ToString();
		}

		char LookAhead(int distance = 0)
		{
			var aheadPosition = position + distance;

			if (aheadPosition >= data.Length) return EOF;

			return data[aheadPosition];
		}
	}
*/
/*
	public class LatexMiniParser
	{
		const char EOF = (char) 0;
		const string UnicharCommand = "unichar";
		const string VerbCommand = "verb";
		const string AccentPrefixes = @"`'^""~cvu=.Hbd";

		readonly string data;
		readonly bool preserveGroupBraces;
		int position;

		public LatexMiniParser(string data, bool preserveGroupBraces = false)
		{
			this.data = data;
			this.preserveGroupBraces = preserveGroupBraces;
		}

		public string Parse()
		{
			var sb = new StringBuilder(data.Length);

nextChar:
			while(position < data.Length)
			{
				var currentChar = data[position];

				switch (currentChar)
				{
					case '}':

						if (!preserveGroupBraces)
						{
							position++;
							goto nextChar;
						}

						break;

					case '{':

						if (!preserveGroupBraces)
						{
							position++;
							goto nextChar;
						}

						break;

                    case '~':
						currentChar = '\xa0'; // Non-breaking space
						break;

					case '-':
						if (LookAhead(1) == '-')
						{
							if (LookAhead(2) == '-')
							{
								currentChar = '—';
								position += 2;
							}
							else
							{
								currentChar = '–';
								position++;
							}
						}
						break;

					case '`':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '“';
							position++;
						}
						else
						{
							currentChar = '‘';
						}
						break;

					case '\'':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '”';
							position++;
						}
						else
						{
							currentChar = '’';
						}
						break;

					case '?':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¿';
						}
						break;

					case '!':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¡';
						}
						break;

					case '$':
						position++;

						if (LookAhead() != '$') goto nextChar;
						break;

					case '\r': case '\n': case '\t': case ' ':
						sb.Append(' ');

						char n;
						do
						{
							position++;

							n = LookAhead();
						} while (n == ' ' || n == '\r' || n == '\n' || n == '\t');
						goto nextChar;

					case '\\':
						var la = LookAhead(1);

						position++;

						if (la == '\\') break;

						if (" &#$%_,{}".IndexOf(la) != -1)
						{
							currentChar = la;
							break;
						}

						var accentIndex = AccentPrefixes.IndexOf(la);
						if (accentIndex != -1)
						{
							la = LookAhead(1);

							if (la == '{')
							{
								la = LookAhead(2);
							}

							var accentedChar = SymbolHelper.GetAccentedChar(AccentPrefixes[accentIndex], la);
							if (accentedChar != '\0')
							{
								sb.Append(accentedChar);
								position += LookAhead(1) == '{' ? 4 : 2;
								goto nextChar;
							}
						}

						var wordEnd = position;

						while(wordEnd + 1 < data.Length)
						{
							var c = char.ToLowerInvariant(data[wordEnd + 1]);
							if (c < 'a' || c > 'z') break;

							wordEnd++;
						}
						if (wordEnd == data.Length) break;

						var word = data.Substring(position, wordEnd - position + 1);

						if (word == VerbCommand)
						{
							position += VerbCommand.Length;
							var delimiterChar = LookAhead();
							position++;

							var count = 0;

							while(count < data.Length && LookAhead(count) != delimiterChar) count++;
							sb.Append(data, position, count);
							position += count;
							position++;
							goto nextChar;
						}

						if (word == UnicharCommand)
						{
							position += UnicharCommand.Length;
							position++; // {

							var number = string.Empty;
							var isHex = false;

							currentChar = LookAhead();

							while (currentChar != '}' && currentChar != EOF)
							{
								if (char.IsDigit(currentChar))
								{
									number += currentChar;
								}
								else if ((isHex || number.Length > 0) && "ABCDEFabcdef".IndexOf(currentChar) != -1)
								{
									number += currentChar;
									isHex = true;
								}

								position++;

								currentChar = LookAhead();
							}

							position++;

							if (currentChar == EOF) goto nextChar;
							
							int unicode;
							if (int.TryParse(number, isHex ? NumberStyles.HexNumber : NumberStyles.Integer, null, out unicode) && unicode < char.MaxValue)
							{
								sb.Append((char) unicode);
							}
							else
							{
								sb.Append("?");
							}

							goto nextChar;
						}

						var replacement = SymbolHelper.GetReplacementFor(word);
						if (replacement != null)
						{
							sb.Append(replacement);
position += word.Length;
goto nextChar;
						}

//						position += word.Length;
						if (accentIndex != -1) position += word.Length;
						goto nextChar;

				}

				sb.Append(currentChar);
				position++;
			}

			return sb.ToString();
		}

		char LookAhead(int distance = 0)
		{
			var aheadPosition = position + distance;

			if (aheadPosition >= data.Length) return EOF;

			return data[aheadPosition];
		}
	}
*/
/*
	public class LatexMiniParser
	{
		const char EOF = (char) 0;
		const string UnicharCommand = "unichar";
		const string VerbCommand = "verb";
		const string AccentPrefixes = @"`'^""~cvu=.Hbd";

		readonly string data;
		readonly bool preserveGroupBraces;
		int position;

		public LatexMiniParser(string data, bool preserveGroupBraces)
		{
			this.data = data;
			this.preserveGroupBraces = preserveGroupBraces;
		}

		public string Parse()
		{
			var sb = new StringBuilder(data.Length);

nextChar:
			while(position < data.Length)
			{
				var currentChar = data[position];

				switch (currentChar)
				{
					case '}':

						if (!preserveGroupBraces)
						{
							position++;
							goto nextChar;
						}

						break;

					case '{':

						if (!preserveGroupBraces)
						{
							position++;
							goto nextChar;
						}

						break;

                    case '~':
						currentChar = '\xa0'; // Non-breaking space
						break;

					case '-':
						if (LookAhead(1) == '-')
						{
							if (LookAhead(2) == '-')
							{
								currentChar = '—';
								position += 2;
							}
							else
							{
								currentChar = '–';
								position++;
							}
						}
						break;

					case '`':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '“';
							position++;
						}
						else
						{
							currentChar = '‘';
						}
						break;

					case '\'':
						if (LookAhead(1) == currentChar)
						{
							currentChar = '”';
							position++;
						}
						else
						{
							currentChar = '’';
						}
						break;

					case '?':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¿';
						}
						break;

					case '!':
						if (LookAhead(1) == '`')
						{
							position++;
							currentChar = '¡';
						}
						break;

					case '$':
						position++;

						if (LookAhead() != '$') goto nextChar;
						break;

					case '\r': case '\n': case '\t': case ' ':
						sb.Append(' ');

						char n;
						do
						{
							position++;

							n = LookAhead();
						} while (n == ' ' || n == '\r' || n == '\n' || n == '\t');
						goto nextChar;

					case '\\':
						var la = LookAhead(1);

						position++;

						if (la == '\\') break;

						if (" &#$%_,{}".IndexOf(la) != -1)
						{
							currentChar = la;
							break;
						}

						var accentIndex = AccentPrefixes.IndexOf(la);
						if (accentIndex != -1)
						{
							la = LookAhead(1);

							if (la == '{')
							{
								la = LookAhead(2);
							}

							var accentedChar = SymbolHelper.GetAccentedChar(AccentPrefixes[accentIndex], la);
							if (accentedChar != '\0')
							{
								sb.Append(accentedChar);
								position += LookAhead(1) == '{' ? 4 : 2;
								goto nextChar;
							}
						}

						var wordEnd = position;

						while(wordEnd + 1 < data.Length)
						{
							var c = char.ToLowerInvariant(data[wordEnd + 1]);
							if (c < 'a' || c > 'z') break;

							wordEnd++;
						}
						if (wordEnd == data.Length) break;

						var word = data.Substring(position, wordEnd - position + 1);

						if (word == VerbCommand)
						{
							position += VerbCommand.Length;
							var delimiterChar = LookAhead();
							position++;

							var count = 0;

							while(count < data.Length && LookAhead(count) != delimiterChar) count++;
							sb.Append(data, position, count);
							position += count;
							position++;
							goto nextChar;
						}

						if (word == UnicharCommand)
						{
							position += UnicharCommand.Length;
							position++; // {

							var number = string.Empty;
							var isHex = false;

							currentChar = LookAhead();

							while (currentChar != '}' && currentChar != EOF)
							{
								if (char.IsDigit(currentChar))
								{
									number += currentChar;
								}
								else if ((isHex || number.Length > 0) && "ABCDEFabcdef".IndexOf(currentChar) != -1)
								{
									number += currentChar;
									isHex = true;
								}

								position++;

								currentChar = LookAhead();
							}

							position++;

							if (currentChar == EOF) goto nextChar;
							
							int unicode;
							if (int.TryParse(number, isHex ? NumberStyles.HexNumber : NumberStyles.Integer, null, out unicode) && unicode < char.MaxValue)
							{
								sb.Append((char) unicode);
							}
							else
							{
								sb.Append("?");
							}

							goto nextChar;
						}

						var replacement = SymbolHelper.GetReplacementFor(word);
						if (replacement != null)
						{
							sb.Append(replacement);
position += word.Length;
goto nextChar;
						}

//						position += word.Length;
						if (accentIndex != -1) position += word.Length;
						goto nextChar;

				}

				sb.Append(currentChar);
				position++;
			}

			return sb.ToString();
		}

		char LookAhead(int distance = 0)
		{
			var aheadPosition = position + distance;

			if (aheadPosition >= data.Length) return EOF;

			return data[aheadPosition];
		}
	}
*/
}