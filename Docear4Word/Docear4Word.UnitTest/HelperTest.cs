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
using NUnit.Framework;
using Docear4Word;

namespace docear4word.UnitTest
{
    [TestFixture]
    public class HelperTest
    { 
        /// <summary>
        /// [Allen] Test the dialog window for showing messages. 
        /// </summary>
        [Test]
        public void TestDialog()
        {
            Helper.ShowCorruptBibtexDatabaseMessage("ABC");
            bool result = true;
            Assert.IsTrue(result);
        }
    }
}
