using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioPlayer2.Converters
{
    internal class EqualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null && parameter == null) || (value?.Equals(parameter) ?? false);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}