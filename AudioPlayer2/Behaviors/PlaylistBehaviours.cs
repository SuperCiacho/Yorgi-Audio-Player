using System;
using System.Windows;
using System.Windows.Controls;

namespace AudioPlayer2.Behaviors
{
    public static class PlaylistBehaviours
    {
        public static readonly DependencyProperty RowNumberingProperty = DependencyProperty.RegisterAttached("RowNumbering", typeof (bool), typeof (PlaylistBehaviours), new PropertyMetadata(default(bool), OnRowNumberingChanged));
        public static readonly DependencyProperty RowIndexProperty = DependencyProperty.RegisterAttached("RowIndex", typeof (int), typeof (PlaylistBehaviours), new PropertyMetadata(-1));

        public static void SetRowIndex(DependencyObject element, int value)
        {
            element.SetValue(RowIndexProperty, value);
        }

        public static int GetRowIndex(DependencyObject element)
        {
            return (int) element.GetValue(RowIndexProperty);
        }
        
        public static void SetRowNumering(DependencyObject element, bool value)
        {
            element.SetValue(RowNumberingProperty, value);
        }

        public static bool GetRowNumering(DependencyObject element)
        {
            return (bool) element.GetValue(RowNumberingProperty);
        }

        private static void OnRowNumberingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var dataGrid = dependencyObject as DataGrid;

            if (dataGrid == null) { return; }

            if ((bool) args.NewValue)
            {
                dataGrid.LoadingRow += DataGrid_OnLoadingRow;
            }
            else
            {
                dataGrid.LoadingRow += DataGrid_OnLoadingRow;
            }
        }

        private static void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var index = e.Row.GetIndex();
            e.Row.Header = $"{index + 1}.";
            e.Row.SetValue(PlaylistBehaviours.RowIndexProperty, index);
        }
    }
}