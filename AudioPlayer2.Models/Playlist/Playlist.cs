using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioPlayer2.Models.Playlist.Comparers;

namespace AudioPlayer2.Models.Playlist
{
    internal sealed class Playlist : IPlaylist
    {
        private readonly List<Track> tracks;
        private Track currentTrack;
        private bool isNewTrackAssigned;

        public Playlist()
        {
            this.tracks = new List<Track>();
        }

        public Track CurrentTrack
        {
            get => this.currentTrack;
            set
            {
                if (!Track.Equals(this.currentTrack, value))
                {
                    this.currentTrack = value;
                    this.isNewTrackAssigned = true;
                }

                if (value == null)
                {
                    this.CurrentTrackIndex = -1;
                }
            }
        }

        public IReadOnlyCollection<Track> Tracks => this.tracks;

        public int CurrentTrackIndex { get; private set; } = int.MinValue;

        public int Size => this.tracks.Count;

        public void Add(Track trackToAdd)
        {
            this.tracks.Add(trackToAdd);
        }
        
        public void Add(IEnumerable<Track> tracksToAdd)
        {
            foreach (var track in tracksToAdd)
            {
                this.Add(track);
            }
        }

        public void Remove(Track trackToRemove)
        {
            this.tracks.Remove(trackToRemove);
        }

        public void Remove(IEnumerable<Track> tracksToRemove)
        {
            foreach (var track in tracksToRemove)
            {
                this.Remove(track);
            }
        }

        public void Clear()
        {
            this.tracks.Clear();
        }

        public Track GetTrack(PlayerAction playerAction)
        {
            var index = -1;

            if (this.isNewTrackAssigned || playerAction == PlayerAction.Next || playerAction == PlayerAction.Previous)
            {
                index = this.tracks.FindIndex(track => track.Equals(this.CurrentTrack));
                this.isNewTrackAssigned = false;
            }

            if (index == -1)
            {
                index = this.CurrentTrackIndex;
            }

            if (index == int.MinValue)
            {
                return null;
            }

            switch (playerAction)
            {
                case PlayerAction.Previous:
                    index = index == 0 ? this.tracks.Count - 1 : --index;
                    break;
                case PlayerAction.Current:
                case PlayerAction.Selected:
                    break;
                case PlayerAction.Next:
                    index = index == this.tracks.Count - 1 ? 0 : ++index;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerAction), playerAction, null);
            }

            this.CurrentTrackIndex = index;
            return this.CurrentTrack = this.tracks[index];
        }

        public string GetTrackPath(PlayerAction playerAction)
        {
            return this.GetTrack(playerAction)?.Uri;
        }

        public Task SortAsync(SortPlaylistBy mode, bool descending)
        {
            return Task.Run(() =>
            {
                var comparer = TrackComparer.GetTrackComparer(mode, descending);
                this.tracks.Sort(comparer);
            });
        }
    }
}