// AudioPlayer2
// Createad by Bartosz Nowak on 05/12/2015 01:17

using System;

namespace AudioPlayer2.Models.Tag
{
    public interface ITaggedFile
    {
        string Title { get; set; }
        string Artist { get; set; }
        string Genre { get; set; }
        uint Year { get; set; }
        string Album { get; set; }
        uint TrackNumber { get; set; }

        TimeSpan Duration { get; }
        string Quality { get; }
        string Size { get; }
        string Path { get; }


    }
}