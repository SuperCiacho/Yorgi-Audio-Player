using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AudioPlayer2.Behaviors;

namespace AudioPlayer2.Converters
{
    internal class DataGridRowBackgroundSelector : IMultiValueConverter
    { 
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2) 
            {
                return false;
            }

            var rowIndex = PlaylistBehaviours.GetRowIndex(values[1] as DependencyObject);
            return rowIndex.Equals(values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}