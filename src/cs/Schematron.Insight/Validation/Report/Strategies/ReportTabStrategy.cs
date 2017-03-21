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
    /// Tab split text
    /// Serialize can available, but Deserialize cannot.
    /// </summary>
    public class ReportTabStrategy : IReportStrategy
    {
        #region Public Properties
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        #endregion
        #region Constructor
        public ReportTabStrategy()
        {
            Name = "Tab";
            Description = "Tab split text format.\nSerialize can available, but Deserialize cannot.";
        }
        #endregion
        public string Serialize(Reporter reporter)
        {
            StringBuilder sb = new StringBuilder();
            List<string> headers = new List<string>();
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

        public Reporter Deserialize(string str)
        {
            throw new NotImplementedException();
        }
    }
}
