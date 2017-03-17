using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Schematron.Validator.Utilities.Converter
{
    /// <summary>
    /// 真偽値から表示の設定
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="targetType">対象の型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="culture">カルチャー</param>
        /// <returns>Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="targetType">対象の型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="culture">カルチャー</param>
        /// <returns>boolean</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }
}
