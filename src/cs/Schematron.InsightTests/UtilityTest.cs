using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Schematron.InsightTests
{
    [TestClass()]
    public class UtilityTest
    {
        #region FindListTest
        [TestMethod()]
        public void FindListTest()
        {
            List<string> items = GenerateList();
            long time_findall = 0;
            long time_where = 0;

            for(int i = 0; i < 35; i++)
            {
                string name = $"group-{i}";
                if(i % 2 == 0)
                {
                    time_findall += FindAll(items, name);
                    time_where += FindWhere(items, name);
                }
                else
                {
                    time_where += FindWhere(items, name);
                    time_findall += FindAll(items, name);
                }
            }
            Debug.Print("average");
            Debug.Print("time_findall:{0}, time_where:{1}", time_findall/35, time_where/35);

        }
        private List<string> GenerateList()
        {
            List<string> items = new List<string>();
            Random random = new Random(Environment.TickCount);
            for (int i = 0; i <= 999999; i++)
            {
                int n = random.Next(0, 30);
                items.Add($"group-{n}");
            }
            return items;
        }

        private long FindAll(List<string> items, string name)
        {
            Stopwatch sw;
            StringBuilder sb;

            sw = Stopwatch.StartNew();
            sb = new StringBuilder();
            List<string> results = items.FindAll((i) => i == name);
            foreach (string result in results)
            {
                sb.Append(result);
            }
            sw.Stop();
            Debug.Print("FindAll({0}): {1} ms", results.Count, sw.ElapsedMilliseconds);
            return sw.ElapsedMilliseconds;
        }
        private long FindWhere(List<string> items, string name)
        {
            Stopwatch sw;
            StringBuilder sb;

            sw = Stopwatch.StartNew();
            sb = new StringBuilder();
            IEnumerable < string > results = items.Where<string>((i) => i == name);
            foreach (string result in results)
            {
                sb.Append(result);
            }
            sw.Stop();
            Debug.Print("Where({0}): {1} ms", results.Count(), sw.ElapsedMilliseconds);
            return sw.ElapsedMilliseconds;
        }
        #endregion
        #region String
        [TestMethod()]
        public void ReplaceIndentTest()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < 30; i++)
            {
                if(i % 13 != 0)
                {
                    sb.AppendFormat("<code index='{0}'>{1}</code>", i, new String(' ', i));
                }
                else
                {
                    sb.AppendFormat("<p index='{0}'>{1}</p>", i, new String(' ', i));
                }
            }
            string str = sb.ToString();
            string res1;
            string res2;
            string res3;
            Stopwatch sw;
            {
                sw = Stopwatch.StartNew();
                res1 = ReplaceIndent1(str);
                sw.Stop();
                Debug.Print("ReplaceIndent1: {0} ms", sw.ElapsedMilliseconds);
            }
            {
                sw = Stopwatch.StartNew();
                res2 = ReplaceIndent2(str);
                sw.Stop();
                Debug.Print("ReplaceIndent2: {0} ms", sw.ElapsedMilliseconds);
            }
            {
                sw = Stopwatch.StartNew();
                res3 = ReplaceIndent3(str);
                sw.Stop();
                Debug.Print("ReplaceIndent3: {0} ms", sw.ElapsedMilliseconds);
            }
            Assert.AreEqual(res1, res2, "res1 != res2");
            Assert.AreEqual(res1, res3, "res1 != res3");
            Assert.AreEqual(res2, res3, "res2 != res3");

        }


        private string ReplaceIndent1(string str)
        {
            int limit = int.MaxValue;
            int count = 0;
            while (Regex.IsMatch(str, "(<code.*?>[\t]*)[ ]{2}"))
            {
                str = Regex.Replace(str, "(<code.*?>[\t]*)[ ]{2}", "$1\t");
                if (++count > limit)
                    break;
            }
            return str;
        }
        private string ReplaceIndent2(string str)
        {
            Regex reg = new Regex("(<code.*?>[\t]*)[ ]{2}");
            int limit = int.MaxValue;
            int count = 0;
            while (reg.IsMatch(str))
            {
                str = reg.Replace(str, "$1\t");
                if (++count > limit)
                    break;
            }
            return str;
        }
        private string ReplaceIndent3(string str)
        {
            return Regex.Replace(str, "(<code.*?>)([ ]+)", (m) =>
            {
                string code = m.Groups[1].Value;
                string space = m.Groups[2].Value;
                return code + space.Replace(" ", "&nbsp;");
            });
        }
        #endregion



    }
}
