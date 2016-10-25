using System.Collections.Generic;
using System.IO;

namespace Schematron.Insight.Utilities
{
    internal class FileSystemHelper
    {
        public static List<string> CopyDirectory(string srcdir, string dstdir)
        {
            List<string> dstfiles = new List<string>();
            dstdir = Path.Combine(dstdir, Path.GetFileName(srcdir));
            if (!Directory.Exists(dstdir))
            {
                Directory.CreateDirectory(dstdir);
            }

            string[] files = Directory.GetFiles(srcdir);
            foreach (string file in files)
            {
                string dstfile = Path.Combine(dstdir, Path.GetFileName(file));
                File.Copy(file, dstfile, true);
                dstfiles.Add(dstfile);
            }
            string[] dirs = Directory.GetDirectories(srcdir);
            foreach (string dir in dirs)
            {
                List<string> buf = CopyDirectory(dir, dstdir);
                dstfiles.AddRange(buf);
            }
            return dstfiles;
        }

        public static void DeleteDirectory(string dir)
        {
            DirectoryInfo di = new DirectoryInfo(dir);

            RemoveReadonlyAttribute(di);

            di.Delete(true);
        }

        public static void RemoveReadonlyAttribute(DirectoryInfo dirInfo)
        {
            if ((dirInfo.Attributes & FileAttributes.ReadOnly) ==
                FileAttributes.ReadOnly)
                dirInfo.Attributes = FileAttributes.Normal;
            foreach (FileInfo fi in dirInfo.GetFiles())
                if ((fi.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly)
                    fi.Attributes = FileAttributes.Normal;
            foreach (DirectoryInfo di in dirInfo.GetDirectories())
                RemoveReadonlyAttribute(di);
        }
        public static bool IsLocalDrive(string path)
        {
            string root = Path.GetPathRoot(path);
            foreach (DriveInfo info in DriveInfo.GetDrives())
            {
                if (info.Name == root)
                    return info.DriveType == DriveType.Fixed;
            }
            return false;
        }

    }
}
