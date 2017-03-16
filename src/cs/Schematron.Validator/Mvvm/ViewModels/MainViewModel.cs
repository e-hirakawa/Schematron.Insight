using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Schematron.Validator.Mvvm.Models;
using Schematron.Validator.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schematron.Validator.Mvvm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Properties
        private ResourceModel _schemaFile;

        public ResourceModel SchemaFile
        {
            get { return _schemaFile; }
            set
            {
                if (_schemaFile != value)
                {
                    _schemaFile = value;
                    RaisePropertyChanged(() => SchemaFile);
                }
            }
        }

        #endregion
        #region Public Properties
        public string Title => $"{ApplicationInfo.Name} -ver.{ApplicationInfo.Version}";
        #endregion
        #region Constructors
        public MainViewModel(string[] args)
        {

        }
        public MainViewModel() : this(new string[] { }) { }
        #endregion
        #region Command Declare
        public ICommand SchemaSelectCommand => new RelayCommand(SchemaSelectCommandExecute, SchemaSelectCommandCanExecute);
        #region Command Executions
        bool SchemaSelectCommandCanExecute() => true;
        void SchemaSelectCommandExecute()
        {
            SchemaFile = new ResourceModel(@"C:\Users\ehirakawa\Downloads\ky964216546910007811.pdf");
        }

        #endregion
        #endregion
        #region Methods

        #endregion
    }
}
