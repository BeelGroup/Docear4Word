using System;

namespace Docear4Word.BibTex
{
	public class BibTexParser
	{
		readonly BibTexLexer lexer;
		Token currentToken;
		int unknownCounter;

		public BibTexParser(BibTexLexer lexer)
		{
			this.lexer = lexer;
		}

		Token Consume()
		{
			var oldToken = currentToken;
			currentToken = lexer.Next();
			return oldToken;
		}

		Token Consume(TokenType tokenType)
		{
			var old = currentToken;
			currentToken = lexer.Next();

			if (old.TokenType != tokenType)
			{
				// Special case for missing bibtex key - we'll just create one. (Also deals with no tags)
				if (tokenType == TokenType.Text && old.TokenType == TokenType.Comma && (currentToken.TokenType == TokenType.Text || currentToken.TokenType == TokenType.ClosingBrace))
				{
					old = new Token(TokenType.Text, "_Unknown_" + (++unknownCounter), old.Line, old.Column, old.Position);
					return old;
				}

				throw new TemplateParseException("Unexpected token: " + old.TokenType + ". Was expecting: " + tokenType, currentToken.Line, currentToken.Column);
			}

			return old;
		}

		internal Token Current
		{
			get { return currentToken; }
		}

		public BibTexDatabase Parse()
		{
			var database = new BibTexDatabase();

			Consume();

			while (true)
			{
				switch (Current.TokenType)
				{
					case TokenType.EOF:
						return database;

					case TokenType.At:
						ParseRootEntry(database);
						break;

					default:
						Consume();
						break;
				}
			}
		}

		void ParseRootEntry(BibTexDatabase database)
		{
			Consume(TokenType.At);

			var entryType = Consume(TokenType.Text).Data;

			Consume(TokenType.OpeningBrace);

			switch (entryType.ToLower())
			{
				case "string":
					ParseAbbreviation(database);
					break;

				case "preamble":
					ParsePreamble();
					break;

				case "comment":
					ParseComment();
					break;

				default:
					var entry = ParseEntry(database, entryType);
					database.AddEntry(entry);
					//Console.WriteLine("@{0}{{{1},", entry.EntryType, entry.Name);
					break;

			}
		}

		void ParsePreamble()
		{
			while (Current.TokenType != TokenType.ClosingBrace)
			{
				Consume();
			}

			Consume(TokenType.ClosingBrace);
		}

		void ParseComment()
		{
			while (Current.TokenType != TokenType.ClosingBrace)
			{
				Consume();
			}

			Consume(TokenType.ClosingBrace);
		}

		void ParseAbbreviation(BibTexDatabase database)
		{
			var entryName = Consume(TokenType.Text).Data;

			Consume(TokenType.Equals);

			var entryValue = Consume().Data;

			// Consume any trailing comma
			if (Current.TokenType == TokenType.Comma) Consume();
            
			Consume(TokenType.ClosingBrace);

			//Console.WriteLine("@string{{{0} = \"{1}\"}}", entryName, entryValue);
			database.AddAbbreviation(entryName, entryValue);
		}

		Entry ParseEntry(BibTexDatabase database, string entryType)
		{
			var entryName = Consume(TokenType.Text).Data;
			var entry = new Entry(entryType, entryName, Helper.GetClassificationForType(entryType));

			while(true)
			{
				var token = Consume();

				switch (token.TokenType)
				{
					case TokenType.EOF:
						return entry;

					case TokenType.ClosingBrace:
						return entry;

					case TokenType.Equals:
						break;

					case TokenType.Comma:
						break;

					case TokenType.Text:
						ParseTag(database, entry, token.Data);
						break;

					case TokenType.OpeningBrace:
						break;

					default:
						throw new TemplateParseException("Unexpected token: " + token.TokenType, currentToken.Line, currentToken.Column);
				}
			}
		}

		void ParseTag(BibTexDatabase database, Entry entry, string tagName)
		{
			Consume(TokenType.Equals);

			string value = null;
            Token LastToken;
			while(true)
			{
                LastToken = Current;

				switch (Current.TokenType)
				{
					case TokenType.QuotedString:
						value += Consume().Data;
						break;

					case TokenType.BracedString:
						value += Consume().Data;
						break;

					case TokenType.Text:
						var token = Consume().Data;
						value += database.GetAbbreviation(token, token);

						break;

					case TokenType.Hash:
						break;

					default:
						throw new NotImplementedException();
				}
                // [Allen] Check the invalid token like:
                // *************************
                // owner = {Norman}   # LastToken.TokenType == TokenType.BracedString
                // timestamp = {2012-07-19} # Current.TokenType == TokenType.Text
                // *************************
                if (LastToken.TokenType == TokenType.BracedString &&
                    Current.TokenType == TokenType.Text )
                {
                    throw new TemplateParseException("Unexpected token: " + LastToken.TokenType + ". Was expecting: '" + LastToken.Data + ",'", LastToken.Line, LastToken.Column);
                }

				if (Current.TokenType != TokenType.Hash) break;

				Consume();
			} 

			entry.AddTag(new TagEntry(tagName, value));
		}
	}

	public class TemplateParseException: Exception
	{
		readonly int line;
		readonly int column;
		
		public TemplateParseException(string message, int line, int column): base(message)
		{
			this.line = line;
			this.column = column;
		}

		public int Column
		{
			get { return column; }
		}

		public int Line
		{
			get { return line; }
		}

		public override string ToString()
		{
			return string.Format("{0} at line {1}, column {2}", Message, Line, Column);
		}
	}
}