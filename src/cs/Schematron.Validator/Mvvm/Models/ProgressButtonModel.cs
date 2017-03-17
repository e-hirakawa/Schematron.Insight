using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schematron.Validator.Mvvm.Models
{
    /// <summary>
    /// 進捗制御ボタンクラス
    /// </summary>
    public class ProgressButtonModel : ViewModelBase
    {
        #region Private Properties
        private string _caption;
        private string _executorCaption;
        private string _cancellerCaption;
        private bool _isProgress;
        #endregion
        #region public Properties
        /// <summary>
        /// 進捗制御ボタンキャプション
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            private set {
                if (_caption != value)
                    Set(() => Caption, ref _caption, value);
            }
        }
        /// <summary>
        /// 実行時ボタンキャプション
        /// </summary>
        public string ExecutorCaption
        {
            get { return _executorCaption; }
            set
            {
                if (_executorCaption != value) { 
                    Set(() => ExecutorCaption, ref _executorCaption, value);
                    SetCaption();
                }
            }
        }
        /// <summary>
        /// 中断時ボタンキャプション
        /// </summary>
        public string CancellerCaption
        {
            get { return _cancellerCaption; }
            set
            {
                if (_cancellerCaption != value)
                {
                    Set(() => CancellerCaption, ref _cancellerCaption, value);
                    SetCaption();
                }
            }
        }
        /// <summary>
        /// 進捗判定
        /// </summary>
        public bool IsProgress
        {
            get { return _isProgress; }
            set
            {
                if (_isProgress != value)
                {
                    Set(() => IsProgress, ref _isProgress, value);
                    SetCaption();
                }
            }
        }
        #endregion
        #region Constructors
        public ProgressButtonModel(string executorCaption, string cancellerCaption)
        {
            ExecutorCaption = executorCaption;
            CancellerCaption = cancellerCaption;
        }
        public ProgressButtonModel() :this(null, null) { }
        #endregion
        #region Methods
        private void SetCaption()
        {
            Caption = IsProgress ? CancellerCaption : ExecutorCaption;
        }
        #endregion
    }
}
