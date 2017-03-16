using Schematron.Insight.Properties;
using Schematron.Insight.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Schematron.Insight.Utilities
{
    public class HtmlReporter
    {
        #region Private Properties
        private XmlDocument _doc = null;
        private XmlNode _head = null;
        private XmlNode _body = null;
        #endregion
        #region Public Properties
        public string Title
        {
            get
            {
                XmlNode node = _head.SelectSingleNode("//title");
                return node != null ? node.InnerText : "";
            }
            set
            {
                XmlNode node;
                node = _head?.SelectSingleNode("//title");
                if (node != null)
                {
                    node.InnerText = value;
                }
                node = _body.SelectSingleNode("//h1");
                if (node != null)
                {
                    node.InnerText = value;
                }
            }
        }
        public string ApplicationInfo
        {
            get
            {
                XmlNode node = _body.SelectSingleNode("//div[@class='appinfo']");
                return node != null ? node.InnerText : "";
            }
            set
            {
                XmlNode node = _body.SelectSingleNode("//div[@class='appinfo']");
                if (node != null)
                {
                    node.InnerText = value;
                }
            }
        }
        public string ModifiedInfo
        {
            get
            {
                XmlNode node = _body.SelectSingleNode("//div[@class='dateinfo']");
                return node != null ? node.InnerText : "";
            }
            set
            {
                XmlNode node = _body.SelectSingleNode("//div[@class='dateinfo']");
                if (node != null)
                {
                    node.InnerText = value;
                }
            }
        }
        public string TotalSyntaxError
        {
            get
            {
                XmlNode node = _body.SelectSingleNode("//div[contains(@class,'result-error')]");
                return node != null ? node.ParentNode.InnerText : "";
            }
            set
            {
                XmlNode node = _body.SelectSingleNode("//div[contains(@class,'result-error')]");
                if (node != null)
                {
                    node.ParentNode.AppendChild(_doc.CreateTextNode(value));
                }
            }
        }
        public string TotalAssert
        {
            get
            {
                XmlNode node = _body.SelectSingleNode("//div[contains(@class,'result-assert')]");
                return node != null ? node.ParentNode.InnerText : "";
            }
            set
            {
                XmlNode node = _body.SelectSingleNode("//div[contains(@class,'result-assert')]");
                if (node != null)
                {
                    node.ParentNode.AppendChild(_doc.CreateTextNode(value));
                }
            }
        }
        public string TotalReport
        {
            get
            {
                XmlNode node = _body.SelectSingleNode("//div[contains(@class,'result-report')]");
                return node != null ? node.ParentNode.InnerText : "";
            }
            set
            {
                XmlNode node = _body.SelectSingleNode("//div[contains(@class,'result-report')]");
                if (node != null)
                {
                    node.ParentNode.AppendChild(_doc.CreateTextNode(value));
                }
            }
        }
        #endregion
        #region Constructor
        public HtmlReporter()
        {
            try
            {
                string xml = Resources.HtmlReportTemplate;
                _doc = new XmlDocument();
                _doc.LoadXml(xml);
                _head = _doc.SelectSingleNode("//head");
                _body = _doc.SelectSingleNode("//body");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Public Method
        public void WriteReport(string xmlfile, List<Result> results)
        {
            XmlAttribute attr;
            try
            {
                XmlNode container = _body.SelectSingleNode("//div[@class='result-container']");
                if (container == null)
                    throw new Exception("'result-container' is undefined.");

                XmlNode result = _doc.CreateElement("div");
                attr = _doc.CreateAttribute("class");
                attr.Value = "result";
                result.Attributes.Append(attr);
                XmlNode filename = _doc.CreateElement("div");
                attr = _doc.CreateAttribute("class");
                attr.Value = "filename";
                filename.Attributes.Append(attr);
                filename.InnerText = Path.GetFileName(xmlfile);
                XmlNode table = _doc.CreateElement("table");

                string[] lines = File.ReadAllLines(xmlfile, Encoding.UTF8);

                WriteResult(table, lines, results);

                result.AppendChild(filename);
                result.AppendChild(table);
                container.AppendChild(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region Private Methods
        private void WriteResult(XmlNode table, string[] lines, List<Result> results)
        {
            XmlNode tr, td;
            XmlAttribute attr;
            try
            {
                int index = 0;
                foreach (string line in lines)
                {
                    tr = _doc.CreateElement("tr");
                    // index
                    td = _doc.CreateElement("td");
                    td.AppendChild(_doc.CreateTextNode((++index).ToString()));
                    tr.AppendChild(td);
                    // source code
                    td = _doc.CreateElement("td");
                    XmlNode pre = _doc.CreateElement("pre");
                    XmlNode code = _doc.CreateElement("code");
                    attr = _doc.CreateAttribute("class");
                    attr.Value = "html";
                    code.Attributes.Append(attr);
                    code.AppendChild(_doc.CreateTextNode(line));
                    pre.AppendChild(code);
                    td.AppendChild(pre);
                    tr.AppendChild(td);
                    table.AppendChild(tr);

                    List<Result> finds = results.FindAll((item) => { return item.Line == index; });
                    if (finds.Count > 0)
                    {
                        finds.Sort((a, b) => { return a.Pos.CompareTo(b.Pos); });
                        foreach (Result result in finds)
                        {
                            SetReport(table, result);
                            results.Remove(result);
                        }
                    }
                }
                if (results.Count > 0)
                {
                    foreach (Result result in results)
                    {
                        SetReport(table, result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetReport(XmlNode table, Result result)
        {
            XmlNode tr, td;
            XmlAttribute attr = GetReportAttribute(result);

            tr = _doc.CreateElement("tr");
            // index(empty)
            td = _doc.CreateElement("td");
            td.InnerText = "!";
            if (attr != null)
            {
                attr.Value += " notice";
                td.Attributes.Append(attr);
            }

            tr.AppendChild(td);
            // result reports
            td = _doc.CreateElement("td");

            attr = GetReportAttribute(result);
            if (attr != null)
            {
                td.Attributes.Append(attr);
            }
            string info = GetResultInfo(result);

            td.AppendChild(_doc.CreateTextNode(result.Message));
            td.AppendChild(_doc.CreateElement("br"));
            td.AppendChild(_doc.CreateTextNode(info));

            tr.AppendChild(td);
            table.AppendChild(tr);
        }
        private XmlAttribute GetReportAttribute(Result result)
        {
            string value = "";
            switch (result.Status)
            {
                case ResultStatus.SyntaxError:
                    value = "result-error";
                    break;
                case ResultStatus.Assert:
                    value = "result-assert";
                    break;
                case ResultStatus.Report:
                    value = "result-report";
                    break;
            }
            if (value != "")
            {
                XmlAttribute attr = _doc.CreateAttribute("class");
                attr.Value = value;
                return attr;
            }
            return null;
        }
        private string GetResultInfo(Result result)
        {

            string info = "";
            // create info
            {
                List<string> infos = new List<string>();
                if (result.Status != ResultStatus.None)
                    infos.Add($"Status By:{result.Status}");
                if (result.Line >= 0)
                    infos.Add($"Line:{result.Line}");
                if (result.Pos >= 0)
                    infos.Add($"Pos:{result.Pos}");
                if (result.Role.IsEnabled)
                    infos.Add($"Role:{result.Role}");
                info = $"　（{String.Join(", ", infos)}）";

            }
            return info;
        }
        public override string ToString()
        {
            string str = _doc?.OuterXml ?? "";
            str = Regex.Replace(str, @"<\?xml.*?\?>\r\n", "");
            str = ReplaceIndent(str);
            return str;
        }
        private string ReplaceIndent(string str)
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
