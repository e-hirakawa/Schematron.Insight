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
        private SchResourceModel _schemaFile;
        private XmlResourceCollection _xmlFiles = new XmlResourceCollection();
        private XmlResourceModel _selectedXmlFile;



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

        public SchResourceModel SchemaFile
        {
            get { return _schemaFile; }
            set
            {
                if (_schemaFile != value)
                {
                    Set(() => SchemaFile, ref _schemaFile, value);
                }
            }
        }
        public XmlResourceCollection XmlFiles
        {
            get { return _xmlFiles; }
            set
            {
                if (_xmlFiles != value)
                {
                    Set(() => XmlFiles, ref _xmlFiles, value);
                }
            }
        }
        public XmlResourceModel SelectedXmlFile
        {
            get { return _selectedXmlFile; }
            set {
                Set(() => SelectedXmlFile, ref _selectedXmlFile, value);
            }
        }
        #endregion
        #region Constructors
        public MainViewModel(string[] args)
        {
            BindingOperations.EnableCollectionSynchronization(XmlFiles, new object());
            Progress = new ProgressModel();
            Progress.Button.ExecutorCaption = "Validation";
            Progress.Button.CancellerCaption = "Cancel";

        }
        public MainViewModel() : this(new string[] { }) { }
        #endregion
        #region Command Declare
        /// <summary>
        /// Choose Schematron File
        /// </summary>
        public ICommand SchFileSelectCommand => new RelayCommand(SchFileSelectCommandExecute, SchFileSelectCommandCanExecute);
        public ICommand XmlFilesSelectCommand => new RelayCommand(XmlFilesSelectCommandExecute, XmlFilesSelectCommandCanExecute);
        public ICommand ValidationCommand => new RelayCommand(ValidationCommandExecute, ValidationCommandCanExecute);
        public ICommand ReportViewCommand => new RelayCommand<object>(ReportViewCommandExecute, ReportViewCommandCanExecute);
        #region Command Executions
        bool SchFileSelectCommandCanExecute() => !Progress.DoProgress;
        void SchFileSelectCommandExecute()
        {
            string file = ExDialogs.SelectionFile("choose schema file(*.sch)", new[] { ".sch" });
            if (File.Exists(file))
            {
                SchemaFile = new SchResourceModel(file);
            }
        }
        bool XmlFilesSelectCommandCanExecute() => !Progress.DoProgress;
        void XmlFilesSelectCommandExecute()
        {
            string[] files = ExDialogs.SelectionFiles("choose xml file(*.xml)", new[] { ".xml" });
            foreach (string file in files)
            {
                if (File.Exists(file) && !XmlFiles.Exists(file))
                {
                    XmlFiles.Add(file);
                }
            }
        }
        bool ValidationCommandCanExecute() => SchemaFile != null && XmlFiles.Count > 0;
        async void ValidationCommandExecute()
        {
            if (!Progress.DoProgress)
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
        bool ReportViewCommandCanExecute(object param) => true;
        void ReportViewCommandExecute(object param)
        {
            XmlResourceModel model = param as XmlResourceModel;
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
