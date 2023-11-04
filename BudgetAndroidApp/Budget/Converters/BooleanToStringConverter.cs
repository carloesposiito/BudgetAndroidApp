using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Budget.Converters
{
    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // Converte il valore booleano in una stringa
                return boolValue ? "Nascondi" : "Mostra";
            }
            return string.Empty; // Ritorna una stringa vuota se il valore non è booleano
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException(); // Non implementato in questo caso
        }
    }
}
