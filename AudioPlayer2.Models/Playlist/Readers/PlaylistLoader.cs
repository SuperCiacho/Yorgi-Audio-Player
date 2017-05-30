using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AudioPlayer2.Models.Tag;

namespace AudioPlayer2.Models.Playlist.Readers
{
    internal abstract class PlaylistLoader
    {
        private static readonly Dictionary<string, Lazy<PlaylistLoader>> LazyLoaders;
        public string Path { get; set; }

        static PlaylistLoader()
        {
            LazyLoaders = new Dictionary<string, Lazy<PlaylistLoader>>()
            {
                { ".m3u", new Lazy<PlaylistLoader>(() => new M3ULoader(), true) },  
                { ".m3u8", new Lazy<PlaylistLoader>(() => LazyLoaders[".m3u"].Value, true) }  
            };
        }
                                                                                                                                                                         
        public abstract Task<IReadOnlyList<Track>> LoadAsync(ITagManager tagManager);

        public static PlaylistLoader GetLoader(string playlistPath)
        {
            Lazy<PlaylistLoader> loader;
            if (string.IsNullOrWhiteSpace(playlistPath) || !LazyLoaders.TryGetValue(System.IO.Path.GetExtension(playlistPath), out loader))
            {
                throw new InvalidDataException("Playlist format not supported. Yet.");
            }
            loader.Value.Path = playlistPath;
            return loader.Value;
        }
    }
}