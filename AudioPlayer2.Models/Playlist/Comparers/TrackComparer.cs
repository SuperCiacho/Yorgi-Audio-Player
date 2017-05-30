using System;
using System.Collections.Generic;

namespace AudioPlayer2.Models.Playlist.Comparers
{
    internal abstract class TrackComparer : IComparer<Track>
    {
        private static readonly Dictionary<SortPlaylistBy, Lazy<TrackComparer>> comparers;

        static TrackComparer()
        {
            comparers = new Dictionary<SortPlaylistBy, Lazy<TrackComparer>>(3)
            {
                {SortPlaylistBy.Artist, new Lazy<TrackComparer>(() => new ByArtistTrackComparer())},
                {SortPlaylistBy.Name, new Lazy<TrackComparer>(() => new ByNameTrackComparer())},
                {SortPlaylistBy.Duration, new Lazy<TrackComparer>(() => new ByTimeTrackComparer())},
            };
        }

        public bool? Descending { get; set; }

        protected int? NullCheck(object x, object y)
        {
            if (x == null & y == null)
            {
                return 0;
            }

            if (y == null)
            {
                return 1;
            }

            if (x == null)
            {
                return -1;
            }

            return default(int?);
        }

        public int Compare(Track x, Track y)
        {
            var comparison = this.NullCheck(x, y) ?? this.CompareTracks(x, y);
            if (this.Descending.HasValue && this.Descending.Value)
            {
                comparison *= -1;
                this.Descending = null; // reseting comparer's state;
            }
            return comparison;
        }

        protected abstract int CompareTracks(Track x, Track y);

        internal static IComparer<Track> GetTrackComparer(SortPlaylistBy mode, bool descending)
        {
            Lazy<TrackComparer> lazyComparer;
            if (!comparers.TryGetValue(mode, out lazyComparer))
            {
                throw new ArgumentOutOfRangeException(nameof(mode));
            }
            var comparer = lazyComparer.Value;
            comparer.Descending = @descending;
            return comparer;
        }
    }
}
