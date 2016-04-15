using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioPlayer2.Models;
using AudioPlayer2.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Ninject;

namespace AudioPlayer2.ViewModels
{
    public class AudioPlayerControlViewModel : ViewModelBase
    {
        #region Fields

        private string status;
        private string time;
        private int currentTime;
        private int trackDuration;
        private Track currentTrack;
        private List<Track> tracks;

        private bool isStoppedManually;

        private IAudioPlayer audioPlayer;

        private ICommand playCommand;
        private ICommand pauseCommand;
        private ICommand stopCommand;
        private ICommand previousCommand;
        private ICommand nextCommand;
        private ICommand openCommand;
        private ICommand updateStatusCommand;

        #endregion

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

        [Inject]
        public IAudioPlayer AudioPlayer
        {
            get { return this.audioPlayer; }
            set
            {
                if (this.audioPlayer != null)
                {
                    this.AudioPlayer.ProgressChanged -= OnTrackProgressChanged;
                    this.AudioPlayer.PlaybackStopped -= OnPlaybackStopped;
                }

                this.audioPlayer = value;

                this.AudioPlayer.ProgressChanged += OnTrackProgressChanged;
                this.AudioPlayer.PlaybackStopped += OnPlaybackStopped;

                this.InitPlaylist();
            }
        }

        public List<Track> Tracks => this.tracks;

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

        #endregion

        #region Methods

        public void OnOpenCommand()
        {
            var ofd = new OpenFileDialog
            {
                Multiselect = true,
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
            };

            var result = ofd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                foreach (var path in ofd.FileNames)
                {
                    this.AudioPlayer.AddTrack(path);
                    this.tracks.Add(new Track(path));
                }

                this.RaisePropertyChanged(nameof(this.Tracks));
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
                AudioPlayer.Pause();
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
                this.OnNextCommand();
            }
        }

        private void OnUpdateStatusCommand(float value)
        {
            Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith(a => this.Status = this.currentTrack.Name);
        }

        private void UpdateUIState()
        {
            var path = this.audioPlayer.CurrentTrack;
            this.currentTrack = this.tracks.FirstOrDefault(x => x.Uri.Equals(path)) ?? new Track(path);
            this.Status = this.currentTrack.Name;
            this.TrackDuration = (int)this.currentTrack.Duration.TotalSeconds;
        }

        public override void Cleanup()
        {
            base.Cleanup();

            this.audioPlayer.Dispose();
            this.audioPlayer.ProgressChanged -= OnTrackProgressChanged;
            this.audioPlayer.PlaybackStopped -= OnPlaybackStopped;
        }

        #endregion

    }
}
