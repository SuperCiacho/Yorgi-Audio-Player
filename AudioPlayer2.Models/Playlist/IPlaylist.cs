﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioPlayer2.Models.Playlist
{
    public interface IPlaylist
    {
        /// <summary>
        /// Gets number or tracks on the playlist.
        /// </summary>
        int Size { get; }
        Track CurrentTrack { get; set; }
        int CurrentTrackIndex { get; }

        IReadOnlyCollection<Track> Tracks { get; }

        void Add(Track trackToAdd);
        void Add(IEnumerable<Track> tracksToAdd);
        void Remove(Track trackToRemove);
        void Remove(IEnumerable<Track> tracksToRemove);
        void Clear();

        Track GetTrack(PlayerAction playerAction);
        string GetTrackPath(PlayerAction playerAction);

        Task SortAsync(SortPlaylistBy mode, bool @descending);
    }
}