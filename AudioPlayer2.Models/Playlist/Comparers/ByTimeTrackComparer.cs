namespace AudioPlayer2.Models.Playlist.Comparers
{
    internal class ByTimeTrackComparer : TrackComparer
    {
        protected override int CompareTracks(Track x, Track y)
        {
            return x.Duration.CompareTo(y.Duration);               
        }
    }
}