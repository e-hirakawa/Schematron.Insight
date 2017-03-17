using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Schematron.Validator.Mvvm.Models
{
    public class ProgressModel:ViewModelBase
    {
        #region Private Properties
        private ProgressButtonModel _button = new ProgressButtonModel();
        private int _value = 0;
        private bool _isVisible = false;
        private string _text = "";

        private CancellationTokenSource _cancelSource = null;


        #endregion
        #region Public Properties
        /// <summary>
        /// 進捗制御ボタンキャプション
        /// </summary>
        public ProgressButtonModel Button
        {
            get { return _button; }
            private set
            {
                if (_button != value)
                    Set(() => Button, ref _button, value);
            }
        }
        /// <summary>
        /// 進捗パーセント
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                    Set(() => Value, ref _value, value);
            }
        }
        /// <summary>
        /// 進捗最小値
        /// </summary>
        public  int Minimum => 0;
        /// <summary>
        /// 進捗最大値
        /// </summary>
        public int Maximum => 100;
        /// <summary>
        /// プログレスバー
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                    Set(() => IsVisible, ref _isVisible, value);
            }
        }
        /// <summary>
        /// 進捗通知テキスト
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                    Set(() => Text, ref _text, value);
            }
        }
        #endregion
        #region Constructor

        #endregion
        #region Methods

        /// <summary>
        /// プログレスバーの開始
        /// </summary>
        public void Start()
        {
            _cancelSource = new CancellationTokenSource();
            IsVisible = true;
            Button.IsProgress = true;
            SetValue(0, 0);
        }
        /// <summary>
        /// プログレスバーの終了
        /// </summary>
        public void End()
        {
            _cancelSource?.Dispose();
            _cancelSource = null;
            IsVisible = false;
            Button.IsProgress = false;
            SetValue(0, 0);
        }
        public void SetValue(int count, int total, string message = null)
        {
            string text = message ?? "";
            if (count < 0 || total <= 0 || count > total)
            {
                Value = 0;
            }
            else
            {
                Value = (int)Math.Round((double)count / total * 100) ;
                if (!String.IsNullOrWhiteSpace(Text))
                    text += "…";
                text += $"{count}/{total}({Value}%)";
            }
            Text = text;
        }
        /// <summary>
        /// 進行中の判定取得
        /// </summary>
        public bool DoProgress => _cancelSource != null;
        /// <summary>
        /// 中断指示
        /// </summary>
        public void Cancel() => _cancelSource?.Cancel();
        /// <summary>
        /// 取り消し要求確認
        /// </summary>
        public bool IsRequestCancel => _cancelSource?.Token.IsCancellationRequested ?? false;

        #endregion
    }
}
