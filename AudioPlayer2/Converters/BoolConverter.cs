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

            var boolVal = (bool?) value ?? false;

            switch (stringParam)
            {
                case "Invert": return !boolVal;
                case "Vis": return boolVal ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return boolVal;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}