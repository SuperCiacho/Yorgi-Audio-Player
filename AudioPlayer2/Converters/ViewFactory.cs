using System;
using System.Globalization;
using System.Windows.Data;
using AudioPlayer2.Utils;

namespace AudioPlayer2.Converters
{
    public sealed class ViewFactory : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IoCContainer.Instance.Get(value as Type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
