using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Schematron.Validator.Utilities.Converter
{
    /// <summary>
    /// 列挙体と真偽値の変換クラス
    /// </summary>
    public class EnumBooleanConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="targetType">対象の型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="culture">カルチャー</param>
        /// <returns>真偽値</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = parameter as string;
            if (str == null)
                return DependencyProperty.UnsetValue;

            if (!Enum.IsDefined(value.GetType(), value))
                return DependencyProperty.UnsetValue;

            object obj = Enum.Parse(value.GetType(), str);

            return obj.Equals(value);
        }
        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="targetType">対象の型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="culture">カルチャー</param>
        /// <returns>真偽値</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = parameter as string;
            return (str == null) ? DependencyProperty.UnsetValue : Enum.Parse(targetType, str);
        }
        #endregion
    }
}
