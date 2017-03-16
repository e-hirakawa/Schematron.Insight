using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Validator.Utilities
{
    /// <summary>
    /// Application Infomation Providing Class
    /// </summary>
    internal static class ApplicationInfo
    {
        /// <summary>
        /// Application Name
        /// </summary>
        public static string Name { get; private set; }
        /// <summary>
        /// Application Version
        /// </summary>
        public static string Version { get; private set; }

        static ApplicationInfo()
        {
            AssemblyName asm = Assembly.GetExecutingAssembly().GetName();
            Name = asm.Name;
            Version = asm.Version.ToString();
        }
    }
}
