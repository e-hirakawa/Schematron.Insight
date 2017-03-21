using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Insight.Validation.Report.Strategies
{
    public class ReportDefaultStrategy : ReportXmlStrategy
    {
        public ReportDefaultStrategy():base()
        {
            Name = $"Default({base.Name})";
        }
    }
}
