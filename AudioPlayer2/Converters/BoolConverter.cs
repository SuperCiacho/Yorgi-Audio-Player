using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioPlayer2.Converters
{
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringParam = parameter as string;

            var boolVal = (bool?)value ?? false;
            var converted = value;

            if (targetType == typeof(bool))
            {
                if (stringParam == "Invert") converted = !boolVal;
            }
            else if (targetType == typeof(Visibility))
            {
                converted = boolVal ? Visibility.Visible : Visibility.Collapsed;
            }

            return converted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}