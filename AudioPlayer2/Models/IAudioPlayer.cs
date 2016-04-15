using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace AudioPlayer2.Models
{
    public interface IAudioPlayer : IDisposable
    {
         /// <summary>
        /// Gets number or tracks on the playlist.
        /// </summary>
        int TrackCount { get; }

        /// <summary>
        /// Returns copy of playlist.
        /// </summary>
        IReadOnlyList<string> Tracks { get; }

        float Volume { get; set; }
        TimeSpan Position { get; set; }
        TimeSpan Length { get; }

        bool IsPlaying { get; }
        bool IsPaused { get; }
        bool IsStopped { get; }

        string CurrentTrack { get; }
        int CurrentTrackIndex { get; }

        void Play(string path);
        void Resume();
        void Pause();
        void Stop();
        void Previous();
        void Next();

        void AddTrack(string trackToAdd);
        void AddTracks(IEnumerable<string> tracksToAdd);
        void RemoveTrack(string trackToRemove);
        void RemoveTracks(IEnumerable<string> tracksToRemove);
        void ClearTracks();

        event EventHandler<ProgressChangedArgs> ProgressChanged;
        event EventHandler<StoppedEventArgs> PlaybackStopped;
    }
}
