using Schematron.Insight.Processors;
using Schematron.Insight.Utilities;
using Schematron.Insight.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Schematron.Insight
{
    public class SchemaDocument : IDisposable
    {
        #region Private Properties
        private string _workdir = "";

        ProcessorFactory _processor = null;
        public FileInfo SchemaSrc = null;
        public FileInfo SchemaTmp = null;
        #endregion
        #region Public Properties
        public List<Phase> Phases { get; private set; } = new List<Phase>();
        public bool IsDebug { get; set; } = false;
        #endregion
        #region Constructors
        public SchemaDocument()
        {
            try
            {
                _workdir = Path.Combine(LibraryInfo.WorkingDirectory, "temp");
                if (!Directory.Exists(_workdir))
                    Directory.CreateDirectory(_workdir);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Private Methods
        private string GetLocalName(XElement e)
        {
            return e.Name.LocalName;
        }
        private string GetLocalName(XAttribute e)
        {
            return e.Name.LocalName;
        }
        private string GetName(XElement e)
        {
            string prefix = e.GetPrefixOfNamespace(e.Name.Namespace);
            return (prefix != null ? $"{prefix}:" : "") + e.Name.LocalName;
        }
        private string GetName(XAttribute e)
        {
            string prefix = e.Parent.GetPrefixOfNamespace(e.Name.Namespace);
            return (prefix != null ? $"{prefix}:" : "") + e.Name.LocalName;
        }
        private void Preprocessor(FileInfo schema, string phase)
        {
            FileStream fs = null;
            XDocument xdoc = null;
            try
            {
                fs = schema.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Position = 0;

                xdoc = XDocument.Load(fs, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);

                // bad schematron 1.5
                // http://www.ascc.net/xml/schematron
                // iso schematron is good.
                // http://purl.oclc.org/dsdl/schematron
                if (xdoc.Root.Name.Namespace != "http://purl.oclc.org/dsdl/schematron")
                {
                    throw new Exception($"bad namespace uri ('{xdoc.Root.Name.Namespace}'). iso schematron('http://purl.oclc.org/dsdl/schematron') is good.");
                }
                // select phase 
                if (!String.IsNullOrWhiteSpace(phase) && phase != Phase.ALL && phase != Phase.DEFAULT)
                {
                    xdoc.Root.SetAttributeValue("defaultPhase", phase);
                }
                // to absolute of external document 
                IEnumerable<XElement> finds;
                finds = xdoc.XPathSelectElements("//*[@test]");
                foreach (XElement find in finds)
                {
                    string value = find.Attribute("test").Value;
                    find.Attribute("test").Value = ToAbsoluteFilePath(value);
                }
                finds = xdoc.XPathSelectElements("//*[local-name()='let'][@value]");
                foreach (XElement find in finds)
                {
                    string value = find.Attribute("value").Value;
                    find.Attribute("value").Value = ToAbsoluteFilePath(value);
                }
                finds = xdoc.XPathSelectElements("//*[local-name()='value-of'][@select]");
                foreach (XElement find in finds)
                {
                    string value = find.Attribute("select").Value;
                    find.Attribute("select").Value = ToAbsoluteFilePath(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs?.Close();
                fs?.Dispose();
            }
            if (xdoc != null)
            {
                try
                {
                    xdoc.Save(schema.FullName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private string ToAbsoluteFilePath(string value)
        {
            return Regex.Replace(value, "document\\(['\"](.*)['\"]\\)", (mc) =>
            {
                string docpath = mc.Groups[1].Value;
                string absolute = Path.Combine(SchemaSrc.DirectoryName, docpath);
                absolute = Path.GetFullPath(absolute);
                absolute = absolute.Replace("\\", "/");
                absolute = absolute.Replace("'", "\'");
                return $"document('file:///{absolute}')";
            });
        }
        private bool GetLineNumber(XDocument xdoc, string location, out IXmlLineInfo info)
        {
            info = null;
            try
            {
                XElement find = xdoc.XPathSelectElement(location);
                info = find as IXmlLineInfo;
            }
            catch { }
            return info?.HasLineInfo() ?? false;
        }
        private bool GetLineNumberBySaxon(ProcessorEvaluation evaluation, string location, out IXmlLineInfo info)
        {
            info = null;
            try
            {
                XmlNode node = evaluation.SelectNode(location);
                if (node == null)
                    throw new Exception();
                string findAt = FindAtPath(node.CreateNavigator());
                XElement find = evaluation.XPathSelectElement(findAt);
                info = find as IXmlLineInfo;
            }
            catch { }
            return info?.HasLineInfo() ?? false;
        }
        public static string FindAtPath(XPathNavigator target)
        {
            List<string> paths = new List<string>();
            XPathNavigator nav = target.Clone();
            do
            {
                string path = "";
                switch (nav.NodeType)
                {
                    case XPathNodeType.Attribute:
                        path = $"@{nav.Name}";
                        break;
                    case XPathNodeType.Comment:
                        path = "comment()";
                        break;
                    case XPathNodeType.Element:
                        path = nav.Name;
                        break;
                    case XPathNodeType.Text:
                        path = "text()";
                        break;
                }
                switch (nav.NodeType)
                {
                    case XPathNodeType.Comment:
                    case XPathNodeType.Element:
                    case XPathNodeType.Text:
                        int index = FindAtPathIndex(nav);
                        if (index > 0)
                        {
                            path += $"[{index}]";
                        }
                        break;
                }
                if (!String.IsNullOrEmpty(path))
                    paths.Insert(0, path);

            }
            while (nav.MoveToParent());

            return paths.Count > 0 ? $"/{String.Join("/", paths)}" : "";
        }
        private static int FindAtPathIndex(XPathNavigator nav)
        {
            int index = 0;
            XPathNavigator buf = nav.Clone();
            if (buf.MoveToParent())
            {
                if (buf.MoveToChild(nav.NodeType))
                {
                    do
                    {
                        if (buf.LocalName == nav.LocalName)
                        {
                            index++;
                        }
                        XmlNodeOrder cmp = buf.ComparePosition(nav);
                        if (cmp == XmlNodeOrder.Same)
                            break;
                    }
                    while (buf.MoveToNext(nav.NodeType));
                }
            }
            return index;
        }
        private ResultCollection CheckWellFormat(string xmlfile)
        {
            ResultCollection results = new ResultCollection();
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Parse,
                ValidationType = ValidationType.DTD
            };
            settings.ValidationEventHandler += (sender, e) =>
            {
                XmlSchemaException ex = e.Exception;
                Result result = new Result()
                {
                    SchFile = SchemaSrc,
                    XmlFile = new FileInfo(xmlfile),
                    Status = ResultStatus.SyntaxError,
                    Message = e.Message,
                    Line = ex?.LineNumber ?? -1,
                    Pos = ex?.LinePosition ?? -1,
                };
                results.Add(result);
            };
            try
            {
                XmlReader reader = XmlReader.Create(xmlfile, new XmlReaderSettings());
                while (reader.Read()) ;
            }
            catch (XmlException ex)
            {
                results.Add(new Result()
                {
                    SchFile = SchemaSrc,
                    XmlFile = new FileInfo(xmlfile),
                    Status = ResultStatus.SyntaxError,
                    Message = ex.Message,
                    Line = ex.LineNumber,
                    Pos = ex.LinePosition,
                });
            }
            catch (Exception ex)
            {
                results.Add(new Result()
                {
                    SchFile = SchemaSrc,
                    XmlFile = new FileInfo(xmlfile),
                    Status = ResultStatus.SyntaxError,
                    Message = ex.Message
                });
            }
            return results;
        }
        #endregion
        #region Public Methods
        public void Init()
        {
            try
            {
                _processor = new ProcessorFactory();
                _processor.InitializeCompiler();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Open(string schemafile)
        {
            Phases.Clear();
            FileStream fs = null;
            try
            {
                SchemaSrc = new FileInfo(schemafile);
                string tmppath = Path.Combine(_workdir, SchemaSrc.Name);
                SchemaSrc.CopyTo(tmppath, true);

                SchemaTmp = new FileInfo(tmppath);
                if (!SchemaTmp.Exists)
                    throw new FileNotFoundException(SchemaTmp.FullName);

                fs = SchemaTmp.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Position = 0;

                XDocument xdoc = XDocument.Load(fs, LoadOptions.None);

                foreach (XElement e1 in xdoc.Descendants().Where(p => p.Name.LocalName == "phase"))
                {
                    XAttribute id = e1.Attribute("id");

                    Phase phase = new Phase();
                    phase.Id = id?.Value ?? "";

                    foreach (XElement e2 in e1.Descendants().Where(p => p.Name.LocalName == "active"))
                    {
                        XAttribute pattern = e2.Attribute("pattern");
                        if (pattern != null)
                        {
                            phase.Patterns.Add(pattern.Value);
                        }
                    }
                    if (phase.IsEnabled)
                    {
                        Phases.Add(phase);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs?.Dispose();
            }
        }
        public void Compile(string phase = "")
        {
            try
            {
                if (_processor == null)
                {
                    Init();
                }
                Preprocessor(SchemaTmp, phase);
                _processor.Compile(SchemaTmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ResultCollection Validation(string xmlfile)
        {
            ResultCollection results = new ResultCollection();
            XmlDocument doc = null;
            XDocument xdoc = null;
            ProcessorEvaluation evaluation = null;
            try
            {
                xdoc = XDocument.Load(xmlfile, LoadOptions.SetLineInfo);

                doc = _processor.Validation(xmlfile);

                if (IsDebug)
                {
                    File.WriteAllText(Path.ChangeExtension(xmlfile, ".report.xml"), doc.OuterXml, Encoding.UTF8);
                }
                XmlNodeList nodes = doc.SelectNodes("//*[local-name()='failed-assert' or local-name()='successful-report']");
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        Result result = new Result();
                        result.SchFile = SchemaSrc;
                        result.XmlFile = new FileInfo(xmlfile);

                        result.Status = node.LocalName == "failed-assert" ? ResultStatus.Assert : ResultStatus.Report;
                        result.Test = node.Attributes["test"]?.Value ?? "";
                        result.Location = node.Attributes["location"]?.Value ?? "";
                        result.Role.Name = node.Attributes["role"]?.Value ?? "";

                        IXmlLineInfo lineinfo = null;
                        if (!GetLineNumber(xdoc, result.Location, out lineinfo))
                        {
                            if (evaluation == null)
                            {
                                evaluation = new ProcessorEvaluation(xdoc);
                                evaluation.Compile(_processor.Processor);
                            }
                            GetLineNumberBySaxon(evaluation, result.Location, out lineinfo);
                        }
                        if (lineinfo?.HasLineInfo() ?? false)
                        {
                            result.Line = lineinfo.LineNumber;
                            result.Pos = lineinfo.LinePosition;
                        }

                        StringBuilder sb = new StringBuilder();
                        XmlNodeList texts = node.SelectNodes(".//*[local-name()='text']");
                        if (texts != null)
                        {
                            foreach (XmlNode text in texts)
                            {
                                sb.AppendLine(text.InnerXml);
                            }
                        }
                        result.Message = sb.Length > 0 ? sb.ToString() : node.InnerXml;

                        results.Add(result);
                    }
                }
            }
            catch (XmlException ex)
            {
                results.Add(new Result()
                {
                    SchFile = SchemaSrc,
                    XmlFile = new FileInfo(xmlfile),
                    Status = ResultStatus.SyntaxError,
                    Message = ex.Message,
                    Line = ex.LineNumber,
                    Pos = ex.LinePosition,
                });
            }
            catch (Exception ex)
            {
                results.Add(new Result()
                {
                    SchFile = SchemaSrc,
                    XmlFile = new FileInfo(xmlfile),
                    Status = ResultStatus.SyntaxError,
                    Message = ex.Message
                });
            }
            return results;
        }



        public void Close()
        {
            Phases.Clear();
            SchemaSrc = null;
            SchemaTmp = null;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Close();
            _processor?.Dispose();
            _processor = null;
            if (Directory.Exists(_workdir))
            {
                try
                {
                    FileSystemHelper.DeleteDirectory(_workdir);
                }
                catch (Exception ex)
                {
                    Debug.Print("Dispose Exception:" + ex.Message);
                }
            }
        }
        #endregion
    }
}
