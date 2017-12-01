using AudioPlayer2.Models;
using AudioPlayer2.Models.Tag;
using GalaSoft.MvvmLight;

namespace AudioPlayer2.ViewModels
{
    public class FileInfoViewModel : ViewModelBase
    {
       private ITaggedFile file;

        public FileInfoViewModel(Track track)
        {
            this.file = track.Tag;
        }

        public string Title => this.file.Path;

        public ITaggedFile File
        {
            get { return this.file; }
            set
            {
                this.file = value; 
                this.RaisePropertyChanged();
            }
        }
    }
}