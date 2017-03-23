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
    public class DocumentModel : ViewModelBase, IDisposable
    {
        #region Private Properties
        private string _id;
        private string _fullpath;
        private string _directory;
        private string _filename;
        private long _filesize;
        private string _dispsize;
        private DateTime? _modified;
        private string _dispdate;
        private ImageSource _icon;
        private DocumentStatus _status = DocumentStatus.None;
        private string _message;



        #endregion
        #region Public Properties
        /// <summary>
        /// GUID
        /// </summary>
        public string Id
        {
            get { return _id; }
            private set
            {
                if (_id != value)
                    Set(() => Id, ref _id, value);
            }
        }
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
                    Set(() => FullPath, ref _fullpath, value);
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
                    Set(() => Directory, ref _directory, value);
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
                if (_filename != value)
                    Set(() => Name, ref _filename, value);
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
                    Set(() => Size, ref _filesize, value);
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
                    Set(() => DisplaySize, ref _dispsize, value);
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
                    Set(() => ModifiedDate, ref _modified, value);
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
                    Set(() => DisplayModifiedDate, ref _dispdate, value);
            }
        }
        /// <summary>
        /// File Icon
        /// </summary>
        public ImageSource Icon
        {
            get { return _icon; }
            private set
            {
                if (_icon != value)
                    Set(() => Icon, ref _icon, value);
            }
        }
        /// <summary>
        /// Document Status
        /// </summary>
        public DocumentStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                    Set(() => Status, ref _status, value);
            }
        }
        /// <summary>
        /// Status Message
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                    Set(() => Message, ref _message, value);
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// ResourceModel Constructor
        /// </summary>
        /// <param name="filepath">Resource File Path</param>
        public DocumentModel(string filepath)
        {
            Id = Guid.NewGuid().ToString("N");
            FullPath = filepath;
        }
        /// <summary>
        /// ResourceModel Constructor
        /// </summary>
        public DocumentModel() : this(null) { }
        #endregion
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateProperties()
        {
            UpdateProperties(FullPath);
        }
        protected virtual void UpdateProperties(string filepath)
        {
            FileInfo fi = File.Exists(filepath) ? new FileInfo(filepath) : null;
            Directory = fi?.DirectoryName;
            Name = fi?.Name;
            Size = fi?.Length ?? 0;
            DisplaySize = ToDisplaySize(Size);
            ModifiedDate = fi?.LastWriteTime;
            DisplayModifiedDate = ToDisplayDate(ModifiedDate);
            Icon = CreateBitmapIcon(fi.FullName);
            Status = DocumentStatus.None;
            Message = "";
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
        private static ImageSource CreateBitmapIcon(string fullName)
        {
            BitmapSource img = null;
            System.Drawing.Icon icon = null;
            Int32Rect rect;
            try
            {
                icon = ShFileInfo.GetLargeIcon(fullName);
                rect = new Int32Rect(0, 0, icon.Width, icon.Height);
                img = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, rect, BitmapSizeOptions.FromEmptyOptions());
                img.Freeze();
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

        public virtual void Dispose() { }

        #endregion
    }
}
