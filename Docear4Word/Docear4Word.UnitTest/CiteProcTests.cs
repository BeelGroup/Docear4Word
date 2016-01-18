using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

using Docear4Word;
using Docear4Word.BibTex;

using NUnit.Framework;

using System.Linq;

// ReSharper disable InconsistentNaming

namespace docear4word.UnitTest
{
	[TestFixture, RequiresSTA]
	public class CiteProcTests
	{
		const string SampleInlineJSON = @"{
  ""schema"": ""https://raw.github.com/citation-style-language/schema/master/csl-citation.json"",
  ""citationID"": ""{41d5b922-216f-4218-b0bf-6c55ab99702f}"",
  ""properties"": {
    ""noteIndex"": 2
  },
  ""citationItems"": [
    {
      ""itemData"": {
        ""id"": ""Agarwal05"",
        ""type"": ""article"",
        ""container-title"": ""Lecture notes in computer science"",
        ""keyword"": ""research paper recommender"",
        ""page"": ""475"",
        ""title"": ""Research Paper Recommender Systems: A Subspace Clustering Approach"",
        ""author"": [
          {
            ""family"": ""Agarwal"",
            ""given"": ""N.""
          },
          {
            ""family"": ""Haque"",
            ""given"": ""E.""
          },
          {
            ""family"": ""Liu"",
            ""given"": ""H.""
          },
          {
            ""family"": ""Parsons"",
            ""given"": ""L.""
          }
        ],
        ""issued"": {
          ""date-parts"": [
            [
              2005
            ]
          ]
        },
        ""volume"": ""3739""
      },
      ""id"": ""Agarwal05""
    },
    {
      ""itemData"": {
        ""id"": ""Small85"",
        ""type"": ""article"",
        ""container-title"": ""Scientometrics"",
        ""page"": ""391–409"",
        ""publisher"": ""Akadémiai Kiadó, co-published with Springer Science+ Business Media BV, Formerly Kluwer Academic Publishers BV"",
        ""title"": ""Clustering thescience citation indextextregistered using co-citations"",
        ""author"": [
          {
            ""family"": ""Small"",
            ""given"": ""H.""
          },
          {
            ""family"": ""Sweeney"",
            ""given"": ""E.""
          }
        ],
        ""issued"": {
          ""date-parts"": [
            [
              1985
            ]
          ]
        },
        ""Number"": ""3"",
        ""volume"": ""7""
      },
      ""id"": ""Small85""
    }
  ]
}";
		static readonly StyleInfo MlaCslStyle = StyleHelper.LoadFromFile(@"Sample Files\mla.csl");
		static readonly StyleInfo HavardCslStyle = StyleHelper.LoadFromFile(@"Sample Files\harvard1.csl");
		static readonly BibTexDatabase DocearDatabase = BibTexHelper.LoadBibTexDatabase(@"Sample Files\Docear.bib");

		[Test]
		public void DumpNames()
		{
			var x = new Regex(@"\{(`|'|\^|""|\~|c|v|u|\=|\.|H|b|d).\}");
			var count = 0;

			foreach(var entry in DocearDatabase.Entries)
			{
				var title = entry["title", TagEntry.Empty];
				var author = entry["author", TagEntry.Empty];

				if (x.IsMatch(title.Verbatim))
				{
					//Console.WriteLine(string.Format("'{0}'\r\n  '{1}'", title.Verbatim, title.Display));
					Console.WriteLine(title.Display);
					count++;
				}

				if (x.IsMatch(author.Verbatim))
				{
					//Console.WriteLine(string.Format("'{0}'\r\n  '{1}'", author.Verbatim, author.Display));
					Console.WriteLine(author.Display);
					count++;
				}
/*

				if (title.Verbatim.Contains("{\"")) Console.WriteLine(string.Format("'{0}'=>'{1}'", title.Verbatim, title.Display));
				if (author.Verbatim.Contains("{\"")) Console.WriteLine(string.Format("'{0}'=>'{1}'", author.Verbatim, author.Display));
*/
			}

			Console.WriteLine();
			Console.WriteLine(count);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		[TestCase(7)]
		[TestCase(8)]
		[TestCase(9)]
		[TestCase(10)]
		public void Test01(int x)
		{
			var citeproc = new CiteProcRunner(HavardCslStyle, () => DocearDatabase);
			var listOfItems = new[] { "Agarwal05", "Agrawal08", "Aho75" };

			var result = citeproc.UpdateItems(listOfItems, false, true);
			foreach (var s in result) Console.WriteLine(s);

			var bib = citeproc.MakeBibliography();

			foreach(var entry in bib.Entries)
			{
				Console.WriteLine(entry);
			}
		}

		[Test, RequiresThread(ApartmentState.STA)]
		public void Test02()
		{
			var database = BibTexHelper.LoadBibTexDatabase(@"Sample Files\Docear.bib");
			var citeproc = new CiteProcRunner(HavardCslStyle, () => database );

			var idList = database.GetEntryNames().ToArray();
idList = database.GetEntryNames().Take(300).ToArray();
			Console.WriteLine("Total of " + idList.Length + " items.");


			citeproc.UpdateItems(idList, false, true);

			//var bib = citeproc.MakeBibliography();

			//foreach (var entry in bib.Entries) Console.WriteLine(entry);
		}

/*
		[Test, RequiresThread(ApartmentState.STA)]
		public void Test_CombinedReferencesInCitationItem()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle) { Database = DocearDatabase };
			citeproc.SetOutputFormat("text");

			var before = CitationAndNotePair.EmptyList;
			var after = CitationAndNotePair.EmptyList;

			var citation = new Citation
			               	{
			               		CitationItems = new List<CitationItem>
			               		                	{
			               		                		new CitationItem { ID = "Agarwal05" },
			               		                		new CitationItem { ID = "Small85" }
			               		                	}
			               	};

			var result = citeproc.ProcessCitationCluster(citation, before, after);
			Assert.AreEqual(1, result.AffectedCitations.Count);
			Assert.AreEqual("(Agarwal et al. 2005; Small & Sweeney 1985)", result.AffectedCitations[0].Value);
		}
*/

/*
		[Test, RequiresThread(ApartmentState.STA)]
		public void SingleReferenceInCitationItem()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle) { Database = DocearDatabase };
			citeproc.SetOutputFormat("text");

			var citation = new Citation
			               	{
			               		CitationItems = new List<CitationItem>
			               		                	{
			               		                		new CitationItem { ID = "Agarwal05" },
			               		                	}
			               	};

			var result = citeproc.ProcessCitationCluster(citation, CitationAndNotePair.EmptyList, CitationAndNotePair.EmptyList);
			Assert.AreEqual(1, result.AffectedCitations.Count);
			Assert.AreEqual("(Agarwal et al. 2005)", result.AffectedCitations[0].Value);
		}
*/

/*
		[Test, RequiresThread(ApartmentState.STA)]
		public void TwoSequentialCitationItem()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle) { Database = DocearDatabase };
			citeproc.SetOutputFormat("text");

			var citation1 = new Citation
			               	{
			               		CitationItems = new List<CitationItem>
			               		                	{
			               		                		new CitationItem { ID = "Agarwal05" },
			               		                	}
			               	};

			var result = citeproc.ProcessCitationCluster(citation1, CitationAndNotePair.EmptyList, CitationAndNotePair.EmptyList);
			Assert.AreEqual(1, result.AffectedCitations.Count);
			Assert.AreEqual("(Agarwal et al. 2005)", result.AffectedCitations[0].Value);

			var citation2 = new Citation
			               	{
			               		CitationItems = new List<CitationItem>
			               		                	{
			               		                		new CitationItem { ID = "Small85" },
			               		                	}
			               	};
			var before = new CitationAndNotePair
			             	{
								CitationId = citation1.CitationID,
								NoteIndex = citation1.Properties.NoteIndex
			             	};

			result = citeproc.ProcessCitationCluster(citation2, new[] { before }, CitationAndNotePair.EmptyList);
			Assert.AreEqual(1, result.AffectedCitations.Count);
			Assert.AreEqual("(Small & Sweeney 1985)", result.AffectedCitations[0].Value);
		}
*/

/*
		[Test, RequiresThread(ApartmentState.STA)]
		public void Test_CombinedReferencesInCitationItemX()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle) { Database = DocearDatabase };
			citeproc.SetOutputFormat("text");

			var citation = new Citation
			               	{
			               		CitationItems = new List<CitationItem>
			               		                	{
			               		                		new CitationItem { ID = "Agarwal05" },
			               		                		new CitationItem { ID = "Small85" }
			               		                	}
			               	};

			var result = citeproc.ProcessCitationCluster(citation, CitationAndNotePair.EmptyList, CitationAndNotePair.EmptyList);
			Assert.AreEqual(1, result.AffectedCitations.Count);
			Assert.AreEqual("(Agarwal et al. 2005; Small & Sweeney 1985)", result.AffectedCitations[0].Value);
		}
*/

/*
		[Test, RequiresThread(ApartmentState.STA)]
		public void Test_CombinedReferencesAsJavaScriptObjects()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle) { Database = DocearDatabase };
			citeproc.SetOutputFormat("text");

			//var citation = new JSCitation(citeproc, 0, null, "Agarwal05", "Small85");
			var citation = new JSCitation(citeproc);
			citation.Properties.NoteIndex = 0;
			citation.Items.Add(new JSCitationItem(citeproc) { ID = "Agarwal05"});
			citation.Items.Add(new JSCitationItem(citeproc) { ID = "Small85"});

			Console.WriteLine(citeproc.AppendCitation(citation));
/*

			var result = citeproc.ProcessCitationCluster(citation, CitationAndNotePair.EmptyList, CitationAndNotePair.EmptyList);
			Assert.AreEqual(1, result.AffectedCitations.Count);
			Assert.AreEqual("(Agarwal et al. 2005; Small & Sweeney 1985)", result.AffectedCitations[0].Value);
#1#
		}
*/

/*
		[Test]
		public void TestJSONRoundTripWithWordLF()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle, () => DocearDatabase);
			var json = citeproc.GetJSONForEntryName("Agarwal05");
			Console.WriteLine(json);

			var jsObject = citeproc.Call("createObjectFromJSON", json);

			var newJson = citeproc.ToJSON(jsObject);
			Console.WriteLine(newJson);

		}
*/

/*
		[Test]
		public void TestJSONWithCustomPageNumber()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle, () => DocearDatabase);
			var json = citeproc.GetJSONForEntryName("Agarwal05", "Agarwal05#123");
			Console.WriteLine(json);
		}
*/

/*
		[Test]
		public void ReadInlineCitation()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle, () => DocearDatabase);
			var sourceJSON = SampleInlineJSON.Replace("\r\n", "\n");

			var restoredCitation = JSInlineCitation.FromJSON(citeproc, sourceJSON);
			var newJSON = citeproc.ToJSON(restoredCitation.JSObject);
			Assert.AreEqual(sourceJSON, newJSON);
			Assert.AreEqual(2, restoredCitation.CitationItems.Length);
		}
*/

		[Test]
		public void CheckSameReferenceWithDifferentPagesProducesSingleEntryInBibliography()
		{
			var citeProc = new CiteProcRunner(MlaCslStyle, () => DocearDatabase);
			var citeInserter = new TestCitationInserter(citeProc);

			var citation1 = new EntryAndPagePair(DocearDatabase["price65"], "1");
			var citation2 = new EntryAndPagePair(DocearDatabase["price65"], "2");

			var entryAndPagePairs =
				new[]
					{
						citation1,
						citation2
					};

			var inlineCitation1 = citeInserter.CreateInlineCitation(citation1);
			var inlineCitation2 = citeInserter.CreateInlineCitation(citation2);

			var jsCitations = new object[0];
			var jsResult = citeProc.RestoreProcessorState(jsCitations);



			var bibliographyResult = citeProc.MakeBibliography();

		}

		class TestCitationInserter
		{
			readonly CiteProcRunner citeProc;
			readonly Dictionary<string, JSInlineCitation> inlineCitationCache = new Dictionary<string, JSInlineCitation>();

			public TestCitationInserter(CiteProcRunner citeProc)
			{
				this.citeProc = citeProc;
			}

			public JSInlineCitation CreateInlineCitation(EntryAndPagePair itemSource, object idToUse = null) { return CreateInlineCitation(new[] { itemSource }, idToUse); }
			public JSInlineCitation CreateInlineCitation(IEnumerable<EntryAndPagePair> itemSources, object idToUse = null)
			{
				// ****IMPORTANT****
				// This is called from InsertCitationSequence, InsertCitation, EditCitation and CitationInserter.UpdateCitationsFromDatabase
				//
				// It is imperative that calls from the first 3 work on an empty CiteProc otherwise the cache gets used to create
				// the citation items. Other than first-use, this means using the item after CiteProc has seen it and maybe modified it
				// (it appears to change the Date Parts to strings in some cases)
				// The next refresh is then comparing incorrect JSON and will want to update it from the database.
				//
				// (CitationInserter.UpdateCitationsFromDatabase calls here but this is always within a Refresh which means a brand new CiteProc anyway
				// and so multiple resets here are not a problem because the raw cache would be empty anyway)
	//			citeproc.ResetProcessorState();

				var result = new JSInlineCitation(citeProc);

				if (idToUse != null)
				{
					result.CitationID = idToUse;
				}

				result.Properties.NoteIndex = 0;

				foreach(var itemSource in itemSources)
				{
					var inlineCitationItem = citeProc.CreateJSInlineCitationItem(itemSource);

					result.CitationItems.Add(inlineCitationItem);
				}

				// We store this before Citeproc gets hold of it!
				result.FieldCodeJSON = citeProc.ToJSON(result.JSObject).Replace('\n', '\v') + "\r";

				return result;
			}

			void RemoveCitation(string fieldCodeText)
			{
				inlineCitationCache.Remove(fieldCodeText);
			}

			void SetCitation(string fieldCodeText, JSInlineCitation citation)
			{
				inlineCitationCache[fieldCodeText] = citation;
			}

			void ClearCitationsCache()
			{
				inlineCitationCache.Clear();
			}

			JSInlineCitation GetCitation(string fieldCodeText)
			{
				JSInlineCitation result;

				if (!inlineCitationCache.TryGetValue(fieldCodeText, out result))
				{
					//TODO: Move this to common routine
					var indexOfOpeningBrace = fieldCodeText.IndexOf('{');
					var citationJSON = fieldCodeText.Substring(indexOfOpeningBrace);

					result = JSInlineCitation.FromJSON(citeProc, citationJSON);

					SetCitation(fieldCodeText, result);
				}

				return result;
			}

			public void Reset()
			{
				
			}
		}



		[Test]
		public void NewInlineCitation()
		{
			var citeproc = new CiteProcRunner(HavardCslStyle, () => DocearDatabase);

			var citation = new JSInlineCitation(citeproc);
			Console.WriteLine(citeproc.ToJSON(citation));

			Assert.NotNull(citation.CitationItems);
			Assert.NotNull(citation.Properties);
			Console.WriteLine(citeproc.ToJSON(citation));

			var citationItem = citation.CitationItems.AddNew();
			Console.WriteLine(citeproc.ToJSON(citation));

			citationItem.ID = "123";
			citationItem.Prefix = "bollox";
			Console.WriteLine(citeproc.ToJSON(citation));
		}

		[Test]
		public void CiteProcSampleData()
		{
			var data =
				@"{
	""ITEM-1"": {
		""id"": ""ITEM-1"",
		""title"":""Boundaries of Dissent: Protest and State Power in the Media Age"",
		""author"": [
			{
				""family"": ""D'Arcus"",
				""given"": ""Bruce"",
				""static-ordering"": false
			}
		],
        ""note"":""The apostrophe in Bruce's name appears in proper typeset form."",
		""publisher"": ""Routledge"",
        ""publisher-place"": ""New York"",
		""issued"": {
			""date-parts"":[
				[2006]
			]
		},
		""type"": ""book""
	},
	""ITEM-2"": {
		""id"": ""ITEM-2"",
		""author"": [
			{
				""family"": ""Bennett"",
				""given"": ""Frank G."",
				""suffix"": ""Jr."",
				""comma-suffix"": true,
				""static-ordering"": false
			}
		],
		""title"":""Getting Property Right: \""Informal\"" Mortgages in the Japanese Courts"",
		""container-title"":""Pacific Rim Law & Policy Journal"",
		""volume"": ""18"",
		""page"": ""463-509"",
		""issued"": {
			""date-parts"":[
				[2009, 8]
			]
		},
		""type"": ""article-journal"",
        ""note"": ""Note the flip-flop behavior of the quotations marks around \""informal\"" in the title of this citation.  This works for quotation marks in any style locale.  Oh, and, uh, these notes illustrate the formatting of annotated bibliographies (!).""
	},
	""ITEM-3"": {
		""id"": ""ITEM-3"",
		""title"":""Key Process Conditions for Production of C<sub>4</sub> Dicarboxylic Acids in Bioreactor Batch Cultures of an Engineered <i>Saccharomyces cerevisiae</i> Strain"",
        ""note"":""This cite illustrates the rich text formatting capabilities in the new processor, as well as page range collapsing (in this case, applying the collapsing method required by the Chicago Manual of Style).  Also, as the IEEE example above partially illustrates, we also offer robust handling of particles such as \""van\"" and \""de\"" in author names."",
		""author"": [
			{
				""family"": ""Zelle"",
				""given"": ""Rintze M.""
			},
			{
				""family"": ""Hulster"",
				""given"": ""Erik"",
				""non-dropping-particle"":""de""
			},
			{
				""family"": ""Kloezen"",
				""given"": ""Wendy""
			},
			{
				""family"":""Pronk"",
				""given"":""Jack T.""
			},
			{
				""family"": ""Maris"",
				""given"":""Antonius J.A."",
				""non-dropping-particle"":""van""
			}
		],
		""container-title"": ""Applied and Environmental Microbiology"",
		""issued"":{
			""date-parts"":[
				[2010, 2]
			]
		},
		""page"": ""744-750"",
		""volume"":""76"",
		""issue"": ""3"",
		""DOI"":""10.1128/AEM.02396-09"",
		""type"": ""article-journal""
	},
	""ITEM-4"": {
		""id"": ""ITEM-4"",
		""author"": [
			{
				""family"": ""Razlogova"",
				""given"": ""Elena""
			}
		],
		""title"": ""Radio and Astonishment: The Emergence of Radio Sound, 1920-1926"",
		""type"": ""speech"",
		""event"": ""Society for Cinema Studies Annual Meeting"",
		""event-place"": ""Denver, CO"",
        ""note"":""All styles in the CSL repository are supported by the new processor, including the popular Chicago styles by Elena."",
		""issued"": {
			""date-parts"": [
				[
					2002,
					5
				]
			]
		}
	},
	""ITEM-5"": {
		""id"": ""ITEM-5"",
		""author"": [
			{
				""family"": ""??"",
				""given"": ""??"",
				""multi"":{
					""_key"":{
						""ja-alalc97"":{
								""family"": ""Kajita"",
								""given"": ""Shoji""
						}
					}
				}				
			},
			{
				""family"": ""??"",
				""given"": ""?"",
				""multi"":{
					""_key"":{
						""ja-alalc97"":{
							""family"": ""Kakusho"",
							""given"": ""Takashi""
						}
					}
				}				
			},
			{
				""family"": ""??"",
				""given"": ""??"",
				""multi"":{
					""_key"":{
						""ja-alalc97"":{
							""family"": ""Nakazawa"",
							""given"": ""Atsushi""
						}
					}
				}				
			},
			{
				""family"": ""??"",
				""given"": ""??"",
				""multi"":{
					""_key"":{
						""ja-alalc97"":{
							""family"": ""Takemura"",
							""given"": ""Haruo""
						}
					}
				}				
			},
			{
				""family"": ""??"",
				""given"": ""??"",
				""multi"":{
					""_key"":{
						""ja-alalc97"":{
							""family"": ""Mino"",
							""given"": ""Michihiko""
						}
					}
				}				
			},
			{
				""family"": ""??"",
				""given"": ""??"",
				""multi"":{
					""_key"":{
						""ja-alalc97"":{
							""family"": ""Mase"",
							""given"": ""Kenji""
						}
					}
				}				
			}
		],
		""title"": ""??????????????????????????????????"",
		""multi"":{
			""_keys"":{
				""title"":{
					""ja-alalc97"": ""K?t? ky?iku ni okeru jisedai ky?iku gakush? shien puratto f?mu no k?chiku ni mukete"",
					""en"": ""Toward the Development of Next-Generation Platforms for Teaching and Learning in Higher Education""
				},
				""container-title"":{
					""ja-alalc97"": ""Nihon ky?iku k?gaku ronbunshi"",
					""en"": ""Journal of the Japan Educational Engineering Society""
				}
			}
		},
		""container-title"": ""??????????"",
		""volume"": ""31"",
		""issue"": ""3"",
		""page"": ""297-305"",
		""issued"": {
			""date-parts"": [
				[
					2007,
					12
				]
			]
		},
        ""note"": ""Note the transformations to which this cite is subjected in the samples above, and the fact that it appears in the correct sort position in all rendered forms.  Selection of multi-lingual content can be configured in the style, permitting one database to serve a multi-lingual author in all languages in which she might publish."",
		""type"": ""article-journal""

	},
	""ITEM-6"": {
		""id"": ""ITEM-6"",
		""title"":""Evaluating Components of International Migration: Consistency of 2000 Nativity Data"",
		""note"": ""This cite illustrates the formatting of institutional authors.  Note that there is no \""and\"" between the individual author and the institution with which he is affiliated."",
		""author"": [
			{
				""family"": ""Malone"",
				""given"": ""Nolan J."",
				""static-ordering"": false
			},
			{
				""literal"": ""U.S. Bureau of the Census""
			}
		],
		""publisher"": ""Routledge"",
        ""publisher-place"": ""New York"",
		""issued"": {
			""date-parts"":[
				[2001, 12, 5]
			]
		},
		""type"": ""book""
	},
	""ITEM-7"": {
		""id"": ""ITEM-7"",
		""title"": ""True Crime Radio and Listener Disenchantment with Network Broadcasting, 1935-1946"",
		""author"":[
			{
				""family"": ""Razlogova"",
				""given"": ""Elena""
			}
		],
		""container-title"": ""American Quarterly"",
		""volume"": ""58"",
		""page"": ""137-158"",
		""issued"": {
			""date-parts"": [
				[2006, 3]
			]
		},
		""type"": ""article-journal""
	},
	""ITEM-8"": {
		""id"": ""ITEM-8"",
		""title"": ""The Guantanamobile Project"",
		""container-title"": ""Vectors"",
		""volume"": ""1"",
		""author"":[
			{
				""family"": ""Razlogova"",
				""given"": ""Elena""
			},
			{
				""family"": ""Lynch"",
				""given"": ""Lisa""
			}
		],
		""issued"": {
			""season"": 3,
			""date-parts"": [
				[2005]
			]
		},
		""type"": ""article-journal""

	},
	""ITEM-9"": {
		""id"": ""ITEM-9"",
		""container-title"": ""FEMS Yeast Research"",
		""volume"": ""9"",
		""issue"": ""8"",
		""page"": ""1123-1136"",
		""title"": ""Metabolic engineering of <i>Saccharomyces cerevisiae</i> for production of carboxylic acids: current status and challenges"",
		""contributor"":[
			{
				""family"": ""Zelle"",
				""given"": ""Rintze M.""
			}
		],
		""author"": [
			{
				""family"": ""Abbott"",
				""given"": ""Derek A.""
			},
			{
				""family"": ""Zelle"",
				""given"": ""Rintze M.""
			},
			{
				""family"":""Pronk"",
				""given"":""Jack T.""
			},
			{
				""family"": ""Maris"",
				""given"":""Antonius J.A."",
				""non-dropping-particle"":""van""
			}
		],
		""issued"": {
			""season"": ""2"",
			""date-parts"": [
				[
					2009,
					6,
					6
				]
			]
		},
		""type"": ""article-journal""
	},
    ""ITEM-10"": {
        ""container-title"": ""N.Y.2d"", 
        ""id"": ""ITEM-10"", 
        ""issued"": {
            ""date-parts"": [
                [
                    ""1989""
                ]
            ]
        }, 
        ""page"": ""683"", 
        ""title"": ""People v. Taylor"", 
        ""type"": ""legal_case"", 
        ""volume"": 73
    }, 
    ""ITEM-11"": {
        ""container-title"": ""N.E.2d"", 
        ""id"": ""ITEM-11"", 
        ""issued"": {
            ""date-parts"": [
                [
                    ""1989""
                ]
            ]
        }, 
        ""page"": ""386"", 
        ""title"": ""People v. Taylor"", 
        ""type"": ""legal_case"", 
        ""volume"": 541
    }, 
    ""ITEM-12"": {
        ""container-title"": ""N.Y.S.2d"", 
        ""id"": ""ITEM-12"", 
        ""issued"": {
            ""date-parts"": [
                [
                    ""1989""
                ]
            ]
        }, 
        ""page"": ""357"", 
        ""title"": ""People v. Taylor"", 
        ""type"": ""legal_case"", 
        ""volume"": 543
    },
    ""ITEM-13"": {
        ""id"": ""ITEM-13"", 
        ""title"": ""??"",
		""multi"":{
			""_keys"":{
				""title"": {
					""ja-alalc97"": ""Minp?"",
					""en"": ""Japanese Civil Code""
				}
			}
		},
        ""type"": ""legislation""
    },
    ""ITEM-14"": {
        ""id"": ""ITEM-14"", 
        ""title"": ""Clayton Act"",
        ""container-title"": ""ch."",
        ""number"": 323,
		""issued"": {
           ""date-parts"": [
             [
                1914
             ]
           ]
		},
        ""type"": ""legislation""
    },
    ""ITEM-15"": {
        ""id"": ""ITEM-15"", 
        ""title"": ""Clayton Act"",
		""volume"":38,
        ""container-title"": ""Stat."",
        ""page"": 730,
		""issued"": {
           ""date-parts"": [
             [
                1914
             ]
           ]
		},
        ""type"": ""legislation""
    },
    ""ITEM-16"": {
        ""id"": ""ITEM-16"", 
        ""title"": ""FTC Credit Practices Rule"",
		""volume"":16,
        ""container-title"": ""C.F.R."",
        ""section"": 444,
		""issued"": {
           ""date-parts"": [
             [
                1999
             ]
           ]
		},
        ""type"": ""legislation""
    },
    ""ITEM-17"": {
        ""id"": ""ITEM-17"", 
        ""title"": ""Beck v. Beck"",
		""volume"":1999,
        ""container-title"": ""ME"",
        ""page"": 110,
		""issued"": {
           ""date-parts"": [
             [
                1999
             ]
           ]
		},
        ""type"": ""legal_case""
    },
    ""ITEM-18"": {
        ""id"": ""ITEM-18"", 
        ""title"": ""Beck v. Beck"",
		""volume"":733,
        ""container-title"": ""A.2d"",
        ""page"": 981,
		""issued"": {
           ""date-parts"": [
             [
                1999
             ]
           ]
		},
        ""type"": ""legal_case""
    },
    ""ITEM-19"": {
        ""id"": ""ITEM-19"", 
        ""title"": ""Donoghue v. Stevenson"",
		""volume"":1932,
        ""container-title"": ""App. Cas."",
        ""page"": 562,
		""issued"": {
           ""date-parts"": [
             [
                1932
             ]
           ]
		},
        ""type"": ""legal_case""
    },
    ""ITEM-20"": {
        ""id"": ""ITEM-20"", 
        ""title"": ""British Columbia Elec. Ry. v. Loach"",
		""volume"":1916,
		""issue"":1,
        ""container-title"": ""App. Cas."",
        ""page"": 719,
		""authority"":""P.C."",
		""issued"": {
           ""date-parts"": [
             [
                1915
             ]
           ]
		},
        ""type"": ""legal_case""
    },
    ""ITEM-21"": {
        ""id"": ""ITEM-21"", 
        ""title"": ""Chapters on Chaucer"",
		""author"":[
			{
				""family"": ""Malone"",
				""given"": ""Kemp""
			}
		],
        ""publisher"":""Johns Hopkins Press"",
        ""publisher-place"": ""Baltimore"",
		""issued"": {
           ""date-parts"": [
             [
                1951
             ]
           ]
		},
        ""type"": ""book""
    }
}";
		}

		[Test]
		public void TestSpeedForEntireDatabase()
		{
			
		}
	}
}