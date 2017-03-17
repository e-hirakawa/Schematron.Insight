using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Schematron.Validator.Mvvm.Models;
using Schematron.Validator.Utilities;
using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<XmlResourceModel> _xmlFiles = new ObservableCollection<XmlResourceModel>();


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
        public ObservableCollection<XmlResourceModel> XmlFiles
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
        public ICommand SchemaSelectCommand => new RelayCommand(SchemaSelectCommandExecute, SchemaSelectCommandCanExecute);
        public ICommand ValidationCommand => new RelayCommand(ValidationCommandExecute, ValidationCommandCanExecute);
        #region Command Executions
        bool SchemaSelectCommandCanExecute() => true;
        void SchemaSelectCommandExecute()
        {
            string path = ExDialogs.SelectionFile("choose schema file(*.sch)", new[] { ".sch" });
            if (File.Exists(path))
            {
                SchemaFile = new SchResourceModel(path);
            }
        }
        bool ValidationCommandCanExecute() => true;
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
