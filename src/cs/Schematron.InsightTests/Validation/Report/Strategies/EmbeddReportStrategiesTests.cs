using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schematron.Insight.Validation.Report.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Insight.Validation.Report.Strategies.Tests
{
    [TestClass()]
    public class EmbeddReportStrategiesTests
    {
        [TestMethod()]
        public void GetEnumeratorTest()
        {
            foreach(IReportStrategy rs in EmbeddReportStrategies.GetEnumerator())
            {
                Debug.Print("[{0}]{1}: {2}", rs.GetType().Name,  rs.Name, rs.Description);
            }
        }
    }
}