using System;

namespace AudioPlayer2.Models.Playlist.Comparers
{
    internal class ByArtistTrackComparer : TrackComparer
    {
        protected override int CompareTracks(Track x, Track y)
        {
            return string.Compare(x.Artist, y.Artist, StringComparison.Ordinal);
        }
    }
}