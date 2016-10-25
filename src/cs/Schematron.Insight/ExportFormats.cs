using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace Schematron.Insight
{
    [Flags]
    public enum ExportFormats : int
    {
        /// <summary>
        /// formatted log text
        /// </summary>
        [Display(Name = "Log")]
        Log = 0,
        /// <summary>
        /// Tab split text
        /// </summary>
        [Display(Name = "Tab")]
        Tab = 1 << 0,
        /// <summary>
        /// markup text
        /// </summary>
        [Display(Name = "Xml")]
        Xml = 1 << 1,
        /// <summary>
        /// json
        /// </summary>
        [Display(Name = "Json")]
        Json = 1 << 2,
        /// <summary>
        /// Html
        /// </summary>
        [Display(Name = "Html")]
        Html = 1 << 3
    }
    public static class ExportFormatsHelper
    {

        public static bool Contains(this ExportFormats target, ExportFormats flags)
        {
            return (target & flags) == flags;
        }

        public static bool NotContains(this ExportFormats target, ExportFormats flags)
        {
            return (target & (~flags)) == 0;
        }

        public static ExportFormats Append(this ExportFormats target, ExportFormats flags)
        {
            return target | flags;
        }

        public static ExportFormats Remove(this ExportFormats target, ExportFormats flags)
        {
            return target & (~flags);
        }

        public static string DisplayName(this ExportFormats value)
        {
            FieldInfo info = value.GetType().GetField(value.ToString());
            var attrs = info.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            return (attrs?.Length ?? 0) > 0 ? attrs[0].Name : value.ToString();
        }
    }
}
