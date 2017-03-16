using System;
using System.ComponentModel.DataAnnotations;


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
}
