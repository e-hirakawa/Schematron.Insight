using System;
using System.ComponentModel.DataAnnotations;


namespace Schematron.Insight
{
    [Flags]
    public enum ExportFormats : int
    {
        /// <summary>
        /// Formatted log text
        /// Serialize can available, but Deserialize cannot.
        /// </summary>
        [Display(Name = "Log", Description = "Serialize can available, but Deserialize cannot.")]
        Log = 0,
        /// <summary>
        /// Tab split text
        /// Serialize can available, but Deserialize cannot.
        /// </summary>
        [Display(Name = "Tab", Description = "Serialize can available, but Deserialize cannot.")]
        Tab = 1 << 0,
        /// <summary>
        /// markup text
        /// Serialize/Deserialize can available
        /// </summary>
        [Display(Name = "Xml", Description = "Serialize/Deserialize can available")]
        Xml = 1 << 1,
        /// <summary>
        /// json
        /// Serialize/Deserialize can available
        /// </summary>
        [Display(Name = "Json", Description = "Serialize/Deserialize can available")]
        Json = 1 << 2,
        /// <summary>
        /// Html
        /// Serialize can available, but Deserialize cannot.
        /// </summary>
        [Display(Name = "Html", Description = "Serialize can available, but Deserialize cannot.")]
        Html = 1 << 3
    }
}
