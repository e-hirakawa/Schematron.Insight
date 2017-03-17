using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using Schematron.Insight.Utilities;

namespace Schematron.Insight.Validation
{
    [Serializable()]
    [XmlRoot(ElementName = "Report")]
    [DataContract(Name = "Report")]
    public class Reporter
    {
        #region ReadOnly Field
        private readonly static DataContractJsonSerializer jsonSerializer =
            new DataContractJsonSerializer(typeof(Reporter));
        private readonly static XmlSerializer xmlSerializer =
            new XmlSerializer(typeof(Reporter));
        private readonly static Encoding UTF8 = Encoding.UTF8;
        #endregion
        #region Private Properties
        private ResultCollection _results = new ResultCollection();
        #endregion
        #region Public Properties
        [XmlIgnore]
        [IgnoreDataMember]
        public ExportFormats Format { get; set; } = ExportFormats.Log;

        [XmlElement(ElementName = "ReportBy", Order = 1)]
        [DataMember(Name = "ReportBy", Order = 1)]
        public string ReportBy { get; set; }
        [XmlElement(ElementName = "WrittenTo", Order = 2)]
        [DataMember(Name = "WrittenTo", Order = 2)]
        public string WrittenTo { get; set; }

        [XmlElement(ElementName = "TotalSyntaxError", Order = 3)]
        [DataMember(Name = "TotalSyntaxError", Order = 3)]
        public int TotalSyntaxError { get; set; }
        [XmlElement(ElementName = "TotalAssert", Order = 4)]
        [DataMember(Name = "TotalAssert", Order = 4)]
        public int TotalAssert { get; set; }
        [XmlElement(ElementName = "TotalReport", Order = 5)]
        [DataMember(Name = "TotalReport", Order = 5)]
        public int TotalReport { get; set; }

        [XmlArray(ElementName = "Results", Order = 6)]
        [XmlArrayItem(ElementName = "Result", Type = typeof(Result))]
        [DataMember(Name = "Results", Order = 6)]
        public ResultCollection Results
        {
            get { return _results; }
            set
            {
                _results = value;

                TotalSyntaxError = _results.TotalSyntaxError;
                TotalAssert = _results.TotalAssert;
                TotalReport = _results.TotalReport;
            }
        }
        #endregion

        public Reporter() { 
            ReportBy = $"{LibraryInfo.Name} -v.{LibraryInfo.Version}-";
            WrittenTo = $"{DateTime.Now:yyyy.MM.dd HH:mm:ss}";
        }
        #region Export Methods
        #region ToStream
        public Stream ToStream(ExportFormats format)
        {
            MemoryStream stream = null;
            StreamWriter writer = null;
            string str = "";
            try
            {
                str = ToString(format);
                stream = new MemoryStream();
                writer = new StreamWriter(stream);
                writer.Write(str);
                writer.Flush();
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer?.Close();
                writer?.Dispose();
            }
            return stream;
        }
        #endregion
        #region Serializer
        public string ToString(ExportFormats format)
        {
            string str = "";
            try
            {
                if (format == ExportFormats.Log)
                    str = ToLog();
                else if (format == ExportFormats.Tab)
                    str = ToTab();
                else if (format == ExportFormats.Xml)
                    str = ToXml();
                else if (format == ExportFormats.Json)
                    str = ToJson();
                else if (format == ExportFormats.Html)
                    str = ToHtml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        private string ToLog()
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            string sepalator = Environment.NewLine + new string('-', 20) + Environment.NewLine;
            sb.AppendLine(sepalator);
            sb.AppendLine("【検証結果】");
            sb.AppendLine($"Report By: {ReportBy}");
            sb.AppendLine($"Written To: {WrittenTo}");
            sb.AppendLine();
            sb.AppendLine($"Syntax Error: {TotalSyntaxError,4}");
            sb.AppendLine($"Total Assert: {TotalAssert,4}");
            sb.AppendLine($"Total Report: {TotalReport,4}");
            sb.AppendLine(sepalator);
            foreach (Result result in Results)
            {
                Type t = result.GetType();
                PropertyInfo[] infos = t.GetProperties();

                List<ExportData> datas = new List<ExportData>();
                foreach (PropertyInfo info in infos)
                {
                    IgnoreDataMemberAttribute ignore = info.GetCustomAttribute(typeof(IgnoreDataMemberAttribute), false) as IgnoreDataMemberAttribute;
                    if (ignore != null)
                        continue;
                    DataMemberAttribute datamember = info.GetCustomAttribute(typeof(DataMemberAttribute), false) as DataMemberAttribute;
                    if (datamember == null)
                        continue;

                    string name = !String.IsNullOrEmpty(datamember.Name) ? datamember.Name : info.Name;
                    string value = info.GetValue(result).ToString();
                    value = value.Trim(new[] { ' ', '\r', '\n', '\t' });
                    value = Regex.Replace(value, $"[{Environment.NewLine}]+", $"{Environment.NewLine}\t\t");

                    datas.Add(new ExportData()
                    {
                        Index = datamember.Order,
                        Name = name,
                        Value = (!String.IsNullOrWhiteSpace(value)) ? value : "-NA-"
                    });
                }
                datas.Sort((a, b) => { return a.Index - b.Index; });

                if (sb.Length > 0)
                    sb.AppendLine(sepalator);
                sb.AppendLine($"【Result {++count:D4}】");

                foreach (ExportData data in datas)
                {
                    sb.AppendLine($"\t[{data.Name}]{Environment.NewLine}\t\t{data.Value}");
                }
            }
            return sb.ToString();
        }
        private string ToTab()
        {
            StringBuilder sb = new StringBuilder();
            List<string> headers = new List<string>();
            foreach (Result result in Results)
            {
                Type t = result.GetType();
                PropertyInfo[] infos = t.GetProperties();

                List<ExportData> datas = new List<ExportData>();
                foreach (PropertyInfo info in infos)
                {
                    IgnoreDataMemberAttribute ignore = info.GetCustomAttribute(typeof(IgnoreDataMemberAttribute), false) as IgnoreDataMemberAttribute;
                    if (ignore != null)
                        continue;
                    DataMemberAttribute datamember = info.GetCustomAttribute(typeof(DataMemberAttribute), false) as DataMemberAttribute;
                    if (datamember == null)
                        continue;

                    string name = !String.IsNullOrEmpty(datamember.Name) ? datamember.Name : info.Name;

                    string value = info.GetValue(result).ToString();
                    value = value.Trim(new[] { ' ', '\r', '\n', '\t' });
                    value = Regex.Replace(value, $"[{Environment.NewLine}]+", $" ");

                    datas.Add(new ExportData()
                    {
                        Index = datamember.Order,
                        Name = name,
                        Value = value
                    });
                }
                datas.Sort((a, b) => { return a.Index - b.Index; });

                // print header
                if (headers.Count == 0)
                {
                    foreach (ExportData data in datas)
                    {
                        headers.Add(data.Name);
                    }
                }
                List<string> record = new List<string>();
                foreach (ExportData data in datas)
                {
                    record.Add(data.Value);
                }
                sb.AppendLine(String.Join("\t", record));
            }
            sb.Insert(0, String.Join("\t", headers) + Environment.NewLine);
            return sb.ToString();
        }
        private string ToXml()
        {
            string str = "";
            StringWriter sw = null;
            XmlWriter xw = null;
            XmlSerializerNamespaces ns = null;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Encoding = UTF8,
                    Indent = true,
                    OmitXmlDeclaration = true
                };
                ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);

                sw = new StringWriter();
                xw = XmlWriter.Create(sw, settings);

                xmlSerializer.Serialize(xw, this, ns);
                str = sw.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xw?.Close();
                xw?.Dispose();
                sw?.Close();
                sw?.Dispose();
            }
            return str;
        }
        private string ToJson()
        {
            string str = "";
            MemoryStream ms = null;
            StreamReader sr = null;
            try
            {
                ms = new MemoryStream();
                jsonSerializer.WriteObject(ms, this);
                ms.Position = 0;
                sr = new StreamReader(ms);
                str = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sr?.Close();
                sr?.Dispose();
                ms?.Close();
                ms?.Dispose();
            }
            return str;
        }
        private string ToHtml()
        {
            string str = "";
            HtmlReporter html = null;
            try
            {
                html = new HtmlReporter();
                html.Title = "検証結果";
                html.ApplicationInfo = $"Report By: {ReportBy}";
                html.ModifiedInfo = $"Written To: {WrittenTo}";
                html.TotalSyntaxError = $"Syntax Error: {TotalSyntaxError,4}";
                html.TotalAssert = $"Total Assert: {TotalAssert,4}";
                html.TotalReport = $"Total Report: {TotalReport,4}";

                IEnumerable<IGrouping<string, Result>> query = Results.GroupBy(item => item.XmlFile.FullName);
                foreach (IGrouping<string, Result> file in query)
                {
                    html.WriteReport(file.Key, file.ToList());
                }

                str = html.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        #endregion
        #region Deserializer
        public void Deserialize(string path, ExportFormats format)
        {
            try
            {
                if (format == ExportFormats.Log)
                    FromLog(path);
                else if (format == ExportFormats.Tab)
                    FromTab(path);
                else if (format == ExportFormats.Xml)
                    FromXml(path);
                else if (format == ExportFormats.Json)
                    FromJson(path);
                else if (format == ExportFormats.Html)
                    FromHtml(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FromLog(string path)
        {
            throw new NotSupportedException();
        }
        private void FromTab(string path)
        {
            throw new NotSupportedException();
        }
        private void FromXml(string path)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path, UTF8);
                Reporter repoter = (Reporter)xmlSerializer.Deserialize(sr);
                Update(repoter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sr?.Close();
            }
        }
        private void FromJson(string path)
        {
            MemoryStream ms = null;
            try
            {
                string str = File.ReadAllText(path, UTF8);
                byte[] bytes = UTF8.GetBytes(str);
                ms = new MemoryStream(bytes);
                Reporter repoter = (Reporter)jsonSerializer.ReadObject(ms);
                Update(repoter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ms?.Close();
                ms?.Dispose();
            }
        }
        private void FromHtml(string path)
        {
            throw new NotSupportedException();
        }


        #endregion
        private class ExportData
        {
            internal int Index { get; set; } = 0;
            internal string Name { get; set; } = "";
            internal string Value { get; set; } = "";
        }
        #endregion
        #region Report Read/Write
        public void Read(string path)
        {
            try
            {
                Deserialize(path, Format);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Write(string path)
        {
            try
            {
                File.WriteAllText(path, ToString(Format), UTF8);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Update Method
        private void Update(Reporter repoter)
        {
            ReportBy = repoter.ReportBy;
            WrittenTo = repoter.WrittenTo;
            Results = repoter.Results;
        }
        #endregion
    }

}
