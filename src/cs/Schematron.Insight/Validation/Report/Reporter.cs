using Schematron.Insight.Validation.Report.Strategies;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Schematron.Insight.Validation.Report
{
    [Serializable()]
    [XmlRoot(ElementName = "Report")]
    [DataContract(Name = "Report")]
    public class Reporter
    {
        #region ReadOnly Field
        #endregion
        #region Private Properties
        private ResultCollection _results = new ResultCollection();
        #endregion
        #region Public Properties
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

        public Stream ToStream(IReportStrategy strategy = null)
        {
            MemoryStream stream = null;
            StreamWriter writer = null;
            string str = "";
            try
            {
                str = Serialize(strategy);
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
        public string Serialize(IReportStrategy strategy = null)
        {
            string str = "";
            try
            {
                if (strategy == null)
                    strategy = new ReportDefaultStrategy();

                str = strategy.Serialize(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        
        #endregion
        #region Deserializer
        public void Deserialize(string str, IReportStrategy strategy = null)
        {
            try
            {
                if (strategy == null)
                    strategy = new ReportDefaultStrategy();

                Reporter reporter = strategy.Deserialize(str);

                ReportBy = reporter.ReportBy;
                WrittenTo = reporter.WrittenTo;
                Results = reporter.Results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #endregion
        #region Report Read/Write
        public void Read(string path, IReportStrategy strategy = null)
        {
            try
            {
                string str = File.ReadAllText(path, Encoding.UTF8);

                Deserialize(str, strategy);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Write(string path, IReportStrategy strategy = null)
        {
            try
            {
                string str = Serialize(strategy);

                File.WriteAllText(path, str, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

}
