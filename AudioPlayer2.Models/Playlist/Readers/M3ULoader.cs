using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AudioPlayer2.Models.Properties;
using AudioPlayer2.Models.Tag;

namespace AudioPlayer2.Models.Playlist.Readers
{
    internal class M3ULoader : PlaylistLoader
    {
        public override async Task<IReadOnlyList<Track>> LoadAsync(ITagManager tagManager)
        {
            var tracks = new List<Track>();

            var encoding = this.Path.EndsWith("m3u8") ? Encoding.UTF8 : Encoding.Default;
            using (var reader = new StreamReader(this.Path, encoding))
            {
                var line = await reader.ReadLineAsync();

                if (!string.Equals(line, Resources.M3UOpenTag, StringComparison.InvariantCultureIgnoreCase))
                {
                    return tracks;
                }

                while (!reader.EndOfStream)
                {
                    var info = await reader.ReadLineAsync();
                    line = await reader.ReadLineAsync();

                    if (line[1] != System.IO.Path.VolumeSeparatorChar && (line[2] != System.IO.Path.DirectorySeparatorChar) || line[2] != System.IO.Path.AltDirectorySeparatorChar)
                    {
                        line = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), line);
                    }

                    var track = Track.Create(tagManager, line);
                    var duration = this.GetTrackDuration(info);
                    if (duration.HasValue) track.Duration = duration.Value;
                    tracks.Add(track);
                }
            }

            return new ReadOnlyCollection<Track>(tracks);
        }

        private TimeSpan? GetTrackDuration(string m3uExt)
        {
            var components = m3uExt.Split(',',':');

            int seconds;
            if (int.TryParse(components[1], out seconds))
            {
                return TimeSpan.FromSeconds(seconds);
            }

            return null;
        }
    }
}
