using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Schematron.Insight.Validation.Report.Strategies
{
    /// <summary>
    /// Formatted log text
    /// Serialize can available, but Deserialize cannot.
    /// </summary>
    public class ReportLogStrategy : IReportStrategy
    {
        #region Public Properties
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        #endregion
        #region Constructor
        public ReportLogStrategy()
        {
            Name = "Log";
            Description = "Formatted log text.\nSerialize can available, but Deserialize cannot.";
        }
        #endregion
        public string Serialize(Reporter reporter)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            string sepalator = Environment.NewLine + new string('-', 20) + Environment.NewLine;
            sb.AppendLine(sepalator);
            sb.AppendLine("【検証結果】");
            sb.AppendLine($"Report By: {reporter.ReportBy}");
            sb.AppendLine($"Written To: {reporter.WrittenTo}");
            sb.AppendLine();
            sb.AppendLine($"Syntax Error: {reporter.TotalSyntaxError,4}");
            sb.AppendLine($"Total Assert: {reporter.TotalAssert,4}");
            sb.AppendLine($"Total Report: {reporter.TotalReport,4}");
            sb.AppendLine(sepalator);
            foreach (Result result in reporter.Results)
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
        public Reporter Deserialize(string str)
        {
            throw new NotImplementedException();
        }

    }
}
