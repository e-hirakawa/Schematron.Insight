using Saxon.Api;
using Schematron.Insight.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Schematron.Insight.Processors
{
    /// <summary>
    /// スキマトロン形式変換クラス
    /// </summary>
    internal class ProcessorFactory : IDisposable
    {
        #region Private Properties
        private string _workdir = "";

        private string _file_iso_dsdl_include = "iso_dsdl_include.xsl";
        private string _file_iso_abstract_expand = "iso_abstract_expand.xsl";
        private string _file_iso_svrl_for_xslt2 = "iso_svrl_for_xslt2.xsl";

        private Processor _processor = null;
        private XsltCompiler _compiler = null;
        private XsltTransformer _iso_dsdl_include = null;
        private XsltTransformer _iso_abstract_expand = null;
        private XsltTransformer _iso_svrl_for_xslt2 = null;

        private XsltTransformer _schematron = null;
        #endregion
        #region Public Properties
        public Processor Processor => _processor;
        #endregion
        #region Constructors
        public ProcessorFactory()
        {
            _processor = new Processor();
        }
        #endregion
        #region Private Methods
        private void CopyProcessors(string srcdir, string dstdir)
        {
            try
            {
                DirectoryInfo srcdirinfo = new DirectoryInfo(srcdir);
                FileInfo[] srcfiles = srcdirinfo.GetFiles("*.xsl", SearchOption.AllDirectories);
                foreach (FileInfo srcfile in srcfiles)
                {
                    srcfile.CopyTo(Path.Combine(dstdir, srcfile.Name), true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private XsltTransformer CreateTramsformer(string path)
        {
            XsltTransformer xslt = null;
            Stream sm = null;
            try
            {
                sm = new FileInfo(path).Open(FileMode.Open, FileAccess.Read, FileShare.Read);

                xslt = CreateTramsformer(sm, new Uri(Path.GetDirectoryName(path)+"/"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sm?.Close();
                sm?.Dispose();
            }
            return xslt;
        }
        private XsltTransformer CreateTramsformer(Stream sm, Uri baseuri)
        {
            XsltTransformer xslt = null;
            try
            {
                _compiler.BaseUri = baseuri;


                xslt = _compiler.Compile(sm).Load();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xslt;
        }
        private void CloseXsltTransformer(XsltTransformer xslt)
        {
            try
            {
                xslt = null;
            }
            catch
            {
                Debug.Print("Failed Dispose of " + nameof(xslt));
            }
        }

        private Stream Transform(XsltTransformer xslt, Stream src, Uri baseuri)
        {
            MemoryStream dst = null;
            DomDestination dom = new DomDestination();
            try
            {
                xslt.SetInputStream(src, baseuri);
                xslt.Run(dom);

                dst = new MemoryStream();

                dom.XmlDocument.Save(dst);

                dst.Flush();
                dst.Position = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dom.Close();
            }
            return dst;
        }
        #endregion
        #region Public Methods for Validation
        public void InitializeCompiler()
        {
            try
            {
                _workdir = Path.Combine(LibraryInfo.WorkingDirectory, "processors");
                if (!Directory.Exists(_workdir))
                    Directory.CreateDirectory(_workdir);

                CopyProcessors(LibraryInfo.ExecuteDirectory, _workdir);

                _compiler = _processor.NewXsltCompiler();

                _iso_dsdl_include = CreateTramsformer(Path.Combine(_workdir, _file_iso_dsdl_include));
                _iso_abstract_expand = CreateTramsformer(Path.Combine(_workdir, _file_iso_abstract_expand));
                _iso_svrl_for_xslt2 = CreateTramsformer(Path.Combine(_workdir, _file_iso_svrl_for_xslt2));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Compile(FileInfo file)
        {
            Stream src = null;
            Stream dst = null;
            try
            {
                if (_processor == null)
                {
                    throw new Exception("incompleted initialize");
                }
                
                Uri baseuri = new Uri(file.DirectoryName + "/");
                src = file.OpenRead();

                dst = Transform(_iso_dsdl_include, src, baseuri);

                dst = Transform(_iso_abstract_expand, dst, baseuri);

                dst = Transform(_iso_svrl_for_xslt2, dst, baseuri);

                _schematron = CreateTramsformer(dst, baseuri);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
            finally
            {
                src?.Close();
                src?.Dispose();
            }
        }
        public XmlDocument Validation(string xmlfile)
        {
            XmlDocument doc = null;
            DomDestination dom = new DomDestination();
            Stream src = null;
            try
            {
                if (_schematron == null)
                    throw new Exception("incompleted schema stylesheet");

                FileInfo file = new FileInfo(xmlfile);
                Uri baseuri = new Uri(file.DirectoryName + "/");

                src = file.OpenRead();

                _schematron.SetInputStream(src, baseuri);
                _schematron.Run(dom);

                doc = dom.XmlDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dom.Close();
                src?.Close();
                src?.Dispose();
            }
            return doc;
        }
        public void Close()
        {
            CloseXsltTransformer(_schematron);
        }
        #endregion
        #region IDisposable
        public void Dispose()
        {
            CloseXsltTransformer(_schematron);

            CloseXsltTransformer(_iso_dsdl_include);
            CloseXsltTransformer(_iso_abstract_expand);
            CloseXsltTransformer(_iso_svrl_for_xslt2);
            _processor = null;
            _compiler = null;
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
