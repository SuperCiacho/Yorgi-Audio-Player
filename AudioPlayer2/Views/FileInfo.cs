using System.Windows.Controls;
using AudioPlayer2.ViewModels;

namespace AudioPlayer2.Views
{
    /// <summary>
    /// Interaction logic for FileInfo.xaml
    /// </summary>
    public partial class FileInfo : UserControl
    {
        public FileInfo(FileInfoViewModel dataContext)
        {
            this.DataContext = dataContext;
            this.InitializeComponent();
        }
    }
}
