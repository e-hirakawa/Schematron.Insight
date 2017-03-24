using Schematron.Validator.Mvvm.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static Schematron.Validator.Properties.Resources;

namespace Schematron.Validator.Utilities.Converter
{
    public class DocumentStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type t = typeof(DocumentStatus);
            if (!Enum.IsDefined(t, value))
                return DependencyProperty.UnsetValue;

            DocumentStatus val = (DocumentStatus)Enum.Parse(t, value.ToString());
            string str = DocumentStatusNone;
            switch (val)
            {
                case DocumentStatus.Loading:
                    str = DocumentStatusLoading;
                    break;
                case DocumentStatus.LoadedCorrectly:
                    str = DocumentStatusLoadedCorrectly;
                    break;
                case DocumentStatus.LoadedFailure:
                    str = DocumentStatusLoadedFailure;
                    break;
                case DocumentStatus.Validating:
                    str = DocumentStatusValidating;
                    break;
                case DocumentStatus.ValidatedNoInfo:
                    str = DocumentStatusValidatedNoInfo;
                    break;
                case DocumentStatus.ValidatedHasInfo:
                    str = DocumentStatusValidatedHasInfo;
                    break;
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
