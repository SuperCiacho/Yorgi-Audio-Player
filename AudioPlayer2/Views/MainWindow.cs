using System.ComponentModel;
using System.Windows;
using AudioPlayer2.ViewModels;
using GalaSoft.MvvmLight;

namespace AudioPlayer2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.Closing += this.OnClosing;
            this.InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            var vm = this.DataContext as ICleanup;
            vm?.Cleanup();
        }
    }
}
