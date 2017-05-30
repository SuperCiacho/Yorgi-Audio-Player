using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AudioPlayer2.Models.Playlist.Writers
{
    internal abstract class PlaylistWriter
    {
        private static readonly Dictionary<string, Lazy<PlaylistWriter>> LazyWriters;
        public string Path { get; set; }

        static PlaylistWriter()
        {
            LazyWriters = new Dictionary<string, Lazy<PlaylistWriter>>()
            {
                {".m3u", new Lazy<PlaylistWriter>(() => new M3UWriter(), true)},
                {".m3u8", new Lazy<PlaylistWriter>(() => LazyWriters[".m3u"].Value, true)}
            };
        }

        public abstract void WriteAsync(IEnumerable<Track> tracks);

        public static PlaylistWriter GetWriter(string playlistPath)
        {
            Lazy<PlaylistWriter> writer;
            if (string.IsNullOrWhiteSpace(playlistPath) ||
                !LazyWriters.TryGetValue(System.IO.Path.GetExtension(playlistPath), out writer))
            {
                return null;
            }

            writer.Value.Path = playlistPath;
            return writer.Value;
        }
    }
}