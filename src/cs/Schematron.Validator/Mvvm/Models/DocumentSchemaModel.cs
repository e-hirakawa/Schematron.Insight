using Schematron.Insight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Schematron.Validator.Mvvm.Models
{
    public class DocumentSchemaModel : DocumentModel
    {
        #region Private Properties
        private SchemaDocument _doc = null;
        private Phase _selectedPhase;
        private ObservableCollection<Phase> _phases = new ObservableCollection<Phase>();
        #endregion
        #region Public Properties

        public Phase SelectedPhase
        {
            get { return _selectedPhase; }
            set
            {
                if (_selectedPhase != value)
                    Set(() => SelectedPhase, ref _selectedPhase, value);
            }
        }
        public ObservableCollection<Phase> Phases
        {
            get { return _phases; }
            private set
            {
                if (_phases != value)
                    Set(() => Phases, ref _phases, value);
            }
        }
        #endregion
        #region Constructor

        public DocumentSchemaModel(string filepath) : base(filepath)
        {
            BindingOperations.EnableCollectionSynchronization(Phases, new object());
        }
        #endregion
        #region Override Method
        public override void UpdateProperties()
        {
            base.UpdateProperties();
        }
        protected override void UpdateProperties(string filepath)
        {
            base.UpdateProperties(filepath);
            Loading();
        }
        private async void Loading()
        {
            if (File.Exists(FullPath))
            {
                if (_doc != null)
                    Close();

                await Task.Run(() =>
                {
                    Status = DocumentStatus.Loading;
                    try
                    {
                        _doc = new SchemaDocument();
                        _doc.Open(FullPath);
                        Phases.Add(new Phase() { Id = "ALL" });
                        foreach (Phase phase in _doc.Phases)
                            Phases.Add(phase);
                        //Phases = new ObservableCollection<Phase>(_doc.Phases);
                        //Phases.Insert(0, new Phase() { Id = "ALL" });

                        Status = DocumentStatus.LoadedCorrectly;
                    }
                    catch (Exception ex)
                    {
                        Status = DocumentStatus.LoadedFailure;
                        Message = ex.Message;
                    }
                });
            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion
        #region Original Methods
        public void Close()
        {
            SelectedPhase = null;
            Phases.Clear();
            _doc?.Dispose();
            _doc = null;
        }
        #endregion
    }
}
