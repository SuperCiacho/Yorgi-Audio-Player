using System;
using AudioPlayer2.Models.Playlist;

namespace AudioPlayer2.Models.Tag
{
    public interface ITagManager
    {
        string GetTrackName(string path);
        TimeSpan GetTrackDuration(string path);
        ITaggedFile GetTaggedFile(Track track);
    }
}
