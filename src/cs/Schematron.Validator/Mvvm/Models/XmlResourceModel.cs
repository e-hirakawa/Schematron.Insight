using Schematron.Insight.Validation;

namespace Schematron.Validator.Mvvm.Models
{
    public class XmlResourceModel : ResourceModel
    {
        #region Private Properties
        private ResultStatus _resultStatus = ResultStatus.None;
        #endregion
        #region Public Properties
        public ResultStatus ResultStatus
        {
            get { return _resultStatus; }
            set
            {
                if (_resultStatus != value)
                    Set(() => ResultStatus, ref _resultStatus, value);
            }
        }
        #endregion
        #region Constructor
        public XmlResourceModel(string path): base(path) { }
        public XmlResourceModel() : this(null) { }
        #endregion
    }
}
