using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Schematron.Insight.Validation
{
    [Flags]
    public enum ResultStatus : int
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Syntax Error")]
        SyntaxError = 1 << 1,
        [Display(Name = "Assert")]
        Assert = 1 << 2,
        [Display(Name = "Report")]
        Report = 1 << 3
    }
}
