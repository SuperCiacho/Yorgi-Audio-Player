using System;
using System.Threading;
using AudioPlayer2.Models.Playlist;
using AudioPlayer2.Models.Properties;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace AudioPlayer2.Models.Audio
{
    //TODO: Zrobić z tego kontrolke
    internal sealed class AudioPlayer : IAudioPlayer
    {
        #region Constants

        private const int SleepTime = 998;
        private const int Latency = 100;

        #endregion

        #region Fields
        private ISoundOut playbackMethod;
        private IWaveSource audioSource;

        /// <summary>
        /// Field holds volume value while audio source is null.
        /// </summary>
        private float volume;

        private readonly MMDevice playbackDevice;
        private bool isDisposed;

        private Timer timer;
        private IPlaylist playlist;

        #endregion

        #region Constructors
        public AudioPlayer() : this(new WasapiOut { Latency = Latency }) { }

        public AudioPlayer(ISoundOut playerType) : this(playerType, null) { }

        public AudioPlayer(ISoundOut playerType, IPlaylist playlist)
        {
            this.playbackMethod = playerType;
            this.Playlist = playlist;
            this.volume = Settings.Default.LastVolume;
            this.playbackDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }
        #endregion

        #region Events
        public event EventHandler<TrackProgressChangedArgs> ProgressChanged;
        public event EventHandler<EventArgs> PlaybackStarted;
        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;

        #endregion

        #region Properties

        public IPlaylist Playlist
        {
            get { return this.playlist; }
            set
            {
                if (this.playlist != null)
                {
                    this.playlist.CurrentTrackChanged -= this.OnPlaylistCurrentTrackChanged;
                }

                this.playlist = value;

                if (this.playlist != null)
                {
                    this.playlist.CurrentTrackChanged += this.OnPlaylistCurrentTrackChanged;
                }

            }
        }

        public bool IsPlaying => this.playbackMethod?.PlaybackState == PlaybackState.Playing;

        public bool IsPaused => this.playbackMethod == null || this.playbackMethod.PlaybackState == PlaybackState.Paused;

        public bool IsStopped => this.playbackMethod == null || this.playbackMethod.PlaybackState == PlaybackState.Stopped;

        public float Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                if (this.playbackMethod?.WaveSource != null)
                {
                    this.playbackMethod.Volume = value;
                }

                this.volume = value;
            }
        }

        public TimeSpan Position
        {
            get => this.audioSource?.GetPosition() ?? TimeSpan.Zero;
            set => this.audioSource.SetPosition(value);
        }

        public TimeSpan Length => this.audioSource?.GetLength() ?? TimeSpan.Zero;
        #endregion

        #region Methods

        private void OnPlaylistCurrentTrackChanged(object sender, Track track)
        {
            this.Play(track);
        }

        public void Play()
        {
            this.playlist.GetTrack(PlayerAction.Selected);
        }

        private void Play(Track currentTrack)
        {
            this.StopTracking();

            this.CleanUpPlayback();

            this.audioSource = CodecFactory.Instance.GetCodec(currentTrack.Uri)
                .ToSampleSource()
                .ToWaveSource();
            this.playbackMethod = new WasapiOut
            {
                Latency = Latency,
                Device = this.playbackDevice
            };
            this.playbackMethod.Initialize(this.audioSource);
            this.playbackMethod.Volume = this.volume;
            this.playbackMethod.Stopped += this.PlaybackStopped;
            this.playbackMethod.Play();

            this.StartTracking();

            //TODO Co jeśli włączono piosenkę spoza playlisty?!
        }

        public void Resume()
        {
            this.playbackMethod.Play();
            this.StartTracking();
        }

        public void Pause()
        {
            this.playbackMethod.Pause();
        }

        public void Stop()
        {
            this.playbackMethod.Stop();
            this.playbackMethod.Stopped -= this.PlaybackStopped;
            this.ProgressChanged?.Invoke(this.playlist.CurrentTrack, TrackProgressChangedArgs.Empty);
        }

        public void Previous()
        {
            this.Playlist.GetTrack(PlayerAction.Previous);
        }

        public void Next()
        {
            this.Playlist.GetTrack(PlayerAction.Next);
        }

        private void StartTracking()
        {
            this.timer = new Timer(this.Tracking, this, 0, SleepTime);
            this.PlaybackStarted?.Invoke(this.playlist.CurrentTrack, EventArgs.Empty);
        }

        private void StopTracking()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }
        }

        private void Tracking(object player)
        {
            var audioPlayer = player as AudioPlayer;
            if (audioPlayer == null) return;
            this.ProgressChanged?.Invoke(audioPlayer.playlist.CurrentTrack, new TrackProgressChangedArgs(audioPlayer.Position, audioPlayer.Length));
        }

        private void CleanUpPlayback()
        {
            if (this.playbackMethod != null)
            {
                this.playbackMethod.Stopped -= this.PlaybackStopped;
                this.playbackMethod.Dispose();
                this.playbackMethod = null;
            }

            if (this.audioSource != null)
            {
                this.audioSource.Dispose();
                this.audioSource = null;
            }
        }

        public void Dispose()
        {
            Settings.Default.LastVolume = this.Volume;
            Settings.Default.Save();

            this.Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                this.StopTracking();
                this.CleanUpPlayback();

                if (this.ProgressChanged != null)
                {
                    foreach (var e in this.ProgressChanged.GetInvocationList())
                    {
                        this.ProgressChanged -= e as EventHandler<TrackProgressChangedArgs>;
                    }
                }
            }

            this.isDisposed = true;
        }

        #endregion
    }
}
