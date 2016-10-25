using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Schematron.Insight
{
    public static class LibraryInfo
    {
        public static string Name { get; private set; }
        public static string Version { get; private set; }
        public static string Company { get; private set; }
        public static string ExecuteDirectory { get; private set; }
        public static string WorkingDirectory { get; private set; }
        static LibraryInfo()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            AssemblyName asmn = asm.GetName();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);

            Name = asmn.Name;
            Version = asmn.Version.ToString(4);
            Company = fvi.CompanyName;

            UriBuilder codeBase = new UriBuilder(asm.CodeBase);
            ExecuteDirectory = Path.GetDirectoryName(Uri.UnescapeDataString(codeBase.Path));
            try
            {
                string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string dir = Path.Combine(local, $"{Name}-{Company}");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                dir = Path.Combine(dir, Version);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                WorkingDirectory = dir;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
