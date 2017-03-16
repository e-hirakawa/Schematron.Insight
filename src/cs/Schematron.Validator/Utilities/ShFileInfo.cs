using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Schematron.Validator.Utilities
{
    internal class ShFileInfo
    {
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(
            string pszPath, uint dwFileAttributes,
            ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatKBSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_TYPENAME = 0x400;

        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        private static Char Chr(int i)
        {
            return Convert.ToChar(i);
        }

        private static SHFILEINFO shinfo;
        static ShFileInfo()
        {
            shinfo = new SHFILEINFO();
        }
        internal static string GetType(string path)
        {
            shinfo.szDisplayName = new String(Chr(0), 260);
            shinfo.szTypeName = new String(Chr(0), 80);
            IntPtr hSuccess = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_TYPENAME);
            return shinfo.szTypeName;
        }

        internal static Icon GetIcon(string path)
        {
            IntPtr hImgSmall = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
            return Icon.FromHandle(shinfo.hIcon);
        }
        internal static Icon GetLargeIcon(string path)
        {
            IntPtr hImgSmall = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
            return Icon.FromHandle(shinfo.hIcon);
        }
        internal static string FileSizeToString(long fileSize)
        {
            StringBuilder builder = new StringBuilder(32);
            StrFormatByteSize(fileSize, builder, 32);
            return builder.ToString();
        }
        internal static string FileKBSizeToString(long fileSize)
        {
            StringBuilder builder = new StringBuilder(32);
            StrFormatKBSize(fileSize, builder, 32);
            return builder.ToString();
        }

    }

}
