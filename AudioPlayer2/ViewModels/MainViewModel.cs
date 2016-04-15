using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AudioPlayer2.Models;
using AudioPlayer2.Properties;
using AudioPlayer2.Utils;
using AudioPlayer2.ViewModels;
using AudioPlayer2.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Ninject;

namespace AudioPlayer2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private string status;
        private string time;
        private int currentTime;
        private int trackDuration;
        private int selectedTrackIndex;
        private Track selectedTrack;
        private Track currentTrack;

        private bool isStoppedManually;

        private ICommand playCommand;
        private ICommand pauseCommand;
        private ICommand stopCommand;
        private ICommand previousCommand;
        private ICommand nextCommand;
        private ICommand openCommand;
        private ICommand updateStatusCommand;
        private ICommand initializedCommand;
        private ICommand showFileInfoCommand;
        private ICommand removeTrackCommand;
        private IAudioPlayer audioPlayer;

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.Tracks = new ObservableCollection<Track>();

            if (this.IsInDesignMode || IsInDesignModeStatic)
            {
                for (int i = 0; i < 20;)
                {
                    this.Tracks.Add(new Track("Track " + ++i));
                }
            }
        }

        #region Properties

        #region Commands

        public ICommand PlayCommand => this.playCommand ?? (this.playCommand = new RelayCommand<Track>(this.OnPlayCommand));

        public ICommand PauseCommand => this.pauseCommand ?? (this.pauseCommand = new RelayCommand(this.OnPauseCommand));

        public ICommand StopCommand => this.stopCommand ?? (this.stopCommand = new RelayCommand(this.OnStopCommand));

        public ICommand PreviousCommand => this.previousCommand ?? (this.previousCommand = new RelayCommand(this.OnPreviousCommand));

        public ICommand NextCommand => this.nextCommand ?? (this.nextCommand = new RelayCommand(this.OnNextCommand));

        public ICommand OpenCommand => this.openCommand ?? (this.openCommand = new RelayCommand(this.OnOpenCommand));

        public ICommand RemoveTrackCommand => this.removeTrackCommand ?? (this.removeTrackCommand = new RelayCommand<RemoveTrackMode>(this.OnRemoveTrackCommand));

        public ICommand UpdateStatusCommand => this.updateStatusCommand ?? (this.updateStatusCommand = new RelayCommand<float>(this.OnUpdateStatusCommand));

        public ICommand InitializedCommand => this.initializedCommand ?? (this.initializedCommand = new RelayCommand(this.InitPlaylist));

        public ICommand ShowFileInfoCommand => this.showFileInfoCommand ?? (this.showFileInfoCommand = new RelayCommand<Track>(this.OnShowFileInfoCommand));

        #endregion

        [Inject]
        public IAudioPlayer AudioPlayer
        {
            get { return this.audioPlayer; }
            set
            {
                if (this.audioPlayer != null)
                {
                    this.AudioPlayer.ProgressChanged -= this.OnTrackProgressChanged;
                    this.AudioPlayer.PlaybackStopped -= this.OnPlaybackStopped;
                }

                this.audioPlayer = value;

                this.AudioPlayer.ProgressChanged += this.OnTrackProgressChanged;
                this.AudioPlayer.PlaybackStopped += this.OnPlaybackStopped;
            }
        }

        [Inject]
        public ITagManager TagManager { get; set; }

        public ObservableCollection<Track> Tracks { get; }

        public string Time
        {
            get { return this.time; }
            set { this.Set(ref this.time, value); }
        }

        public string Status
        {
            get { return this.status; }
            set { this.Set(ref this.status, value); }
        }

        public int CurrentTime
        {
            get { return this.currentTime; }
            set
            {
                this.Set(ref this.currentTime, value);

                if (Math.Abs(this.AudioPlayer.Position.TotalSeconds - value) > 1)
                {
                    this.AudioPlayer.Position = TimeSpan.FromSeconds(value);
                }
            }
        }

        public float Volume
        {
            get { return this.audioPlayer.Volume; }
            set
            {
                this.audioPlayer.Volume = value;
                var percentValue = value < 0.01 ? 0 : Math.Round(value * 100, 0);
                this.Status = string.Format(Resources.VolumeFormat, percentValue);
                this.RaisePropertyChanged();
            }
        }

        public int TrackDuration
        {
            get { return this.trackDuration; }
            set { this.Set(ref this.trackDuration, value); }
        }

        public Track CurrentTrack
        {
            get { return this.currentTrack; }
            set { this.Set(ref this.currentTrack, value); }
        }

        public Track SelectedTrack
        {
            get { return this.selectedTrack; }
            set { this.Set(ref this.selectedTrack, value); }
        }

        public int SelectedTrackIndex
        {
            get { return this.selectedTrackIndex; }
            set { this.Set(ref this.selectedTrackIndex, value); }
        }

        public int CurrentTrackIndex => this.audioPlayer.CurrentTrackIndex;

        #endregion

        #region Methods

        private void InitPlaylist()
        {
            var supported = Settings.Default.SupportedFiles;
            foreach (var path in System.IO.Directory.EnumerateFiles(@"F:\mp3\Całe Albumy\Bonobo - Black Sands")
                .Where(p => !string.IsNullOrEmpty(p) && supported.Contains(System.IO.Path.GetExtension(p))))
            {
                this.Tracks.Add(new Track(this.TagManager, path));
                this.AudioPlayer.AddTrack(path);
            }
        }

        public void OnOpenCommand()
        {
            var ofd = new OpenFileDialog
            {
                Multiselect = true,
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Resources.SupportedFilesFilter
            };

            var result = ofd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var ioc = IoCContainer.Instance;

                foreach (var path in ofd.FileNames)
                {
                    this.AudioPlayer.AddTrack(path);
                    this.Tracks.Add(ioc.Get<Track>(null, new[] { ioc.CreateConstructorArgument("path", path) }));
                }
            }
        }

        private void OnPlayCommand(Track track)
        {
            if (this.AudioPlayer.IsPlaying &&
                this.SelectedTrack.Uri.Equals(this.AudioPlayer.CurrentTrack))
            {
                this.AudioPlayer.Pause();
                return;
            }

            if (this.AudioPlayer.IsPaused)
            {
                this.AudioPlayer.Resume();
                return;
            }

            this.AudioPlayer.Play(track?.Uri);
            this.UpdateUIState();
        }

        private void OnPauseCommand()
        {
            if (this.AudioPlayer.IsPlaying)
            {
                this.AudioPlayer.Pause();
            }
            else if (this.AudioPlayer.IsPaused)
            {
                this.AudioPlayer.Resume();
            }
        }

        private void OnStopCommand()
        {
            this.isStoppedManually = true;
            this.AudioPlayer.Stop();
        }

        private void OnNextCommand()
        {
            this.audioPlayer.Next();
            this.UpdateUIState();
        }

        private void OnPreviousCommand()
        {
            this.audioPlayer.Previous();
            this.UpdateUIState();
        }

        private void OnShowFileInfoCommand(Track track)
        {
            var param = new Dictionary<string, object>()
                        {
                            { "track", track }
                        };

            DialogManager.CreateDialog<FileInfo, FileInfoViewModel>(track.Uri, 500, 300, param).Show();
        }

        private void OnTrackProgressChanged(object sender, ProgressChangedArgs args)
        {
            this.CurrentTime = (int)args.NewPosition.TotalSeconds;
            this.TrackDuration = (int)args.TrackLength.TotalSeconds;
            this.Time = args.NewPosition.ToString(Resources.TimeFormat);
        }

        private void OnPlaybackStopped(object sender, EventArgs e)
        {
            if (this.isStoppedManually)
            {
                this.isStoppedManually = false;
                return;
            }

            if (this.AudioPlayer.TrackCount < 2)
            {
                this.CurrentTime = 0;
                this.TrackDuration = 1;
            }
            else
            {
                this.SelectedTrackIndex++;
                this.AudioPlayer.Next();
            }
        }

        private void OnUpdateStatusCommand(float value)
        {
            Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith(a => this.Status = this.currentTrack?.Name);
        }

        private void UpdateUIState()
        {
            var path = this.audioPlayer.CurrentTrack;
            this.CurrentTrack = this.Tracks.FirstOrDefault(x => x.Uri.Equals(path)) ?? new Track(this.TagManager, path);
            this.Status = this.currentTrack.Name;
            this.TrackDuration = (int)this.currentTrack.Duration.TotalSeconds;
            this.RaisePropertyChanged(nameof(this.CurrentTrackIndex));
        }
        private void OnRemoveTrackCommand(RemoveTrackMode mode)
        {
            switch (mode)
            {
                case RemoveTrackMode.All:
                    this.Tracks.Clear();
                    break;
                case RemoveTrackMode.Selected:
                    if (this.selectedTrack != null)
                    {
                        this.Tracks.Remove(this.selectedTrack);
                    }
                    break;
                case RemoveTrackMode.Unselected:
                    var leaveIt = this.selectedTrack;
                    this.Tracks.Clear();
                    this.Tracks.Add(leaveIt);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();

            this.audioPlayer.Dispose();
            this.audioPlayer.ProgressChanged -= this.OnTrackProgressChanged;
            this.audioPlayer.PlaybackStopped -= this.OnPlaybackStopped;
        }

        #endregion

    }
}