using Schematron.Insight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Validator.Mvvm.Models
{
    public class SchResourceModel : ResourceModel
    {
        #region Private Properties
        private SchemaDocument _doc = null;
        private Phase _selectedPhase;
        private ObservableCollection<Phase> _phases;
        private bool _isValid = false;




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
        public bool IsValid
        {
            get { return _isValid; }
            private set
            {
                if (_isValid != value)
                    Set(() => IsValid, ref _isValid, value);
            }
        }
        #endregion
        #region Constructor

        public SchResourceModel(string filepath) : base(filepath)
        {
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
            if (File.Exists(filepath))
            {
                if (_doc != null)
                    Close();
                try
                {
                    _doc = new SchemaDocument();
                    _doc.Open(filepath);
                    Phases = new ObservableCollection<Phase>(_doc.Phases);
                    Phases.Insert(0, new Phase() { Id = "ALL" });

                    IsValid = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
            IsValid = false;
        }
        #endregion
    }
}
