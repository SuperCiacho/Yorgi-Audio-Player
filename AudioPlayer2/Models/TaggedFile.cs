// AudioPlayer2
// Createad by Bartosz Nowak on 05/12/2015 01:18

using System;
using System.Runtime.Remoting.Messaging;
using TagLib;

namespace AudioPlayer2.Models
{
    public class TaggedFile : ITaggedFile
    {
        private readonly File taggedFile;

        private string title;
        private string artist;
        private string genre;
        private uint year;
        private string album;
        private uint trackNumber;

        public TaggedFile(File taggedFile)
        {
            this.taggedFile = taggedFile;

            this.title = taggedFile.Tag.Title;
            this.artist = taggedFile.Tag.FirstPerformer;
            this.genre = taggedFile.Tag.FirstGenre;
            this.year = taggedFile.Tag.Year;
            this.album = taggedFile.Tag.Album;
            this.trackNumber = taggedFile.Tag.Track;

            this.Quality = $"{this.taggedFile.Properties.AudioSampleRate}, {this.taggedFile.Properties.BitsPerSample} ";
            this.Size = this.GetSizeString(this.taggedFile.InvariantEndPosition);
        }

        #region Implementation of ITaggedFile

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Artist
        {
            get { return this.artist; }
            set { this.artist = value; }
        }

        public string Genre
        {
            get { return this.genre; }
            set { this.genre = value; }
        }

        public uint Year
        {
            get { return this.year; }
            set { this.year = value; }
        }

        public string Album
        {
            get { return this.album; }
            set { this.album = value; }
        }

        public uint TrackNumber
        {
            get { return this.trackNumber; }
            set { this.trackNumber = value; }
        }

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