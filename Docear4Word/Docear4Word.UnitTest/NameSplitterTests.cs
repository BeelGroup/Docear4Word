using System;

using Docear4Word;
using Docear4Word.BibTex;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace docear4word.UnitTest
{
	[TestFixture]
	public class NameSplitterTests
	{
		static readonly NameSplitter Splitter = new NameSplitter();

		static void Check(CSLNameComponents nameComponents, 
			string expectedFamily = null,
			string expectedGiven = null,
			string expectedDroppingParticle = null,
			string expectedNonDroppingParticle = null,
			string expectedSuffix = null,
			bool? expectedCommaSuffix = null,
			bool? expectedStaticOrdering = null	)
		{
			if (expectedFamily != null)
			{
				Assert.AreEqual(expectedFamily, nameComponents.Family, "Family");
			}

			if (expectedGiven != null)
			{
				Assert.AreEqual(expectedGiven, nameComponents.Given, "Given");
			}	

			if (expectedDroppingParticle != null)
			{
				Assert.AreEqual(expectedDroppingParticle, nameComponents.DroppingParticle, "Dropping Particle");
			}	

			if (expectedNonDroppingParticle != null)
			{
				Assert.AreEqual(expectedNonDroppingParticle, nameComponents.NonDroppingParticle, "Non-Dropping Particle");
			}	

			if (expectedSuffix != null)
			{
				Assert.AreEqual(expectedSuffix, nameComponents.Suffix, "Suffix");
			}	

			if (expectedCommaSuffix != null)
			{
				Assert.AreEqual(expectedCommaSuffix, nameComponents.CommaSuffix, "Comma-Suffix");
			}	

			if (expectedStaticOrdering != null)
			{
				Assert.AreEqual(expectedStaticOrdering, nameComponents.StaticOrdering, "Static-Ordering");
			}	
		}

		[Test]
		public void TestSplitName_NoCommas_01()
		{
			const string name = "jean de la fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine", expectedGiven:"", expectedDroppingParticle: "jean de la");
		}

		[Test]
		public void TestSplitName_NoCommas_02()
		{
			const string name = "Jean de la fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine", expectedGiven:"Jean", expectedDroppingParticle: "de la");
		}

		[Test]
		public void TestSplitName_NoCommas_03()
		{
			const string name = "Jean {de} la fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine", expectedGiven:"Jean de", expectedDroppingParticle: "la");
		}

		[Test]
		public void TestSplitName_NoCommas_04()
		{
			const string name = "jean {de} {la} fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "de la fontaine", expectedDroppingParticle: "jean");
		}

		[Test]
		public void TestSplitName_NoCommas_05()
		{
			const string name = "Jean {de} {la} fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine", expectedGiven:"Jean de la");
		}

		[Test]
		public void TestSplitName_NoCommas_06()
		{
			const string name = "Jean De La Fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "Fontaine", expectedGiven:"Jean De La");
		}

		[Test]
		public void TestSplitName_NoCommas_07()
		{
			const string name = "jean De la Fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "Fontaine", expectedDroppingParticle: "jean De la");
		}

		[Test]
		public void TestSplitName_NoCommas_08()
		{
			const string name = "Jean de La Fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "La Fontaine", expectedGiven:"Jean", expectedDroppingParticle: "de");
		}

		[Test]
		public void TestSplitName_SingleComma_01()
		{
			const string name = "jean de la fontaine,";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine", expectedDroppingParticle: "jean de la");
		}

		[Test]
		public void TestSplitName_SingleComma_02()
		{
			const string name = "de la fontaine, Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "de la", expectedFamily: "fontaine");
		}

		[Test]
		public void TestSplitName_SingleComma_03()
		{
			const string name = "De La Fontaine, Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedFamily: "De La Fontaine");
		}

		[Test]
		public void TestSplitName_SingleComma_04()
		{
			const string name = "De la Fontaine, Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "De la", expectedFamily: "Fontaine");
		}

		[Test]
		public void TestSplitName_SingleComma_05()
		{
			const string name = "de La Fontaine, Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "de", expectedFamily: "La Fontaine");
		}

		[Test]
		public void TestSplitName_DualComma_01()
		{
			const string name = "jean de la fontaine, Jr.,";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine", expectedDroppingParticle: "jean de la Jr.");
		}

		[Test]
		public void TestSplitName_DualComma_02()
		{
			const string name = "de la fontaine, Jr., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "de la Jr.", expectedFamily: "fontaine");
		}

		[Test]
		public void TestSplitName_DualComma_03()
		{
			const string name = "De La Fontaine, Jr., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "Jr.", expectedFamily: "De La Fontaine");
		}

		[Test]
		public void TestSplitName_DualComma_04()
		{
			const string name = "De la Fontaine, Jr., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "De la Jr.", expectedFamily: "Fontaine");
		}

		[Test]
		public void TestSplitName_DualComma_05()
		{
			const string name = "de La Fontaine, Jr., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "de Jr.", expectedFamily: "La Fontaine");
		}

		[Test]
		public void TestSplitName_MultiComma_01()
		{
			const string name = "jean de la fontaine, Jr., Hons.,";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine", expectedDroppingParticle: "jean de la Jr. Hons.");
		}

		[Test]
		public void TestSplitName_MultiComma_02()
		{
			const string name = "de la fontaine, Jr., Hons., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "de la Jr. Hons.", expectedFamily: "fontaine");
		}

		[Test]
		public void TestSplitName_MultiComma_03()
		{
			const string name = "De La Fontaine, Jr., Hons., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "Jr. Hons.", expectedFamily: "De La Fontaine");
		}

		[Test]
		public void TestSplitName_MultiComma_04()
		{
			const string name = "De la Fontaine, Jr., Hons., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "De la Jr. Hons.", expectedFamily: "Fontaine");
		}

		[Test]
		public void TestSplitName_MultiComma_05()
		{
			const string name = "de La Fontaine, Jr., Hons., Jean";

			Check(Splitter.Split(name)[0], expectedGiven: "Jean", expectedDroppingParticle: "de Jr. Hons.", expectedFamily: "La Fontaine");
		}

		[Test]
		public void TestSinglePartIsFamily()
		{
			const string name = "fontaine";

			Check(Splitter.Split(name)[0], expectedFamily: "fontaine");
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void TestNullName()
		{
			const string name = null;

			Splitter.Split(name);
		}

		[Test]
		public void TestEmptyName()
		{
			const string name = "";

			var result = Splitter.Split(name);
			Assert.AreEqual(0, result.Length);
		}

		[TestCase(",")]
		[TestCase(",,")]
		[TestCase(",,,")]
		[TestCase(" , , , ")]
		public void TestSingleComma(string name)
		{
			var result = Splitter.Split(name);
			Assert.AreEqual(0, result.Length);
		}

		[Test]
		public void TestAdjoined()
		{
			const string name = "{de Solla Price}, {D}erek {J}.";
			Check(Splitter.Split(name)[0], expectedFamily: "de Solla Price", expectedGiven: "Derek J.");
		}

		[Test]
		public void TestAdjoined02()
		{
			const string name = "{de {Solla} Price}, Dere{k} {J}.";
			Check(Splitter.Split(name)[0], expectedFamily: "de Solla Price", expectedGiven: "Derek J.");
		}

		[Test]
		public void TestAdjoined03()
		{
			const string name = "{de Solla Price}, {D}er{}e{k} {} {J}";
			Check(Splitter.Split(name)[0], expectedFamily: "de Solla Price", expectedGiven: "Derek J");
		}

		TagEntry GetAuthorTagFromParser(string name)
		{
			var text = @"@article{Test, author = {" + name + @"}}";
			var database = BibTexHelper.CreateBibTexDatabase(text);

			return database["Test"]["author"];
		}

		[Test]
		public void TestSpecial()
		{
			var authorTag = GetAuthorTagFromParser(@"Maciej M. Sys{\l}o");

			var display = authorTag.Display;
			Console.WriteLine(display);
			var verbatim = authorTag.Verbatim;
			Console.WriteLine(verbatim);
			var forNameParse = new LatexMiniParser(authorTag.Verbatim).Parse();
			Console.WriteLine(forNameParse);

			Check(Splitter.Split(forNameParse)[0], expectedFamily: "Syslo", expectedGiven: "Maciej M.");
		}

		[Test]
		public void TestSpecialBracedAccentedChars()
		{
			Assert.AreEqual("Höding", new LatexMiniParser(@"{H}{""o}ding").Parse());
			Assert.AreEqual("Straßburger", new LatexMiniParser(@"{S}tra{""s}burger").Parse());
		}

		[Test]
		public void TestPreserveContentInBraces()
		{
			Assert.AreEqual("Google Inc.", new LatexMiniParser(@"{Google Inc.}").Parse());
			Assert.AreEqual("Google Inc.", new LatexMiniParser(@"{{Google Inc.}}").Parse());

			var authorTag = GetAuthorTagFromParser(@"{Google Inc.}");
			Assert.AreEqual("Google Inc.", authorTag.Display);

			authorTag = GetAuthorTagFromParser(@"{{Google Inc.}}");
			Assert.AreEqual("Google Inc.", authorTag.Display);
		}

		[Test]
		public void TestPreserveContentInBraces2()
		{
			var authorTag = GetAuthorTagFromParser(@"{International Energy Agency}");

			var display = authorTag.Display;
			Console.WriteLine(display);
			var verbatim = authorTag.Verbatim;
			Console.WriteLine(verbatim);
			var forNameParse = new LatexMiniParser(authorTag.Verbatim).Parse();
			Console.WriteLine(forNameParse);

			var cslNameComponents = Splitter.Split(forNameParse)[0];
			Check(cslNameComponents, expectedFamily: "Syslo", expectedGiven: "Maciej M.");
		}
/*
		[Test]
		public void TestPreserveContentInBraces2()
		{
			var authorTag = GetAuthorTagFromParser(@"{International Energy Agency}");

			var display = authorTag.Display;
			Console.WriteLine(display);
			var verbatim = authorTag.Verbatim;
			Console.WriteLine(verbatim);
			var forNameParse = new LatexMiniParser(authorTag.Verbatim).Parse();
			Console.WriteLine(forNameParse);

			var cslNameComponents = Splitter.Split(forNameParse)[0];
			Check(cslNameComponents, expectedFamily: "Syslo", expectedGiven: "Maciej M.");
		}
*/
/*
		[Test]
		public void TestPreserveContentInBraces2()
		{
			var text = @"@Book{IEA2012,
Title = {World Energy Outlook 2012},
Author = {{International Energy Agency}},
Year = {2012}
}";

			var database = BibTexHelper.CreateBibTexDatabase(text);

			var author = database[0]["author"];

			var forNameParse = new LatexMiniParser(author).Parse();
			Console.WriteLine(forNameParse);

			Check(Splitter.Split(forNameParse)[0], expectedFamily: "Syslo", expectedGiven: "Maciej M.");
		}
*/

		[Test]
		public void Temp()
		{
			foreach (var specialFolder in Enum.GetValues(typeof(Environment.SpecialFolder)))
			{
				Console.WriteLine("{0}->'{1}'", specialFolder, Environment.GetFolderPath((Environment.SpecialFolder) specialFolder));
			}

			var stylesInfo = StyleHelper.GetStylesInfo();
			foreach (var styleInfo in stylesInfo)
			{
				Console.WriteLine("{0} ({1})", styleInfo.Title, styleInfo.FileInfo.Name);
			}
		}
	}
}