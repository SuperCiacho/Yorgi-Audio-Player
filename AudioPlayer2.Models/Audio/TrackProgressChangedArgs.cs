using System;

namespace AudioPlayer2.Models.Audio
{
    public sealed class TrackProgressChangedArgs : EventArgs
    {
        public new static readonly TrackProgressChangedArgs Empty = new TrackProgressChangedArgs(TimeSpan.Zero, TimeSpan.FromSeconds(1));

        public TrackProgressChangedArgs(TimeSpan position, TimeSpan trackLength)
        {
            this.Position = position;
            this.TrackLength = trackLength;
        }

        public TimeSpan Position { get; }

        public TimeSpan TrackLength { get; }
    }
}