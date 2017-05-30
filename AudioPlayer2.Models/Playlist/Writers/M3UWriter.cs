using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioPlayer2.Models.Properties;

namespace AudioPlayer2.Models.Playlist.Writers
{
    internal class M3UWriter : PlaylistWriter
    {
        private const string ExtLineFormat = "#EXTINF:{0},{1}";

        public override async void WriteAsync(IEnumerable<Track> tracks)
        {
            using (var file = File.CreateText(this.Path))
            {
                await file.WriteLineAsync(Resources.M3UOpenTag);

                foreach (var track in tracks)
                {
                    var computedPath = this.GetComputedPath(this.Path, track.Uri);
                    await file.WriteLineAsync(
                        string.Format(ExtLineFormat, 
                        (int)track.Duration.TotalSeconds, 
                        System.IO.Path.GetFileName(track.Uri)));
                    await file.WriteLineAsync(computedPath);
                }
            }

            this.Path = null;
        }

        private string GetComputedPath(string playlistPath, string trackPath)
        {
            var skip = 0;
            var ppSegments = playlistPath.Split(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            var tpSegments = trackPath.Split(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            int n = Math.Min(ppSegments.Length, tpSegments.Length);

            for (int i = 0; i < n; i++)
            {
                if (ppSegments[i] == tpSegments[i]) skip++;
                else break;
            }

            return string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), tpSegments.Skip(skip));
        }
    }
}
