using Saxon.Api;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Schematron.Insight.Processors
{
    public class ProcessorEvaluation
    {
        private string _xml = "";
        private XDocument _xdoc = null;
        private XdmNode _dom = null;
        private XPathCompiler _compile = null;
        public ProcessorEvaluation(XDocument src)
        {
            _xml = ToStringWithDeclaration(src);
            _xdoc = RemoveNamespace(_xml);
        }
        public void Compile(Processor proc)
        {
            XmlDocument doc;
            try
            {
                doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(_xml);

                DocumentBuilder builder = proc.NewDocumentBuilder();
                _dom = builder.Wrap(doc);

                _compile = proc.NewXPathCompiler();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public XmlNode SelectNode(string location)
        {
            XPathSelector xpSel;
            try
            {
                xpSel = _compile.Compile(location).Load();
                xpSel.ContextItem = _dom;
                foreach (XdmNode xdmn in xpSel)
                {
                    return xdmn.getUnderlyingXmlNode();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public XElement XPathSelectElement(string findAt)
        {
            return !String.IsNullOrEmpty(findAt) ? _xdoc.XPathSelectElement(findAt) : null;
        }
        private XDocument RemoveNamespace(string xml)
        {
            XDocument dst = XDocument.Parse(xml, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
            foreach (XElement e in dst.Root.DescendantsAndSelf())
            {
                if (e.Name.Namespace != XNamespace.None)
                {
                    e.Name = XNamespace.None.GetName(e.Name.LocalName);
                }
                if (e.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
                {
                    e.ReplaceAttributes(e.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));
                }
            }
            return dst;
        }
        private string ToStringWithDeclaration(XDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            StringBuilder builder = new StringBuilder();
            using (TextWriter writer = new StringWriter(builder))
            {
                doc.Save(writer);
            }
            return builder.ToString();
        }
    }
}
