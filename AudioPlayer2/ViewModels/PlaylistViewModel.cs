using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AudioPlayer2.Models;
using AudioPlayer2.Models.Playlist;
using AudioPlayer2.Models.Playlist.Readers;
using AudioPlayer2.Models.Playlist.Writers;
using AudioPlayer2.Models.Properties;
using AudioPlayer2.Models.Tag;
using AudioPlayer2.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using FileInfo = AudioPlayer2.Views.FileInfo;
using Resources = AudioPlayer2.Properties.Resources;

namespace AudioPlayer2.ViewModels
{
    public sealed class PlaylistViewModel : ViewModelBase
    {
        private readonly ITagManager tagManager;
        private readonly IDialogManager dialogManager;
        private readonly IPlaylist playlist;

        private ICommand openCommand;
        private ICommand initializedCommand;
        private ICommand showFileInfoCommand;
        private ICommand removeTrackCommand;
        private ICommand sortPlaylistCommand;
        private ICommand loadPlaylistCommand;
        private ICommand savePlaylistCommand;
        private ICommand openLocationCommand;
        private ICommand trackDoubleClickedCommand;

        private Track selectedTrack;
        private int selectedTrackIndex;

        public event EventHandler<Track> TrackDoubleClicked;

        public PlaylistViewModel(ITagManager tagManager, IPlaylist playlist, IDialogManager dialogManager)
        {
            this.tagManager = tagManager;
            this.dialogManager = dialogManager;
            this.playlist = playlist;
        }

        public ICollectionView Tracks { get; private set; }

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

        #region Commands
        public ICommand OpenCommand => this.openCommand ?? (this.openCommand = new RelayCommand(this.OnOpenCommand));
        public ICommand InitializedCommand => this.initializedCommand ?? (this.initializedCommand = new RelayCommand(this.Initialize));
        public ICommand RemoveTrackCommand => this.removeTrackCommand ?? (this.removeTrackCommand = new RelayCommand<RemoveTrackMode>(this.OnRemoveTrackCommand));
        public ICommand ShowFileInfoCommand => this.showFileInfoCommand ?? (this.showFileInfoCommand = new RelayCommand<Track>(this.OnShowFileInfoCommand));
        public ICommand OpenLocationCommand => this.openLocationCommand ?? (this.openLocationCommand = new RelayCommand<Track>(this.OnOpenLocationCommand));
        public ICommand SortPlaylistCommand => this.sortPlaylistCommand ?? (this.sortPlaylistCommand = new RelayCommand<SortPlaylistBy>(this.OnSortPlaylistCommand));
        public ICommand LoadPlaylistCommand => this.loadPlaylistCommand ?? (this.loadPlaylistCommand = new RelayCommand(this.OnLoadPlaylistCommand));
        public ICommand SavePlaylistCommand => this.savePlaylistCommand ?? (this.savePlaylistCommand = new RelayCommand(this.OnSavePlaylistCommand));
        public ICommand PlayCommand => this.trackDoubleClickedCommand ?? (this.trackDoubleClickedCommand = new RelayCommand<Track>(this.OnPlayCommand));
        
        #endregion
        private void Initialize()
        {
            this.AddToPlaylist(Directory.EnumerateFiles(@"G:\Muzyka", "*.*", SearchOption.AllDirectories));
            this.Tracks = CollectionViewSource.GetDefaultView(this.playlist.Tracks);
            this.RaisePropertyChanged(nameof(this.Tracks));
        }

        private void AddToPlaylist(IEnumerable<string> trackPaths)
        {
            var supportedFiles = Settings.Default.SupportedFiles;
            foreach (var trackPath in trackPaths.Where(p => !string.IsNullOrEmpty(p) && supportedFiles.Contains(Path.GetExtension(p))))
            {
                this.playlist.Add(trackPath);
            }
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
                this.AddToPlaylist(ofd.FileNames);
            }
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

        private void OnShowFileInfoCommand(Track track)
        {
            var param = new Dictionary<string, object> { { "track", track } };
            this.dialogManager.CreateDialog<FileInfo, FileInfoViewModel>(track.Uri, 500, 300, param).Show();
        }

        private void OnPlayCommand(Track track)
        {
            this.playlist.CurrentTrack = track;
            this.TrackDoubleClicked?.Invoke(this.playlist, track);
        }
    }
}
