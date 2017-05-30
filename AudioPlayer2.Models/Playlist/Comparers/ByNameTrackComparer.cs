using System;

namespace AudioPlayer2.Models.Playlist.Comparers
{
    internal class ByNameTrackComparer : TrackComparer
    {
        protected override int CompareTracks(Track x, Track y)
        {
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }
}