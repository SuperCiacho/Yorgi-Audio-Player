using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioPlayer2.Models;
using AudioPlayer2.Models.Audio;
using AudioPlayer2.Models.Playlist;
using AudioPlayer2.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

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

        private bool isUserAction;

        private readonly IPlaylist playlist;
        private readonly IAudioPlayer audioPlayer;

        private ICommand playCommand;
        private ICommand pauseCommand;
        private ICommand stopCommand;
        private ICommand previousCommand;
        private ICommand nextCommand;
        private ICommand openCommand;
        private ICommand updateStatusCommand;

        #endregion

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAudioPlayer audioPlayer, IPlaylist playlist)
        {
            this.playlist = playlist;
            this.playlist.CurrentTrackChanged += this.OnPlaylistCurrentTrackChanged;

            this.audioPlayer = audioPlayer;
            this.audioPlayer.Playlist = playlist;
            this.audioPlayer.ProgressChanged += this.OnTrackProgressChanged;
            this.audioPlayer.PlaybackStopped += this.OnPlaybackStopped;
        }

        #region Properties

        #region Commands

        public ICommand PlayCommand => this.playCommand ?? (this.playCommand = new RelayCommand<Track>(this.OnPlayCommand));
        public ICommand PauseCommand => this.pauseCommand ?? (this.pauseCommand = new RelayCommand(this.OnPauseCommand));
        public ICommand StopCommand => this.stopCommand ?? (this.stopCommand = new RelayCommand(this.OnStopCommand));
        public ICommand PreviousCommand => this.previousCommand ?? (this.previousCommand = new RelayCommand(this.OnPreviousCommand));
        public ICommand NextCommand => this.nextCommand ?? (this.nextCommand = new RelayCommand(this.OnNextCommand));
        public ICommand OpenCommand => this.openCommand ?? (this.openCommand = new RelayCommand(this.OnOpenCommand));
        public ICommand UpdateStatusCommand => this.updateStatusCommand ?? (this.updateStatusCommand = new RelayCommand<float>(this.OnUpdateStatusCommand));

        #endregion

        public string Time
        {
            get => this.time;
            set => this.Set(ref this.time, value);
        }

        public string Status
        {
            get => this.status;
            set => this.Set(ref this.status, value);
        }

        public int CurrentTime
        {
            get => this.currentTime;
            set
            {
                this.Set(ref this.currentTime, value);

                if (Math.Abs(this.audioPlayer.Position.TotalSeconds - value) > 1)
                {
                    this.audioPlayer.Position = TimeSpan.FromSeconds(value);
                }
            }
        }

        public float Volume
        {
            get => this.audioPlayer.Volume;
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
            get => this.trackDuration;
            set => this.Set(ref this.trackDuration, value);
        }

        public Track CurrentTrack => this.playlist.CurrentTrack;

        public int CurrentTrackIndex => this.playlist.CurrentTrackIndex;

        #endregion

        #region Methods

        private void OnPlaylistCurrentTrackChanged(object sender, Track track)
        {
            this.OnPlayCommand(track);
        }

        private void OnPlayCommand(Track track)
        {
            if (track == null)
            {
                return;
            }

            if (this.audioPlayer.IsPaused)
            {
                this.audioPlayer.Resume();
                return;
            }

            this.isUserAction = true;
            this.audioPlayer.Play();
            this.UpdateUIState();
        }

        private void OnPauseCommand()
        {
            if (this.audioPlayer.IsPlaying)
            {
                this.audioPlayer.Pause();
            }
            else if (this.audioPlayer.IsPaused)
            {
                this.audioPlayer.Resume();
            }
        }

        private void OnStopCommand()
        {
            this.isUserAction = true;
            this.audioPlayer.Stop();
        }

        private void OnNextCommand()
        {
            this.isUserAction = true;
            this.audioPlayer.Next();
            this.UpdateUIState();
        }

        private void OnPreviousCommand()
        {
            this.isUserAction = true;
            this.audioPlayer.Previous();
            this.UpdateUIState();
        }

        private void OnOpenCommand()
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

            if (result.GetValueOrDefault())
            {
               // this.AddToPlaylist(ofd.FileNames);
            }
        }

        private void OnTrackProgressChanged(object sender, TrackProgressChangedArgs args)
        {
            this.CurrentTime = (int)args.Position.TotalSeconds;
            this.TrackDuration = (int)args.TrackLength.TotalSeconds;
            this.Time = args.Position.ToString(Resources.TimeFormat);
        }

        private void OnPlaybackStopped(object sender, EventArgs e)
        {
            if (this.isUserAction)
            {
                this.isUserAction = false;
                return;
            }

            if (this.playlist.Size < 2)
            {
                this.CurrentTime = 0;
                this.TrackDuration = 1;
            }
            else
            {
                // this.SelectedTrackIndex++;
                this.audioPlayer.Next();
            }
        }

        private void OnUpdateStatusCommand(float value)
        {
            Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith(a => this.Status = this.CurrentTrack?.Name);
        }

        private void UpdateUIState()
        {
            this.Status = this.CurrentTrack.Name;
            this.TrackDuration = (int)this.CurrentTrack.Duration.TotalSeconds;
            this.RaisePropertyChanged(nameof(this.CurrentTrackIndex));
            this.RaisePropertyChanged(nameof(this.CurrentTrack));
        }

        public override void Cleanup()
        {
            base.Cleanup();

            this.audioPlayer.ProgressChanged -= this.OnTrackProgressChanged;
            this.audioPlayer.PlaybackStopped -= this.OnPlaybackStopped;
            this.audioPlayer.Dispose();
        }

        #endregion
    }
}