using System;
using System.Text;

namespace Docear4Word
{
	public static class JSONHelper
	{
const char SingleQuote = '\'';
		const char Quote = '\"';
		const char Backslash = '\\';
		const char Slash = '/';
		const char Backspace = '\b';
		const char FormFeed = '\f';
		const char NewLine = '\n';
		const char CarriageReturn = '\r';
		const char Tab = '\t';
		
		static readonly char[] EscapableChars = new[] { SingleQuote, Quote, Tab, Backslash, CarriageReturn, NewLine, /*Slash, */FormFeed, Backspace};

		public static string Escape(string text)
		{
			if (text == null) throw new ArgumentNullException("text");

			var index = text.IndexOfAny(EscapableChars);
			if (index == -1) return text;

			var sb = new StringBuilder(text, 0, index, text.Length * 2);

			while(true)
			{
				sb.Append('\\');

				var replacementChar = text[index];

				switch (replacementChar)
				{
					case SingleQuote:
					case Quote:
					case Backslash:
					case Slash:
						break;

					case Backspace:
						replacementChar = 'b';
						break;

					case FormFeed:
						replacementChar = 'f';
						break;

					case NewLine:
						replacementChar = 'n';
						break;

					case CarriageReturn:
						replacementChar = 'r';
						break;

					case Tab:
						replacementChar = 't';
						break;

					default:
						throw new InvalidOperationException();
				}

				sb.Append(replacementChar);

				if (++index == text.Length) break;

				var lastIndex = index;

				index = text.IndexOfAny(EscapableChars, index);

				sb.Append(text, lastIndex, (index == -1 ? text.Length : index) - lastIndex);

				if (index == -1) 
				{
					//sb.Append(text, lastIndex, text.Length - lastIndex);
					break;
				}

			}

/*
			sb.Append('\\');
			sb.Append(text[index]);

			while(++index < text.Length)
			{
				var lastIndex = index;

				index = text.IndexOfAny(EscapableChars, index);
				if (index == -1)
				{
					sb.Append(text, lastIndex, text.Length - lastIndex);
					break;
				}

				sb.Append('\\');
				sb.Append(text[index]);

			}
*/

			return sb.ToString();
		}


	}
}