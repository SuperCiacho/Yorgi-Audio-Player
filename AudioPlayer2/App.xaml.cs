using System;
using System.Windows;
using AudioPlayer2.Models;
using AudioPlayer2.Utils;
using AudioPlayer2.Views;
using GalaSoft.MvvmLight;

namespace AudioPlayer2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.OnStartup;
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            IoCContainer.Instance.Get<MainWindow>().Show();
        }
    }
}
