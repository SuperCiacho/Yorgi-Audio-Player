using System;
using System.Windows.Controls;
using AudioPlayer2.Models;
using AudioPlayer2.ViewModels;

namespace AudioPlayer2.Views
{
    /// <summary>
    /// Interaction logic for FileInfo.xaml
    /// </summary>
    public partial class FileInfo : UserControl, IView
    {
        public FileInfo(FileInfoViewModel dataContext)
        {
            this.DataContext = dataContext;
            this.InitializeComponent();
        }

        public Type ViewModelType => this.DataContext.GetType();
    }
}
