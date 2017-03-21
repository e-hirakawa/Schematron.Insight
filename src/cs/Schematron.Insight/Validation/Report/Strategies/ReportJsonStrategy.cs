using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Insight.Validation.Report.Strategies
{
    /// <summary>
    /// json
    /// Serialize/Deserialize can available
    /// </summary>
    public class ReportJsonStrategy : IReportStrategy
    {

        #region Private Properties
        private readonly static DataContractJsonSerializer jsonSerializer =
            new DataContractJsonSerializer(typeof(Reporter));
        #endregion
        #region Public Properties
        public string Name { get; protected set; } 
        public string Description { get; protected set; } 
        #endregion
        #region Constructor
        public ReportJsonStrategy()
        {
            Name = "Json";
            Description = "Serialize/Deserialize can available.";
        }
        #endregion
        public string Serialize(Reporter reporter)
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
        public Reporter Deserialize(string str)
        {
            Reporter repoter = null;
            MemoryStream ms = null;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                ms = new MemoryStream(bytes);
                repoter = (Reporter)jsonSerializer.ReadObject(ms);
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
            return repoter;
        }
    }
}
