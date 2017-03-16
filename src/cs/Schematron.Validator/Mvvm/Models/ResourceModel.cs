using GalaSoft.MvvmLight;
using Schematron.Validator.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Schematron.Validator.Mvvm.Models
{
    public class ResourceModel:ViewModelBase
    {
        #region Private Properties
        private string _fullpath;
        private string _directory;
        private string _filename;
        private long _filesize;
        private string _dispsize;
        private DateTime? _modified;
        private string _dispdate;
        private ImageSource _icon;

        #endregion
        #region Public Properties
        /// <summary>
        /// Absolute File Path
        /// </summary>
        public string FullPath
        {
            get { return _fullpath; }
            set
            {
                if (_fullpath != value)
                {
                    _fullpath = value;
                    RaisePropertyChanged(() => FullPath);
                    UpdateProperties(value);
                }
            }
        }
        /// <summary>
        /// Parent Directory Path
        /// </summary>
        public string Directory
        {
            get { return _directory; }
            private set
            {
                if (_directory != value)
                {
                    _directory = value;
                    RaisePropertyChanged(() => Directory);
                }
            }
        }
        /// <summary>
        /// File Name
        /// </summary>
        public string Name
        {
            get { return _filename; }
            private set
            {
                if(_filename != value)
                {
                    _filename = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        /// <summary>
        /// File Raw Size
        /// </summary>
        public long Size
        {
            get { return _filesize; }
            private set
            {
                if (_filesize != value)
                {
                    _filesize = value;
                    RaisePropertyChanged(() => Size);
                }
            }
        }
        /// <summary>
        /// File Display Size
        /// </summary>
        public string DisplaySize
        {
            get { return _dispsize; }
            private set
            {
                if (_dispsize != value)
                {
                    _dispsize = value;
                    RaisePropertyChanged(() => DisplaySize);
                }
            }
        }
        /// <summary>
        /// File DateTime Of Modified
        /// </summary>
        public DateTime? ModifiedDate
        {
            get { return _modified; }
            private set
            {
                if (_modified != value)
                {
                    _modified = value;
                    RaisePropertyChanged(() => ModifiedDate);
                }
            }
        }
        /// <summary>
        /// File Display DateTime Of Modified
        /// </summary>
        public string DisplayModifiedDate
        {
            get { return _dispdate; }
            private set
            {
                if (_dispdate != value)
                {
                    _dispdate = value;
                    RaisePropertyChanged(() => DisplayModifiedDate);
                }
            }
        }

        public ImageSource Icon
        {
            get { return _icon; }
            private set {
                if (_icon != value)
                {
                    _icon = value;
                    RaisePropertyChanged(() => Icon);
                }
            }
        }
        #endregion
        #region Constructors
        public ResourceModel(string filepath)
        {
            FullPath = filepath;
        }
        public ResourceModel(): this(null) { }
        #endregion
        #region Methods
        public void UpdateProperties()
        {
            UpdateProperties(FullPath);
        }
        private void UpdateProperties(string filepath)
        {
            FileInfo fi = File.Exists(filepath) ? new FileInfo(filepath) : null;
            Directory = fi?.DirectoryName;
            Name = fi?.Name;
            Size = fi?.Length ?? 0;
            DisplaySize = ToDisplaySize(Size);
            ModifiedDate = fi?.LastWriteTime ;
            DisplayModifiedDate = ToDisplayDate(ModifiedDate);
            Icon = CreateBitmapIcon(fi.FullName);
        }

        private static string ToDisplaySize(double size)
        {
            int block = 1024;
            int index = 0;
            string[] units = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            while (size >= block && index + 1 < units.Length)
            {
                size = size / block;
                index++;
            }
            return $"{size:n1} {units[index]}";
        }
        private static string ToDisplayDate(DateTime? date)
        {
            string str = "";
            if (date.HasValue)
            {
                str = date.Value.ToString();
            }
            return str;
        }
        private ImageSource CreateBitmapIcon(string fullName)
        {
            BitmapSource img = null;
            System.Drawing.Icon icon = null;
            Int32Rect rect;
            try
            {
                icon = ShFileInfo.GetLargeIcon(fullName);
                rect = new Int32Rect(0, 0, icon.Width, icon.Height);
                img = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, rect, BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            finally
            {
                icon?.Dispose();
            }
            return img;
        }

        #endregion
    }
}
