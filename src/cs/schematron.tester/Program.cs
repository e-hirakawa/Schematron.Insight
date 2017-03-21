using Schematron.Insight;
using Schematron.Insight.Utilities;
using Schematron.Insight.Validation;
using Schematron.Insight.Validation.Report;
using Schematron.Insight.Validation.Report.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
//using Wmhelp.XPath2;

namespace schematron.tester.cui
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgumentInfo arginfo = new ArgumentInfo();
            arginfo.PrintHeader();
            arginfo.Load(args);
            if (arginfo.IsEnabled)
            {
                Directory.SetCurrentDirectory(arginfo.SchFile.DirectoryName);

                Execute(arginfo);
            }
#if DEBUG
            Console.ReadLine();
#endif
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void Execute(ArgumentInfo arginfo)
        {
            string disp_se = ResultStatus.SyntaxError.DisplayName();
            string disp_fa = ResultStatus.Assert.DisplayName();
            string disp_sr = ResultStatus.Report.DisplayName();
            string reportheader = $"{disp_se} | {disp_fa} | {disp_sr}";
            string reportborder = new string('-', reportheader.Length);
            Console.ForegroundColor = ConsoleColor.Gray;

            SchemaDocument doc = null;
            try
            {
                doc = new SchemaDocument();

                doc.Open(arginfo.SchFile.FullName);
                doc.Compile(arginfo.Phase);

                ResultCollection results = new ResultCollection();
                foreach (FileInfo xmlfile in arginfo.XmlFiles)
                {
                    Console.WriteLine("");
                    Console.WriteLine($"> Validation Start of '{xmlfile.Name}' by '{doc.SchemaTmp.Name}'");

                    ResultCollection file_results = doc.Validation(xmlfile.FullName);

                    string se = String.Format("{0, " + disp_se.Length + "}", file_results.TotalSyntaxError);
                    string fa = String.Format("{0, " + disp_fa.Length + "}", file_results.TotalAssert);
                    string sr = String.Format("{0, " + disp_sr.Length + "}", file_results.TotalReport);
                    Console.WriteLine("  " + reportborder);
                    Console.WriteLine("  " + reportheader);
                    Console.WriteLine($"  {se} | {fa} | {sr}");
                    Console.WriteLine("  " + reportborder);

                    Console.WriteLine("> End of Validation");

                    results.AddRange(file_results);

                }
                if (results.Count > 0 && arginfo.OutFile != null)
                {
                    Reporter report = new Reporter();
                    report.Results = results;
                    IReportStrategy strategy = EmbeddReportStrategies.Find(arginfo.OutFormat);
                    if (strategy == null)
                        strategy = new ReportDefaultStrategy();

                    report.Write(arginfo.OutFile.FullName, strategy);
                }
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("");
                Console.WriteLine("COMPLETE!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.WriteLine("ERROR!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                doc?.Dispose();
            }
        }
        private static List<FileInfo> GetXmlFiles(string path)
        {
            List<string> files = new List<string>();
            if (File.Exists(path) && Path.GetExtension(path).ToLower() == ".xml")
            {
                files.Add(path);
            }
            else if (Directory.Exists(path))
            {
                files.AddRange(Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly));
            }
            List<FileInfo> fi = new List<FileInfo>();
            for (int i = 0; i < files.Count; i++)
            {
                fi.Add(new FileInfo(files[i]));
            }
            return fi;
        }
    }
}
