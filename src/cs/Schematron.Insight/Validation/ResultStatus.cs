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
    public static class ResultStatusHelper
    {

        public static bool Contains(this ResultStatus target, ResultStatus flags)
        {
            return (target & flags) == flags;
        }

        public static bool NotContains(this ResultStatus target, ResultStatus flags)
        {
            return (target & (~flags)) == 0;
        }

        public static ResultStatus Append(this ResultStatus target, ResultStatus flags)
        {
            return target | flags;
        }

        public static ResultStatus Remove(this ResultStatus target, ResultStatus flags)
        {
            return target & (~flags);
        }

        public static string DisplayName(this ResultStatus value)
        {
            FieldInfo info = value.GetType().GetField(value.ToString());
            var attrs = info.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            return (attrs?.Length ?? 0) > 0 ? attrs[0].Name : value.ToString();
        }
        public static ResultStatus GetValueFromDisplayName(string name)
        {
            foreach (ResultStatus s in Enum.GetValues(typeof(ResultStatus)))
            {
                if (s.DisplayName() == name)
                    return s;
            }
            return ResultStatus.None;
        }
    }
}
