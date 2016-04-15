using System;

namespace AudioPlayer2.Models
{
    public interface ITagManager
    {
        string GetTrackName(string path);
        TimeSpan GetTrackDuration(string path);
        ITaggedFile GetTaggedFile(Track track);
    }
}
