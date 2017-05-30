// AudioPlayer2
// Createad by Bartosz Nowak on 05/12/2015 01:18

using System;
using TagLib;

namespace AudioPlayer2.Models.Tag
{
    public class TaggedFile : ITaggedFile
    {
        private readonly File taggedFile;

        public TaggedFile(File taggedFile)
        {
            this.taggedFile = taggedFile;

            this.Title = taggedFile.Tag.Title;
            this.Artist = taggedFile.Tag.FirstPerformer;
            this.Genre = taggedFile.Tag.FirstGenre;
            this.Year = taggedFile.Tag.Year;
            this.Album = taggedFile.Tag.Album;
            this.TrackNumber = taggedFile.Tag.Track;

            this.Quality = $"{this.taggedFile.Properties.AudioSampleRate}, {this.taggedFile.Properties.BitsPerSample} ";
            this.Size = this.GetSizeString(this.taggedFile.InvariantEndPosition);
        }

        #region Implementation of ITaggedFile

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Genre { get; set; }

        public uint Year { get; set; }

        public string Album { get; set; }

        public uint TrackNumber { get; set; }

        public TimeSpan Duration => this.taggedFile.Properties.Duration;

        public string Quality { get; }

        public string Size { get; }

        public string Path => this.taggedFile.Name;

        #endregion

        private string GetSizeString(long length)
        {
            long B = 0, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size = length;
            string suffix = nameof(B);

            if (length >= TB)
            {
                size = Math.Round((double)length / TB, 2);
                suffix = nameof(TB);
            }
            else if (length >= GB)
            {
                size = Math.Round((double)length / GB, 2);
                suffix = nameof(GB);
            }
            else if (length >= MB)
            {
                size = Math.Round((double)length / MB, 2);
                suffix = nameof(MB);
            }
            else if (length >= KB)
            {
                size = Math.Round((double)length / KB, 2);
                suffix = nameof(KB);
            }

            return $"{size} {suffix}";
        }

        public static explicit operator TaggedFile(File file)
        {
            return new TaggedFile(file);
        }
    }
}