using Schematron.Validator.Mvvm.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Schematron.Validator.Utilities.Converter
{
    public class DocumentStatusToImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type t = typeof(DocumentStatus);
            if (!Enum.IsDefined(t, value))
                return null;

            DocumentStatus val = (DocumentStatus)Enum.Parse(t, value.ToString());
            string name = null;
            switch (val)
            {
                case DocumentStatus.Loading:
                    name = "StatusAnnotations_Play_16xLG_color";
                    break;
                case DocumentStatus.LoadedCorrectly:
                    name = "StatusAnnotations_Complete_and_ok_16xLG_color";
                    break;
                case DocumentStatus.LoadedFailure:
                    name = "StatusAnnotations_Warning_16xLG_color";
                    break;
                case DocumentStatus.Validating:
                    name = "StatusAnnotations_Play_16xLG_color";
                    break;
                case DocumentStatus.ValidatedNoInfo:
                    name = "StatusAnnotations_Complete_and_ok_16xLG_color";
                    break;
                case DocumentStatus.ValidatedHasInfo:
                    name = "StatusAnnotations_Warning_16xLG_color";
                    break;
            }
            return name != null ? $"/Schematron.Validator;component/Resources/{name}.png" : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
