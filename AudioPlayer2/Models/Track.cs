using System;
using System.Dynamic;

namespace AudioPlayer2.Models
{
    public class Track
    {
        private readonly ITagManager tagManager;
        private string name;
        private TimeSpan? duration;

        internal Track(string name)
        {
            this.name = name;
            this.duration = TimeSpan.FromMinutes(3.5d);
        }

        public Track(ITagManager tagmanager, string path)
        {
            this.tagManager = tagmanager;
            this.Uri = path;
        }

        public ITaggedFile Tag => this.tagManager.GetTaggedFile(this);

        public string Name => this.name ?? (this.name = this.tagManager.GetTrackName(this.Uri));

        public string Uri { get; }

        public TimeSpan Duration
        {
            get
            {
                if (this.duration == null)
                {
                    this.duration = this.tagManager.GetTrackDuration(this.Uri);
                }

                return this.duration.Value;
            }
        }
    }
}
