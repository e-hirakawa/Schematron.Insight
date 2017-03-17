using Schematron.Insight.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
