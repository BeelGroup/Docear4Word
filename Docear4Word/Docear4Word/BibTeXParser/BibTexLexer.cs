using System;
using System.Collections.Generic;

namespace Docear4Word.BibTex
{
	public class BibTexLexer
	{
		const char EOF = (char) 0;
		int column;

		LexMode currentMode;
		readonly string data;

		int line;
		Stack<LexMode> modes;
		int position; // position within data

		int startColumn;
		int startLine;
		int startPosition;

		public BibTexLexer(string data)
		{
			if (data == null) throw new ArgumentNullException("data");

			this.data = data;

			Reset();
		}

		void Reset()
		{
			modes = new Stack<LexMode>();
			EnterMode(LexMode.Root);

			line = 1;
			column = 1;
			position = 0;
		}

		void EnterMode(LexMode mode)
		{
			modes.Push(currentMode);
			currentMode = mode;
		}

		void LeaveMode()
		{
			currentMode = modes.Pop();
		}

		char LookAhead()
		{
			if (position >= data.Length) return EOF;

			return data[position];
		}

/*
		char LookAhead(int distance)
		{
			var aheadPosition = position + distance;

			if (aheadPosition >= data.Length) return EOF;

			return data[aheadPosition];
		}
*/

		void Consume()
		{
			position++;
			column++;
		}

/*
		void Consume(int count)
		{
			position += count;
			column += count;
		}
*/

		void NewLine()
		{
			line++;
			column = 1;
		}

/*
		Token CreateToken(TokenType kind, string data)
		{
			return new Token(kind, data, startLine, startColumn, startPosition);
		}
*/

		Token CreateToken(TokenType kind)
		{
			return new Token(kind, data.Substring(startPosition, position - startPosition), startLine, startColumn, startPosition);
		}

		void ConsumeLine(bool includeLineEnd)
		{
			while(true)
			{
				var ch = LookAhead();

				switch(ch)
				{
					case EOF:
						return;

					case '\n':
						if (includeLineEnd)
						{
							Consume();
							NewLine();
						}

						return;

					case '\r':
						if (includeLineEnd)
						{
							Consume();
							if (LookAhead() == '\n') Consume();
							NewLine();
						}

						return;
				}

				Consume();
			}
		}

		void ConsumeWhitespace()
		{
			while (true)
			{
				var ch = LookAhead();

				switch (ch)
				{
					case ' ':
					case '\t':
						Consume();
						break;

					case '\n':
						Consume();
						NewLine();
						break;

					case '\r':
						Consume();
						if (LookAhead() == '\n') Consume();
						NewLine();
						break;

					default:
						return;
				}
			}
		}

		private void StartRead()
		{
			startLine = line;
			startColumn = column;
			startPosition = position;
		}

		public Token Next()
		{
			switch (currentMode)
			{
				case LexMode.Root:
					return NextRootItem();

				case LexMode.Entry:
					return NextEntry();

				default: throw new TemplateParseException("Encountered invalid lexer mode: " + currentMode.ToString(), line, column);
			}
		}

		int braceLevel;

		Token NextEntry()
		{
			ConsumeWhitespace();

			StartRead();

			var ch = LookAhead();

			switch (ch)
			{
				case EOF:
					return CreateToken(TokenType.EOF);

				case '{':
					braceLevel++;
					Consume();

					if (braceLevel == 1) return CreateToken(TokenType.OpeningBrace);

					var result = ReadBracedString();
					Consume();
					return result;

				case '}':
					braceLevel--;
					Consume();
					if (braceLevel == 0) LeaveMode();
					return CreateToken(TokenType.ClosingBrace);

				case ',':
					Consume();
					return CreateToken(TokenType.Comma);

				case '=':
					Consume();
					return CreateToken(TokenType.Equals);

				case '#':
					Consume();
					return CreateToken(TokenType.Hash);

				case '"':
					return ReadQuotedString();

				default:
					if (IsValidNameChar(ch))
					{
						return ReadText();
					}

					Consume();
					return CreateToken(TokenType.Text);
			}
		}

/*
		bool ReplaceWithAccent(char accentType, char toAccent, StringBuilder sb)
		{
			var accentedChar = SymbolHelper.GetAccentedChar(accentType, toAccent);
			if (accentedChar == '\0') return false;

			sb.Append(accentedChar);

			return true;
		}
*/

		Token ReadBracedString()
		{
			StartRead();

			var localBraceLevel = 0;

			while(position < data.Length)
			{
				switch (data[position])
				{
					case '}':
						if (localBraceLevel == 0)
						{
							braceLevel--;
							return CreateToken(TokenType.BracedString);
						}

						localBraceLevel--;
						break;

					case '{':
						localBraceLevel++;
						break;


					case '\n':
						line++; column = 1;

						break;

					case '\r':
						if (position + 1 >= data.Length) break;

						if (data[position + 1] != '\n')
						{
							line++; column = 1;
						}
						break;
				}

				position++;
			}

			return CreateToken(TokenType.EOF);
		}

		Token ReadQuotedString()
		{
			Consume();

			StartRead();

			while(true)
			{
				var ch = LookAhead();

				switch (ch)
				{
					case EOF:
						return CreateToken(TokenType.EOF);

					case '\r':
					case '\n':
						ConsumeWhitespace();
						break;

					case '"':
						var result = CreateToken(TokenType.QuotedString);
						Consume();
						return result;

					default:
						Consume();
						break;

				}
			}
		}

		Token NextRootItem()
		{
			ReadAgain:
			ConsumeWhitespace();

			StartRead();

			var ch = LookAhead();

			switch (ch)
			{
				case EOF:
					return CreateToken(TokenType.EOF);

				case '@':
					EnterMode(LexMode.Entry);
					Consume();
					return CreateToken(TokenType.At);

				case '%':
					ConsumeLine(true);
					goto ReadAgain;
                /*
                case '{':
                    braceLevel++;
                    Consume();

                    if (braceLevel == 1) return CreateToken(TokenType.OpeningBrace);

                    var result = ReadBracedString();
                    Consume();
                    return result;

                case '}':
                    braceLevel--;
                    Consume();
                    if (braceLevel == 0) LeaveMode();
                    return CreateToken(TokenType.ClosingBrace);
                
                case '{':
                    Consume();
                    return CreateToken(TokenType.OpeningBrace);

                case '}':
                    Consume();
                    return CreateToken(TokenType.ClosingBrace);
                */
				default:
					if (Char.IsLetter(ch) || ch == '_')
					{
						return ReadText();
					}
// Ignore anything not recognized. 
                    // [Allen] However, this measure cannot detect invalidate characters. 
                    // We have to add '{' or '}' case to detect those.
					Consume();
					goto ReadAgain;
			}
		}

		static bool IsValidNameChar(char c)
		{
			if (char.IsLetter(c)) return true;
			if (char.IsDigit(c)) return true;

			if (char.IsWhiteSpace(c)) return false;

			switch (c)
			{
				// Xml-reserved (apostrophe is allowed here in the Lexer but stripped out later in the BibTexDatabase.Item)
				case '>': case '<': case '&': case '%': case '{': case '}': 
				
				// Other
				case ',': case '\"': case '=':
					return false;

				default:
					return true;
			}
		}

		Token ReadText()
		{
			StartRead();

			Consume();

			while(position < data.Length)
			{
				if (!IsValidNameChar(data[position])) return CreateToken(TokenType.Text);

				Consume();
			}

			return CreateToken(TokenType.EOF);
		}

		#region Nested type: LexMode
		enum LexMode
		{
			Root,
			Entry,
		}
		#endregion
	}

	public class Token
	{
		readonly int line;
		readonly int column;
		readonly string data;
		readonly TokenType tokenType;
		readonly int position;

		internal Token(TokenType tokenType, string data, int line, int column, int position)
		{
			this.tokenType = tokenType;
			this.line = line;
			this.column = column;
			this.data = data;
			this.position = position;
		}

		public int Column
		{
			get { return column; }
		}

		public string Data
		{
			get { return data; }
		}

		public int Line
		{
			get { return line; }
		}

		public int Position
		{
			get { return position; }
		}

		internal TokenType TokenType
		{
			get { return tokenType; }
		}
	}

	internal enum TokenType
	{
		EOF,
		At,
		OpeningBrace,
		ClosingBrace,
		Text,
		Comma,
		Equals,
		QuotedString,
		BracedString,
		Hash,
	}
}