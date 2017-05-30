using System;
using MusicFile = TagLib.File;

namespace AudioPlayer2.Models.Tag
{
    public class TagManager : ITagManager
    {
        public TimeSpan GetTrackDuration(string path)
        {
            var metaData = ReadMetaData(path);
            return metaData.Properties.Duration;
        }

        public ITaggedFile GetTaggedFile(Track track)
        {
            return (TaggedFile) ReadMetaData(track.Uri);
        }

        public string GetTrackName(string path)
        {
            var metaData = ReadMetaData(path);
            var artist = metaData.Tag.FirstPerformer;
            var title = metaData.Tag.Title;

            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                return System.IO.Path.GetFileNameWithoutExtension(metaData.Name);
            }

            return $"{artist} - {title}";
        }

        private static MusicFile ReadMetaData(string path)
        {
            return MusicFile.Create(path);
        }

        public void SaveMetaData(object data)
        {
            throw new NotImplementedException();
        }
    }
}
