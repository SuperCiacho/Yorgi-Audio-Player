using System;
using AudioPlayer2.Models.Tag;

namespace AudioPlayer2.Models
{
    public class Track
    {
        private readonly ITagManager tagManager;
        private string name;
        private string artist;
        private ITaggedFile tag;
        private TimeSpan? duration;

        public Track(ITagManager tagmanager, string path)
        {
            this.tagManager = tagmanager;
            this.Uri = path;
        }

        public ITaggedFile Tag => this.tag ?? (this.tag = this.tagManager.GetTaggedFile(this));

        public string Name => this.name ?? (this.name = this.tagManager.GetTrackName(this.Uri));

        public string Artist => this.artist ?? (this.artist = this.Tag.Artist);

        public string Uri { get; }

        public TimeSpan Duration
        {
            get
            {
                if (!this.duration.HasValue)
                {
                    this.duration = this.Tag.Duration;
                }

                return this.duration.Value;
            }

            set { this.duration = value; }
        }

        public static Track Create(ITagManager tagmanager, string path)
        {
            return new Track(tagmanager, path);
        }
    }
}
