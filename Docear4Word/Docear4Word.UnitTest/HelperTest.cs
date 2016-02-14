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
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;


namespace docear4word.UnitTest
{
    [TestClass]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class HelperTest
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [ComVisible(true)]
        public class MyRunner : JavaScriptRunner
        {
            public object message = null;
            public object Test()
            {
                try
                {
                    /*
                    wb.DocumentText = (File.ReadAllText(@"C:\Users\Allen\Docear4Word\Docear4Word\Docear4Word\JavaScript\all.html", Encoding.UTF8));
                    wb.ObjectForScripting = this;
                    while (wb.ReadyState != WebBrowserReadyState.Complete || wb.IsBusy)
                    {
                        Application.DoEvents();
                    }
                     * */
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //message = wb.Document.InvokeScript("abc");;
                // return message.ToString();
                return new object();
            }

            public void errorHandler(string url, string message, string line) {
                Console.WriteLine("url:" + url + System.Environment.NewLine +
                                  "message:" + message + System.Environment.NewLine +
                                  "line:" + line);
            }
        }
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

        public BibTexDatabase GetDatabaseTest()
        {
            string documentDatabaseFilename = "E:\\D4W_Test\\XML.bib";
            BibTexDatabase result = BibTexHelper.LoadBibTexDatabase(documentDatabaseFilename);
            return result;
        }

        /// <summary>
        /// [Alen] Test the bibtex parser.
        /// </summary>
        [TestMethod]
        public void TestBibtexParser()
        {
            MyRunner runner = new MyRunner();
            /*
            string jsonScript = File.ReadAllText(@"C:\Users\Allen\Docear4Word\Docear4Word\Docear4Word\JavaScript\JSON.js");
            string xmlDomScript = File.ReadAllText(@"C:\Users\Allen\Docear4Word\Docear4Word\Docear4Word\JavaScript\xmldom.js");
            // string citeProcScript = File.ReadAllText(@"C:\Users\Allen\Docear4Word\Docear4Word\Docear4Word\JavaScript\citeproc.js");
            string citeProcScript = File.ReadAllText(@"C:\Users\Allen\Docear4Word\Docear4Word\Backup\citeproc.js");
            string sysScript = File.ReadAllText(@"C:\Users\Allen\Docear4Word\Docear4Word\Docear4Word\JavaScript\Sys.js");

            string MergedScript = MyRunner.BuildScript(jsonScript, xmlDomScript, citeProcScript, sysScript);

            runner.SetupJavascriptInDoc(MergedScript);

            object obj = runner.Call("getCSLProcessorVersion");
            object obj2 = runner.wb.Document.InvokeScript("HelloTest");
            object msg;
            try
            {
                runner.Test();

                msg = runner.wb.Document.InvokeScript("HelloTest");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                bool a = true;
            }*/
            //File.WriteAllText(@"C:\Users\Allen\Docear4Word\Docear4Word\Docear4Word\JavaScript\all.js", MergedScript,new System.Text.UTF8Encoding(false));
            bool resultA = false;
            Assert.IsTrue(resultA);
        }

        [TestMethod]
        public void TestPageRangeParser()
        {
            string pagesDisplay = "4:1–4:39";
            string itemNumberOfPages = "39";
            var parser = new PageRangeParser(pagesDisplay, itemNumberOfPages);
            bool resultA = false;
            Assert.IsTrue(resultA);
        }
    }
}
