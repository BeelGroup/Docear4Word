using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Docear4Word
{
	[ComVisible(false)]
	public class NameSplitter
	{
		static readonly Regex ListSplitter = new Regex(@"(?<=^|\band\b\s+)(.+?)(?=$|\s+\band\b)", RegexOptions.IgnoreCase);

		public CSLNameComponents[] Split(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			var result = new List<CSLNameComponents>();

			name = name.Trim();

			if (name.Length != 0)
			{
				var splitterLogic = new LatexLogicSplitter();

				foreach(Match match in ListSplitter.Matches(name))
				{
					var tokens = SplitToWordTokens(match.Value);

					var nameComponents = splitterLogic.Split(tokens);
					if (nameComponents == null) continue;

					result.Add(nameComponents);
				}
			}

			return result.ToArray();
		}

		class LatexLogicSplitter
		{
			public CSLNameComponents Split(List<WordToken> tokens)
			{
				var commaIndexes = GetCommaIndexes(tokens);
				if (commaIndexes.Length == tokens.Count) return null;

				return commaIndexes.Length == 0
				       	? SplitToWordsNoCommas(tokens)
				       	: SplitToWordsCommas(tokens, commaIndexes);
			}

			static CSLNameComponents SplitToWordsCommas(List<WordToken> tokens, int[] commaIndexes)
			{
				var result = new CSLNameComponents();
				var familyNameTokens = new List<WordToken>();
				var givenNameTokens = new List<WordToken>();
				var droppingParticleNameTokens = new List<WordToken>();

				// Find the last comma
				var commaIndex = commaIndexes[commaIndexes.Length - 1];

				// Everything after the (last) comma goes to given name
				tokens.RemoveAt(commaIndex);
				givenNameTokens.AddRange(ExtractTokenRange(tokens, commaIndex));

				// Check if multiple commas are present
				if (commaIndex != commaIndexes[0])
				{
					// Remove the first comma
					commaIndex = commaIndexes[0];
					tokens.RemoveAt(commaIndex);

					// Extract following tokens (removing any extra commas)
					var tokensBetweenCommas = ExtractTokenRange(tokens, commaIndex);
					tokensBetweenCommas.RemoveAll(token => token.IsComma);

					// Add to the dropping-particle
					droppingParticleNameTokens.AddRange(tokensBetweenCommas);
				}

				if (tokens.Count > 0)
				{
					// The last word before the (first) comma goes to family name
					familyNameTokens.Add(ExtractToken(tokens, tokens.Count - 1));

					int firstLowerCaseIndex;
					int lowerCaseSequenceCount;
					FindLowerCaseTokenRange(tokens, out firstLowerCaseIndex, out lowerCaseSequenceCount);

					// Did we find any?
					if (lowerCaseSequenceCount > 0)
					{
						// Yes, so extract everything from the start (not the first lower-case token!) 
						// to the last lower-case as dropping particles
						droppingParticleNameTokens.InsertRange(0, ExtractTokenRange(tokens, 0, firstLowerCaseIndex + lowerCaseSequenceCount));
					}

					// Anything else goes into family
					if (tokens.Count > 0)
					{
						familyNameTokens.InsertRange(0, tokens);
					}
				}

				if (familyNameTokens.Count > 0) result.Family = JoinTokens(familyNameTokens);
				if (givenNameTokens.Count > 0) result.Given = JoinTokens(givenNameTokens);
				if (droppingParticleNameTokens.Count > 0) result.DroppingParticle = JoinTokens(droppingParticleNameTokens);

				return result;
			}

			static CSLNameComponents SplitToWordsNoCommas(List<WordToken> tokens)
			{
				var result = new CSLNameComponents();
				var familyNameTokens = new List<WordToken>();
				var givenNameTokens = new List<WordToken>();
				var droppingParticleNameTokens = new List<WordToken>();

				// Always at least a family name
				familyNameTokens.Add(ExtractToken(tokens, tokens.Count - 1));

				// Do we have any remaining tokens?
				if (tokens.Count != 0)
				{
					// Yes, so find the range of the tokens which are lower-cased
					int firstLowerCaseIndex;
					int lowerCaseSequenceCount;
					FindLowerCaseTokenRange(tokens, out firstLowerCaseIndex, out lowerCaseSequenceCount);

					// Did we find any?
					if (lowerCaseSequenceCount > 0)
					{
						// Yes, so extract them as dropping particles
						droppingParticleNameTokens.AddRange(ExtractTokenRange(tokens, firstLowerCaseIndex, lowerCaseSequenceCount));

						// Were there any word tokens after this section?
						if (tokens.Count - firstLowerCaseIndex > 0)
						{
							// Yes, so they are inserted before the family name
							familyNameTokens.InsertRange(0, ExtractTokenRange(tokens, firstLowerCaseIndex, tokens.Count - firstLowerCaseIndex));
						}
					}

					// Given is anything left
					result.Given = JoinTokens(tokens);
				}

				if (familyNameTokens.Count > 0) result.Family = JoinTokens(familyNameTokens);
				if (givenNameTokens.Count > 0) result.Given = JoinTokens(givenNameTokens);
				if (droppingParticleNameTokens.Count > 0) result.DroppingParticle = JoinTokens(droppingParticleNameTokens);

				return result;
			}
		}

		static List<WordToken> SplitToWordTokens(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			var result = new List<WordToken>();

			var braceDepth = 0;
			var position = 0;
			var index = 0;
			var isProtected = false;
			var sb = new StringBuilder(name.Length);

			while(position < name.Length)
			{
				var currentChar = name[position];

				if (braceDepth == 0 && (char.IsWhiteSpace(currentChar) || currentChar == ','))
				{
					if (sb.Length != 0)
					{
						result.Add(new WordToken(sb.ToString(), index++));
						sb.Length = 0;
					}

					if (currentChar == ',')
					{
						result.Add(new WordToken(",", index++));
					}

					while(++position < name.Length && char.IsWhiteSpace(name[position])) {}

					continue;
				}
				
				switch (currentChar)
				{
					case '{':
						braceDepth++;

						isProtected = true;
						break;

					case '}':
						braceDepth--;

						if (braceDepth == 0 &&
						    sb.Length != 0 &&
						    (position + 1) < name.Length &&
						    (char.IsWhiteSpace(name[position + 1]) || name[position + 1] == ','))
						{
							result.Add(new WordToken(sb.ToString(), index++, true));
							sb.Length = 0;
							isProtected = false;
						}
						break;

					default:
						sb.Append(currentChar);
						break;
				}

				position++;
			}

			if (sb.Length != 0)
			{
				result.Add(new WordToken(sb.ToString(), index++, isProtected));
			}

			return result;
		}

		class WordToken
		{
			readonly string text;
			readonly FirstLetter firstLetter;
			readonly int index;
			readonly bool isProtected;

			public WordToken(string text, int index, bool isProtected = false)
			{
				this.text = text;
				this.index = index;
				this.isProtected = isProtected;

				firstLetter = isProtected
					            ? FirstLetter.None
					            : char.IsUpper(text, 0)
					              	? FirstLetter.Upper
					              	: char.IsLower(text, 0)
					              	  	? FirstLetter.Lower
					              	  	: FirstLetter.None;
			}

//public bool IsJoinable { get; set; }
			public string Text
			{
				get { return text; }
			}

			public FirstLetter FirstLetter
			{
				get { return firstLetter; }
			}

			public int Index
			{
				get { return index; }
			}

			public bool IsProtected
			{
				get { return isProtected; }
			}

			public bool IsComma
			{
				get { return text.Length == 1 && text[0] == ','; }
			}

			public override string ToString()
			{
				return text;
			}
		}

		static int[] GetCommaIndexes(List<WordToken> tokens)
		{
			var result = new List<int>();

			for(var i = 0; i < tokens.Count; i++)
			{
				if (tokens[i].IsComma) result.Add(i);
			}

			return result.ToArray();
		}

		static List<WordToken> ExtractTokenRange(List<WordToken> list, int startIndex, int count = -1)
		{
			if (count == -1)
			{
				count = list.Count - startIndex;
			}

			var result = list.GetRange(startIndex, count);
			list.RemoveRange(startIndex, count);

			return result;
		}

		static WordToken ExtractToken(List<WordToken> tokens, int index)
		{
			var result = tokens[index];
			tokens.RemoveAt(index);

			return result;
		}

		static string JoinTokens(IEnumerable<WordToken> tokens, string separator = " ")
		{
			var sb = new StringBuilder();

			foreach(var token in tokens)
			{
				if (sb.Length != 0) sb.Append(separator);

				sb.Append(token.ToString());
			}

			return sb.ToString();
		}

		static void FindLowerCaseTokenRange(List<WordToken> tokens, out int startIndex, out int count)
		{
			startIndex = -1;
			var endIndex = -1;

			for(var i = 0; i < tokens.Count; i++)
			{
				if (tokens[i].FirstLetter == FirstLetter.Lower)
				{
					if (startIndex == -1) startIndex = i;
					endIndex = i;
				}
			}

			count = startIndex == -1
				        ? 0
				        : endIndex - startIndex + 1;
		}

		enum FirstLetter
		{
			Upper,
			Lower,
			None,
		}

	}

	[ComVisible(false)]
	public class CSLNameComponents
	{
		public string Family;
		public string Given;
		public string DroppingParticle;
		public string NonDroppingParticle;
		public string Suffix;
		public bool? CommaSuffix;
		public bool? StaticOrdering;

/*
		object CommaDroppingParticle;
		object Literal;
		object IsInsitution;
		object ParseNames;
*/


	}
}