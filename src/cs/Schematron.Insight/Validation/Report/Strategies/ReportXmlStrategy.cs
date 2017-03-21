using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Schematron.Insight.Validation.Report.Strategies
{
    /// <summary>
    /// markup text
    /// Serialize/Deserialize can available
    /// </summary>
    public class ReportXmlStrategy : IReportStrategy
    {
        #region Private Properties
        private readonly static XmlSerializer xmlSerializer =
            new XmlSerializer(typeof(Reporter));
        #endregion
        #region Public Properties
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        #endregion
        #region Constructor
        public ReportXmlStrategy()
        {
            Name = "Xml";
            Description = "markup text format.\nSerialize/Deserialize can available.";
        }
        #endregion

        public string Serialize(Reporter reporter)
        {
            string str = "";
            StringWriter sw = null;
            XmlWriter xw = null;
            XmlSerializerNamespaces ns = null;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8,
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
        public Reporter Deserialize(string str)
        {
            Reporter repoter = null;
            TextReader sr = null;
            try
            {
                sr = new StringReader(str);
                repoter = (Reporter)xmlSerializer.Deserialize(sr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sr?.Close();
                sr?.Dispose();
            }
            return repoter;
        }
    }
}
