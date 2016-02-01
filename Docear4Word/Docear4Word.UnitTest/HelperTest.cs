/*
 * Program Description: Test the Helper's method.
 * Author: Jyun-Yao Huang (Allen), allen501pc@gmail.com
 * Creation Date: 2016/01/17
 * Modification Logs:
 * 1. [Allen] 2016/01/17 Add a simple test for the method of  showing the dialog "CorruptBibtexDatabase".
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Docear4Word;
using Docear4Word.BibTex;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace docear4word.UnitTest
{
    [TestClass]
    public class HelperTest
    { 
        /// <summary>
        /// [Allen] Test the dialog window for showing messages. 
        /// </summary>
        [TestMethod]
        public void TestDialog()
        {
            Helper.ShowCorruptBibtexDatabaseMessage("ABC");
            bool result = true;
            Assert.IsTrue(result);
        }

        /// <summary>
        /// [Alen] Test the bibtex parser.
        /// </summary>
        [TestMethod]
        public void TestBibtexParser()
        {
            string documentDatabaseFilename = "E:\\D4W_Test\\_referenceDocear.bib";
            BibTexDatabase result = BibTexHelper.LoadBibTexDatabase(documentDatabaseFilename);
            bool resultA = false;
            Assert.IsTrue(resultA);
        }
    }
}
