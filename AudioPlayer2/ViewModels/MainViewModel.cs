using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AudioPlayer2.Models;
using AudioPlayer2.Models.Audio;
using AudioPlayer2.Models.Playlist;
using AudioPlayer2.Models.Playlist.Readers;
using AudioPlayer2.Models.Playlist.Writers;
using AudioPlayer2.Models.Tag;
using AudioPlayer2.Properties;
using AudioPlayer2.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Ninject;
using FileInfo = AudioPlayer2.Views.FileInfo;

namespace AudioPlayer2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ITagManager tagManager, IPlaylist playlist)
        {
            this.tagManager = tagManager;
            this.playlist = playlist;
        }

        #region Fields

        private string status;
        private string time;
        private int currentTime;
        private int trackDuration;
        private int selectedTrackIndex;
        private Track selectedTrack;

        private bool isUserAction;

        private IAudioPlayer audioPlayer;
        private readonly ITagManager tagManager;
        private IPlaylist playlist;

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
        private ICommand sortPlaylistCommand;
        private ICommand loadPlaylistCommand;
        private ICommand savePlaylistCommand;
        private ICommand openLocationCommand;

        #endregion

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

        public ICommand InitializedCommand => this.initializedCommand ?? (this.initializedCommand = new RelayCommand(this.Initialize));

        public ICommand ShowFileInfoCommand => this.showFileInfoCommand ?? (this.showFileInfoCommand = new RelayCommand<Track>(this.OnShowFileInfoCommand));

        public ICommand OpenLocationCommand => this.openLocationCommand ?? (this.openLocationCommand = new RelayCommand<Track>(this.OnOpenLocationCommand));

        public ICommand SortPlaylistCommand => this.sortPlaylistCommand ?? (this.sortPlaylistCommand = new RelayCommand<SortPlaylistBy>(this.OnSortPlaylistCommand));

        public ICommand LoadPlaylistCommand => this.loadPlaylistCommand ?? (this.loadPlaylistCommand = new RelayCommand(this.OnLoadPlaylistCommand));

        public ICommand SavePlaylistCommand => this.savePlaylistCommand ?? (this.savePlaylistCommand = new RelayCommand(this.OnSavePlaylistCommand));

        #endregion

        [Inject]
        public IAudioPlayer AudioPlayer
        {
            get => this.audioPlayer;
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

        public ICollectionView Tracks { get; private set; }

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

                if (Math.Abs(this.AudioPlayer.Position.TotalSeconds - value) > 1)
                {
                    this.AudioPlayer.Position = TimeSpan.FromSeconds(value);
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

        public Track SelectedTrack
        {
            get => this.selectedTrack;
            set => this.Set(ref this.selectedTrack, value);
        }

        public int SelectedTrackIndex
        {
            get => this.selectedTrackIndex;
            set => this.Set(ref this.selectedTrackIndex, value);
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            this.AddToPlaylist(Directory.EnumerateFiles(@"G:\Muzyka", "*.*", SearchOption.AllDirectories));
            this.audioPlayer.Playlist = this.playlist;

            this.Tracks = CollectionViewSource.GetDefaultView(this.playlist.Tracks);
            this.RaisePropertyChanged(nameof(this.Tracks));
        }

        private void AddToPlaylist(IEnumerable<string> trackPaths)
        {
            var supportedFiles = Settings.Default.SupportedFiles;
            foreach (var track in trackPaths
                        .Where(p => !string.IsNullOrEmpty(p) && supportedFiles.Contains(Path.GetExtension(p)))
                        .Select(p => Track.Create(this.tagManager, p)))
            {
                this.playlist.Add(track);
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

            if (result.GetValueOrDefault())
            {
                this.AddToPlaylist(ofd.FileNames);
            }
        }

        private void OnPlayCommand(Track track)
        {
            if (track == null)
            {
                return;
            }

            if (this.AudioPlayer.IsPaused)
            {
                this.AudioPlayer.Resume();
                return;
            }

            this.isUserAction = true;
            this.playlist.CurrentTrack = track;
            this.AudioPlayer.Play();
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
            this.isUserAction = true;
            this.AudioPlayer.Stop();
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

        private void OnShowFileInfoCommand(Track track)
        {
            var param = new Dictionary<string, object> { { "track", track } };

            DialogManager.CreateDialog<FileInfo, FileInfoViewModel>(track.Uri, 500, 300, param).Show();
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
                this.SelectedTrackIndex++;
                this.AudioPlayer.Next();
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

        private void OnRemoveTrackCommand(RemoveTrackMode mode)
        {
            switch (mode)
            {
                case RemoveTrackMode.All:
                    this.playlist.Clear();
                    break;
                case RemoveTrackMode.Selected:
                    if (this.selectedTrack != null)
                    {
                        this.playlist.Remove(this.selectedTrack);
                    }
                    break;
                case RemoveTrackMode.Unselected:
                    var leaveIt = this.selectedTrack;
                    this.playlist.Clear();
                    if (leaveIt != null)
                    {
                        this.playlist.Add(this.selectedTrack);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            this.RaisePropertyChanged(nameof(this.Tracks));
        }

        private async void OnLoadPlaylistCommand()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "M3U|*.m3u;*.m3u8|All|*.*",
                Multiselect = false,
                CheckPathExists = true,
                CheckFileExists = true,
                ReadOnlyChecked = true,
                Title = "Open playlist"
            };

            if (!dialog.ShowDialog().GetValueOrDefault()) return;
            var loader = PlaylistLoader.GetLoader(dialog.FileName);
            var tracks = await loader.LoadAsync(this.tagManager);

            if (tracks.IsEmpty())
            {
                MessageBox.Show(Resources.Error_Playlist_InvalidOrEmpty, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            this.playlist.Clear();
            this.playlist.Add(tracks);
            this.RaisePropertyChanged(nameof(this.Tracks));
        }

        private void OnSavePlaylistCommand()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "M3U|*.m3u|M3U8|*.m3u8|All|*.*",
                CheckPathExists = true,
                CheckFileExists = false,
                Title = "Save playlist"
            };

            if (!dialog.ShowDialog().GetValueOrDefault()) return;
            var loader = PlaylistWriter.GetWriter(dialog.FileName);
            loader.WriteAsync(this.playlist.Tracks);
        }

        private void OnSortPlaylistCommand(SortPlaylistBy mode)
        {
            var propertyNameToSortBy = mode.ToString();
            var sortedBy = this.Tracks.SortDescriptions.FirstOrDefault();
            var descending = propertyNameToSortBy.Equals(sortedBy.PropertyName) && sortedBy.Direction == ListSortDirection.Ascending;
            sortedBy =
                new SortDescription(propertyNameToSortBy,
                    descending
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending);

            this.Tracks.SortDescriptions.Clear();
            this.Tracks.SortDescriptions.Add(sortedBy);
            this.playlist.SortAsync(mode, descending);
        }

        private void OnOpenLocationCommand(Track track)
        {
           Process.Start("explorer.exe", $"/select, \"{track.Uri}\"");
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