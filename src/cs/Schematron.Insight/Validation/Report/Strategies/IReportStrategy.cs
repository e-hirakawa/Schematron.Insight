using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Insight.Validation.Report.Strategies
{
    public interface IReportStrategy
    {
        string Name { get; }
        string Description { get; }
        string Serialize(Reporter reporter);
        Reporter Deserialize(string str);
    }
}
