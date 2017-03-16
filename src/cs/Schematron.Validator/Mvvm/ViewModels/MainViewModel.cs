using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Schematron.Validator.Mvvm.Models;
using Schematron.Validator.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Schematron.Validator.Mvvm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Properties
        private ResourceModel _schemaFile;
        private ObservableCollection<ResourceModel> _xmlFiles = new ObservableCollection<ResourceModel>();
        
        #endregion
        #region Public Properties
        public string Title => $"{ApplicationInfo.Name} -ver.{ApplicationInfo.Version}";
        
        public ResourceModel SchemaFile
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
        public ObservableCollection<ResourceModel> XmlFiles
        {
            get { return _xmlFiles; }
            set
            {
                if(_xmlFiles != value)
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
        }
        public MainViewModel() : this(new string[] { }) { }
        #endregion
        #region Command Declare
        public ICommand SchemaSelectCommand => new RelayCommand(SchemaSelectCommandExecute, SchemaSelectCommandCanExecute);
        #region Command Executions
        bool SchemaSelectCommandCanExecute() => true;
        void SchemaSelectCommandExecute()
        {
            XmlFiles.Clear();
            foreach(string file in Directory.EnumerateFiles(@"../../../../../testdata/"))
            {
                XmlFiles.Add(new ResourceModel(file));
            }
        }

        #endregion
        #endregion
        #region Methods

        #endregion
    }
}
