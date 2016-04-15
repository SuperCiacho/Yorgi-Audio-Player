using System;

namespace AudioPlayer2.Models
{
    public sealed class ProgressChangedArgs : EventArgs
    {
        public new static readonly ProgressChangedArgs Empty = new ProgressChangedArgs(TimeSpan.Zero, 
            TimeSpan.Zero, 
            TimeSpan.FromSeconds(1));

        public ProgressChangedArgs(TimeSpan oldPosition, TimeSpan newPosition, TimeSpan trackLength)
        {
            this.OldPosition = oldPosition;
            this.NewPosition = newPosition;
            this.TrackLength = trackLength;
        }

        public TimeSpan OldPosition { get; }

        public TimeSpan NewPosition { get; }

        public TimeSpan TrackLength { get; }
    }
}