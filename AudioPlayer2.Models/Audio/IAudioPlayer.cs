using System;
using AudioPlayer2.Models.Playlist;
using CSCore.SoundOut;

namespace AudioPlayer2.Models.Audio
{
    public interface IAudioPlayer : IDisposable
    {
        IPlaylist Playlist { get; set; }

        float Volume { get; set; }
        TimeSpan Position { get; set; }
        TimeSpan Length { get; }

        bool IsPlaying { get; }
        bool IsPaused { get; }
        bool IsStopped { get; }

        void Play();
        void Resume();
        void Pause();
        void Stop();
        void Previous();
        void Next();

        event EventHandler<TrackProgressChangedArgs> ProgressChanged;
        event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;
        event EventHandler<EventArgs> PlaybackStarted;
    }
}
