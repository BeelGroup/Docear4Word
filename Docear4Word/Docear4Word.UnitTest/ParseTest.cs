using System;
using System.IO;
using System.Text;

using Docear4Word.BibTex;

using NUnit.Framework;

namespace docear4word.UnitTest
{
	[TestFixture]
	public class ParseTest
	{
		static BibTexDatabase specialCharactersDatabase;

		[TestFixtureSetUp]
		public void LoadSpecialCharactersDatabase()
		{
			var data = File.ReadAllText(@"Sample Files\b4w.bib", Encoding.GetEncoding(1252));
			var parser = new BibTexParser(new BibTexLexer(data));

			specialCharactersDatabase = parser.Parse();
		}

		[Test]
		public void SimpleParser()
		{
			const string Data = @"

@article{mrx05, 
auTHor = ""Mr. X"", 
Title = {Something Great}, 
publisher = ""nob"" # ""ody"", 
YEAR = 2005, 
} ";

			var lexer = new BibTexLexer(Data);
			var parser = new BibTexParser(lexer);

			var db = parser.Parse();
			Console.WriteLine("{0} abbreviations; {1} entries", db.AbbreviationCount, db.EntryCount);
		}

		[Test]
		public void BigFileParser()
		{
			var data = File.ReadAllText(@"Sample Files\BigFile.bib");

			var parser = new BibTexParser(new BibTexLexer(data));

			var db = parser.Parse();
			Console.WriteLine("{0} abbreviations; {1} entries", db.AbbreviationCount, db.EntryCount);

			Assert.AreEqual(232, db.AbbreviationCount);
			Assert.AreEqual(1457, db.EntryCount);
			Assert.AreEqual(11264, parser.Current.Line);
			Assert.AreEqual(1, parser.Current.Column);
			Assert.AreEqual(359616, parser.Current.Position);
		}
		 
		[Test]
		public void DocearParser()
		{
			var data = File.ReadAllText(@"Sample Files\Docear.bib");

			var parser = new BibTexParser(new BibTexLexer(data));

			var db = parser.Parse();
			Console.WriteLine("{0} abbreviations; {1} entries", db.AbbreviationCount, db.EntryCount);

			Assert.AreEqual(8, db.AbbreviationCount);
			Assert.AreEqual(1064, db.EntryCount);
			Assert.AreEqual(17638, parser.Current.Line);
			Assert.AreEqual(1, parser.Current.Column);
			Assert.AreEqual(682497, parser.Current.Position);
		}
		 
		[Test]
		public void Mendeley1Parser()
		{
			var data = File.ReadAllText(@"Sample Files\Mendeley 1.bib");

			var parser = new BibTexParser(new BibTexLexer(data));

			var db = parser.Parse();
			Console.WriteLine("{0} abbreviations; {1} entries", db.AbbreviationCount, db.EntryCount);

			Assert.AreEqual(0, db.AbbreviationCount);
			Assert.AreEqual(1227, db.EntryCount);
			Assert.AreEqual(14633, parser.Current.Line);
			Assert.AreEqual(1, parser.Current.Column);
			Assert.AreEqual(740249, parser.Current.Position);
		}
		 
		[Test]
		public void Mendeley2Parser()
		{
			var data = File.ReadAllText(@"Sample Files\Mendeley 2.bib");

			var parser = new BibTexParser(new BibTexLexer(data));

			var db = parser.Parse();
			Console.WriteLine("{0} abbreviations; {1} entries", db.AbbreviationCount, db.EntryCount);

			Assert.AreEqual(0, db.AbbreviationCount);
			Assert.AreEqual(1231, db.EntryCount);
			Assert.AreEqual(14673, parser.Current.Line);
			Assert.AreEqual(1, parser.Current.Column);
			Assert.AreEqual(713525, parser.Current.Position);
		}

		[Test]
		public void TempParser()
		{
			var data = File.ReadAllText(@"Sample Files\Temp.bib");

			var parser = new BibTexParser(new BibTexLexer(data));

			var db = parser.Parse();
			Console.WriteLine("{0} abbreviations; {1} entries", db.AbbreviationCount, db.EntryCount);
		}

		[Test]
		public void AccuteTests()
		{
			var title = specialCharactersDatabase["Accute"]["title"];

			Assert.AreEqual("CP1252+Latex (\\’): and ÁÁÉÉÍÍÓÓÚÚÝÝááééííóóúúýý", title.Display);
		}

		[Test]
		public void BreveTests()
		{
			var title = specialCharactersDatabase["Breve"]["title"];

			Assert.AreEqual("Base+Latex (\\u): AĂaă", title.Display);
		}
		 
		[Test]
		public void CedillaTests()
		{
			var title = specialCharactersDatabase["Cedilla"]["title"];

			Assert.AreEqual("CP1252+Latex (\\c): ÇÇçç", title.Display);
		}
		 
		[Test]
		public void CircumflexTests()
		{
			var title = specialCharactersDatabase["Circumflex"]["title"];

			Assert.AreEqual("CP1252+Latex (\\^): ÂÂÊÊÎÎÔÔÛÛââêêîîôôûû", title.Display);
		}
		 
		[Test]
		public void EquationTests()
		{
			var title = specialCharactersDatabase["Equation"]["title"];

			Assert.AreEqual("Equ: x^2+y^10+z_0=H^∞", title.Display);
		}
		 
		[Test]
		public void EscapesTests()
		{
			var title = specialCharactersDatabase["Escapes"]["title"];

			Assert.AreEqual("Latex Special characters: \\{}_^~ _", title.Display);
		}
		 
		[Test]
		public void GraveTests()
		{
			var title = specialCharactersDatabase["Grave"]["title"];

			Assert.AreEqual("CP1252+Latex (\\‘): ÀÀÈÈÌÌÒÒÙÙààèèììòòùù", title.Display);
		}
		 
		[Test]
		public void GreekTests()
		{
			var title = specialCharactersDatabase["Greek"]["title"];

			Assert.AreEqual("Latex: ΑαΒβ...Ωω", title.Display);
		}
		 
		[Test]
		public void HacekTests()
		{
			var title = specialCharactersDatabase["Hacek"]["title"];

			Assert.AreEqual("CP1252+Latex (\\v): ŠŠššŽŽžž", title.Display);
		}
		 
		[Test]
		public void OverbarTests()
		{
			var title = specialCharactersDatabase["Overbar"]["title"];

			Assert.AreEqual("Base+Latex (\\=): AĀaā", title.Display);
		}
		 
		[Test]
		public void OverdotTests()
		{
			var title = specialCharactersDatabase["Overdot"]["title"];

			Assert.AreEqual("Base+Latex (\\.): GĠgġ", title.Display);
		}
		 
		[Test]
		public void LatexSubstitutionsTests()
		{
			var title = specialCharactersDatabase["LatexSub"]["title"];

			//Assert.AreEqual("En dash:–, Em dash:—, Non-breaking space:\xa0, Inverted Query:¿,\r\n\tInverted Shreik:¡, Single Quote:‘’, Double Quote:“”", title.Display);
			Assert.AreEqual("En dash:–, Em dash:—, Non-breaking space:\xa0, Inverted Query:¿, Inverted Shreik:¡, Single Quote:‘’, Double Quote:“”", title.Display);
		}
		 
		[Test]
		public void MathematicalSymbolsTests()
		{
			var title = specialCharactersDatabase["MathSym"]["title"];

			Assert.AreEqual("Latex: ∞∀∃ℵ∼", title.Display);
		}
		 
		[Test]
		public void TildeTests()
		{
			var title = specialCharactersDatabase["Tilde"]["title"];

			Assert.AreEqual("CP1252+Latex (\\~): ÃÃÑÑããññ", title.Display);
		}
		 
		[Test]
		public void UmlautTests()
		{
			var title = specialCharactersDatabase["Umlaut"]["title"];

			Assert.AreEqual("CP1252+Latex (\\\"): ÄÄËËÏÏÖÖÜÜŸŸääëëïïööüüÿÿ", title.Display);
		}
		 
		[Test]
		public void HungarianUmlautTests()
		{
			var title = specialCharactersDatabase["HungarianUmlaut"]["title"];

			Assert.AreEqual("Base+Latex (\\H): OŐoő", title.Display);
		}
		 
		[Test]
		public void UnicodeTests()
		{
			var title = specialCharactersDatabase["Unicode"]["title"];

			//Assert.AreEqual("Infinity: dec=∞, dq=∞, wrong=?,\r\n\tsmiles=☺, equation: x^∞=∞", title.Display);
			Assert.AreEqual("Infinity: dec=∞, dq=∞, wrong=?, smiles=☺, equation: x^∞=∞", title.Display);
		}
		 
		[Test]
		public void VerbatimTests()
		{
			var title = specialCharactersDatabase["Verbatim"]["title"];

			Assert.AreEqual("Latex: ~\\{}", title.Display);
		}

		[Test]  // From http://theoval.cmp.uea.ac.uk/~nlct/latex/novices/symbols.html
		public void SymbolTests()
		{
			var text = @"@MISC{LatexSymbols,title = {\textbackslash \_ - -- \textendash --- \textemdash \P \textasciicircum \$ \S \textasciitilde \{ \ldots \pounds \} ?` \textquestiondown \textregistered \# !` \textexclamdown \texttrademark \% ' \textquoteright '' \textquotedblright \copyright \& ` \textquoteleft `` \textquotedblleft \yen \i \j \textbar \textperiodcentered \textless \textgreater \slash}";
			var title = new BibTexParser(new BibTexLexer(text)).Parse()["LatexSymbols"]["title"];

			Assert.AreEqual("\\ _ - – – — — ¶ ^ $ § ~ { … £ } ¿ ¿ ® # ¡ ¡ \x2122 % ’ ’ ” ” © & ‘ ‘ “ “ ¥ \uD835\uDEA4 \uD835\uDEA5 | · < > /", title.Display);
		}
		 
	}
}