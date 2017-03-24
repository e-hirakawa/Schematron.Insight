using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Schematron.Validator.Mvvm.Models;
using Schematron.Validator.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Schematron.Validator.Mvvm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Properties
        private ProgressModel _progress;
        private DocumentSchemaModel _schema;
        private DocumentXmlCollection _xmls = new DocumentXmlCollection();
        private DocumentXmlModel _selectedXml;

        private DocumentStatus _status = DocumentStatus.None;

        public DocumentStatus Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }



        #endregion
        #region Public Properties
        public string Title => $"{ApplicationInfo.Name} -ver.{ApplicationInfo.Version}";
        /// <summary>
        /// 進捗管理モデル
        /// </summary>
        public ProgressModel Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                    Set(() => Progress, ref _progress, value);
            }
        }

        public DocumentSchemaModel Schema
        {
            get { return _schema; }
            set
            {
                if (_schema != value)
                {
                    Set(() => Schema, ref _schema, value);
                }
            }
        }
        public DocumentXmlCollection Xmls
        {
            get { return _xmls; }
            set
            {
                if (_xmls != value)
                {
                    Set(() => Xmls, ref _xmls, value);
                }
            }
        }
        public DocumentXmlModel SelectedXmlFile
        {
            get { return _selectedXml; }
            set {
                Set(() => SelectedXmlFile, ref _selectedXml, value);
            }
        }
        #endregion
        #region Constructors
        public MainViewModel(string[] args)
        {
            BindingOperations.EnableCollectionSynchronization(Xmls, new object());
            Progress = new ProgressModel();
#if DEBUG
            Schema = new DocumentSchemaModel(@"C:\Users\ehirakawa\Source\Workspaces\Schematron Learning\testdata\tournament.iso\tournament-schema.sch");
#endif
        }
        public MainViewModel() : this(new string[] { }) { }
        #endregion
        #region Command Declare
        public ICommand FileDropCommand => new RelayCommand<object>(FileDropCommandExecute, FileDropCommandCanExecute);
        public ICommand SchemaSelectCommand => new RelayCommand(SchemaSelectCommandExecute, SchemaSelectCommandCanExecute);
        public ICommand XmlSelectCommand => new RelayCommand(XmlSelectCommandExecute, XmlSelectCommandCanExecute);
        public ICommand ValidationCommand => new RelayCommand(ValidationCommandExecute, ValidationCommandCanExecute);
        public ICommand SettingsCommand => new RelayCommand(SettingsCommandExecute, SettingsCommandCanExecute);
        public ICommand HelpCommand => new RelayCommand(HelpCommandExecute, HelpCommandCanExecute);
        public ICommand SchemaMessageViewCommand => new RelayCommand(SchemaMessageViewCommandExecute, SchemaMessageViewCommandCanExecute);
        public ICommand ReportViewCommand => new RelayCommand<object>(ReportViewCommandExecute, ReportViewCommandCanExecute);

        #region Command Executions
        bool FileDropCommandCanExecute(object param) => !Progress.IsProgress;
        void FileDropCommandExecute(object param)
        {
            string[] files = param as string[];
            if(files != null)
            {
                foreach(string file in files)
                {
                    if (!File.Exists(file))
                        continue;
                    string ext = Path.GetExtension(file).ToLower();
                    if (ext == ".sch")
                    {
                        Schema = new DocumentSchemaModel(file);
                    }
                    else if (ext == ".xml" && !Xmls.Exists(file))
                    {
                        Xmls.Add(file);
                    }
                }
            }
        }
        bool SchemaSelectCommandCanExecute() => !Progress.IsProgress;
        void SchemaSelectCommandExecute()
        {
            string file = ExDialogs.SelectionFile(Properties.Resources.ToolItemButtonChooseSch, new[] { ".sch" });
            if (File.Exists(file))
            {
                Schema = new DocumentSchemaModel(file);
            }
        }
        bool XmlSelectCommandCanExecute() => !Progress.IsProgress;
        void XmlSelectCommandExecute()
        {
            string[] files = ExDialogs.SelectionFiles(Properties.Resources.ToolItemButtonChooseXml, new[] { ".xml" });
            foreach (string file in files)
            {
                if (File.Exists(file) && !Xmls.Exists(file))
                {
                    Xmls.Add(file);
                }
            }
        }
        bool ValidationCommandCanExecute() => Schema != null && Xmls.Count > 0;
        async void ValidationCommandExecute()
        {
            if (!Progress.IsProgress)
            {
                await Task.Run(() => DoValidation());
            }
            else
            {
                if (ExDialogs.Question("中断しますか？"))
                {
                    Progress.Cancel();
                }
            }
        }
        bool SettingsCommandCanExecute() => !Progress.IsProgress;
        void SettingsCommandExecute()
        {

        }
        bool HelpCommandCanExecute() => true;
        void HelpCommandExecute()
        {

        }
        bool SchemaMessageViewCommandCanExecute() => Schema != null && !String.IsNullOrWhiteSpace(Schema.Message);
        void SchemaMessageViewCommandExecute()
        {
            ExDialogs.Warning(Schema.Message);
        }
        bool ReportViewCommandCanExecute(object param) => true;
        void ReportViewCommandExecute(object param)
        {
            DocumentXmlModel model = param as DocumentXmlModel;
            if(model != null)
            {
                Debug.Print("ReportViewCommandExecute: {0}", model.Name);
            }
        }
        #endregion
        #endregion
        #region Methods
        #region Validation
        private void DoValidation()
        {
            Progress.Start();
            try
            {
                int max = 1000;
                Thread.Sleep(3000);
                for (int i = 0; i <= max; i++)
                {
                    if (Progress.IsRequestCancel)
                        throw new OperationCanceledException();
                    Thread.Sleep(10);
                    Progress.SetValue(i, max);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Print("throw Canceled");
            }
            catch (Exception ex)
            {
                ExDialogs.Error(ex.Message);
            }
            Progress.End();
        }
        #endregion
        #endregion
    }
}
