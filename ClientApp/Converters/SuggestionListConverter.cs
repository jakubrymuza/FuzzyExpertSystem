using System;
using System.Globalization;
using System.Windows.Data;

namespace ClientApp.Converters
{
    internal class SuggestionListConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var name = values[0].ToString();
            double val;

            if (values[1] is IConvertible)
            {
                val = System.Convert.ToDouble(values[1]);
            }
            else {
                val = 0;
            }

            int p = (int)(val * 100);
            return name + " " + p.ToString() + " %";
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) { 
            throw new NotImplementedException();
        }

    }
}
