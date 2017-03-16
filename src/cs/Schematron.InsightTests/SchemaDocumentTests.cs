﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schematron.Insight;
using Schematron.Insight.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Schematron.Insight.Tests
{
    [TestClass()]
    public class SchemaDocumentTests
    {
        [TestMethod()]
        public void SchemaDocumentTest()
        {
            string dir = @"../../../../../testdata/";
            List<string> tests = new List<string>()
            {
                "mimetype", "cultures", "calendar-2017"
            };

            SchemaDocument doc = null;
            Reporter report = new Reporter();
            Func<string, string> replacer = new Func<string, string>((str) =>
            {
                return Regex.Replace(str, "<div class=\"dateinfo\">(.+?)</div>", "");
            });
            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                PrintWorkingSet($"start of processing: ({DateTime.Now.ToShortTimeString()})");

                foreach (string test in tests)
                {
                    PrintWorkingSet($"pre processing of {test}");

                    string testsch = Path.Combine(dir, $"{test}.sch");
                    string testxml = Path.Combine(dir, $"{test}.xml");
                    string dstfile = Path.Combine(dir, $"{test}-result.html");
                    if (!File.Exists(testsch))
                        throw new FileNotFoundException();
                    if (!File.Exists(testxml))
                        throw new FileNotFoundException();

                    doc = new SchemaDocument();

                    doc.Open(testsch);
                    doc.Compile(Phase.ALL);

                    report.Results = doc.Validation(testxml);
                    report.Format = ExportFormats.Html;
#if false
                    report.Write(dstfile);
#else
                    // test result
                    string result = report.ToString(ExportFormats.Html);
                    // trusted result
                    string expected = File.ReadAllText(dstfile, Encoding.UTF8);

                    result = replacer(result);
                    expected = replacer(expected);

                    Assert.AreEqual(result, expected, $"{test} not equals.");
#endif

                    doc?.Dispose();

                    PrintWorkingSet($"post processing of {test}");
                }

                sw.Stop();
                PrintWorkingSet($"end of processing: ({DateTime.Now.ToShortTimeString()}, {sw.ElapsedMilliseconds} ms)");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
            }
        }
        private void PrintWorkingSet(string suffix)
        {
            long n = Environment.WorkingSet;
            int index = 0;
            string[] units = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            while (n>1024 && index + 1 < units.Length)
            {
                n = n / 1024;
                index++;
            }
            Console.WriteLine(">> {0} {1}\t{2}", n, units[index], suffix);
        }
    }
}