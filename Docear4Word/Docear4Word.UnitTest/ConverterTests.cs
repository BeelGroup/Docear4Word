using System;
using System.IO;

using Docear4Word;

using NUnit.Framework;

namespace docear4word.UnitTest
{
	[TestFixture, RequiresSTA]
	public class ConverterTests
	{
		static readonly StyleInfo HavardCslStyle = StyleHelper.LoadFromFile(@"Sample Files\harvard1.csl");

		[Test]
		public void TestBibTexToCSLConverter()
		{
			var database = BibTexHelper.LoadBibTextDatabase(@"Sample Files\Docear.bib");
			var converter = new BibTexToCSLConverter();
/*
var temp = new BibTeXDatabase();
temp.AddEntry(database[0]);
temp.AddEntry(database[1]);
database = temp;
*/
			Console.WriteLine(converter.ToJSON(database));
		}

		[Test]
		public void XXX()
		{
			var database = BibTexHelper.LoadBibTextDatabase(@"Sample Files\Mendeley 2.bib");
			//var database = BibTexHelper.LoadBibTextDatabase(@"Sample Files\b4w.bib");
			//var database = BibTexHelper.LoadBibTextDatabase(@"Sample Files\BigFile.bib");
			var converter = new BibTexToCSLConverter();

			var q = converter.ToJSON(database);
			Console.WriteLine(q);
			File.WriteAllText(@"F:\old.txt", q);
		}
		[Test]
		public void XXXNew()
		{

			var database = BibTexHelper.LoadBibTextDatabase(@"Sample Files\Mendeley 2.bib");
			//var database = BibTexHelper.LoadBibTextDatabase(@"Sample Files\b4w.bib");
			//var database = BibTexHelper.LoadBibTextDatabase(@"Sample Files\BigFile.bib");
			var citeproc = new CiteProcRunner(HavardCslStyle, () => database);
			var converter = new BibTexToCSLConverter(citeproc);

			var result = converter.CreateItem(database);
			var q = citeproc.ToJSON(result.JSObject);

			File.WriteAllText(@"F:\new.txt", q);

//			Debug.WriteLine(q);
		}

		[Test]
		public void XXX2()
		{
			var text = @"
@techreport{Brandstadt91a,
author = {Andreas Brandst\""{a}dt},
title = {Special Graph Classes --- A Survey (preliminary version)},
institution = {University of Duisburg},
year = 1991}
";
			var database = BibTexHelper.CreateBibTextDatabase(text);
			var converter = new BibTexToCSLConverter();

			Console.WriteLine(converter.ToJSON(database));
		}
	}
}