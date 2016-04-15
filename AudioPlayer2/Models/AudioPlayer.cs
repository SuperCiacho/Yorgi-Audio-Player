using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AudioPlayer2.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace AudioPlayer2.Models
{
    //TODO: Zrobić z tego kontrolke
    internal sealed class AudioPlayer : IAudioPlayer
    {
        #region Constants

        private const int SleepTime = 998;
        private const int Latency = 100;

        #endregion
        #region Fields
        private readonly List<string> tracks;
        private IWavePlayer playbackDevice;
        private AudioFileReader audioFileReader;

        /// <summary>
        /// Field holds volume value while Audio File Reader is null.
        /// </summary>
        private float volumeHolder;

        #endregion

        #region Constructors
        public AudioPlayer() : this(new WasapiOut(AudioClientShareMode.Shared, true, 100)) { }

        public AudioPlayer(IWavePlayer playerType) : this(playerType, new List<string>()) { }

        public AudioPlayer(IWavePlayer playerType, List<string> tracks)
        {
            this.playbackDevice = playerType;
            this.tracks = tracks;
            this.volumeHolder = Settings.Default.LastVolume;
        }
        #endregion

        #region Events
        public event EventHandler<ProgressChangedArgs> ProgressChanged;

        public event EventHandler<StoppedEventArgs> PlaybackStopped
        {
            add { this.playbackDevice.PlaybackStopped += value; }
            remove { this.playbackDevice.PlaybackStopped -= value; }
        }

        #endregion

        #region Properties
        public int TrackCount => this.tracks.Count;

        public IReadOnlyList<string> Tracks => this.tracks;

        public string CurrentTrack { get; private set; }

        public int CurrentTrackIndex { get; private set; }

        public bool IsPlaying => this.playbackDevice.PlaybackState == PlaybackState.Playing;

        public bool IsPaused => this.playbackDevice.PlaybackState == PlaybackState.Paused;

        public bool IsStopped => this.playbackDevice.PlaybackState == PlaybackState.Stopped;

        public float Volume
        {
            get { return this.audioFileReader?.Volume ?? this.volumeHolder; }
            set
            {
                if (this.audioFileReader != null)
                {
                    this.audioFileReader.Volume = value;
                }

                this.volumeHolder = value;
            }
        }

        public TimeSpan Position
        {
            get { return this.audioFileReader?.CurrentTime ?? TimeSpan.Zero; }
            set { this.audioFileReader.CurrentTime = value; }
        }

        public TimeSpan Length => this.audioFileReader?.TotalTime ?? TimeSpan.Zero;
        #endregion

        #region Methods

        public void Play(string trackPath)
        {
            this.InternalPlay(PlayAction.Selected, trackPath);

            //TODO Co jeśli włączono piosenkę spoza playlisty?!
        }

        private void InternalPlay(PlayAction playAction, string trackPath = null)
        {
            if (this.tracks.Count == 0)
            {
                this.tracks.Add(trackPath);
            }

            this.CalculateTrackIndex(playAction, trackPath ?? this.CurrentTrack);

            if (trackPath == null)
            {
                trackPath = this.tracks[this.CurrentTrackIndex];
            }

            if (!this.IsStopped && !this.IsPaused)
            {
                this.Stop();
            }

            if (this.CurrentTrack == null || !trackPath.Equals(this.CurrentTrack))
            {
                this.CurrentTrack = trackPath;

                this.audioFileReader = new AudioFileReader(trackPath);
                this.Volume = this.volumeHolder;

                this.playbackDevice = new WasapiOut(AudioClientShareMode.Shared, true, Latency);
                this.playbackDevice.Init(this.audioFileReader);
            }

            this.playbackDevice.Play();

            this.StartTracking();
        }

        public void Resume()
        {
            this.playbackDevice.Play();
            this.StartTracking();
        }

        public void Pause()
        {
            this.playbackDevice.Pause();
        }

        public void Stop()
        {
            this.playbackDevice?.Stop();

            if (audioFileReader != null)
            {
                audioFileReader.Position = 0;
            }

            this.ProgressChanged?.Invoke(this.CurrentTrack, ProgressChangedArgs.Empty);

            this.CurrentTrack = null;
        }

        public void Previous()
        {
            this.InternalPlay(PlayAction.Previous);
        }

        public void Next()
        {
            this.InternalPlay(PlayAction.Next);
        }

        private void CalculateTrackIndex(PlayAction playAction, string track)
        {
            var index = this.tracks.FindIndex(t => t.Equals(track));

            if (index == -1)
            {
                index = this.CurrentTrackIndex;
            }

            switch (playAction)
            {
                case PlayAction.Previous:
                    index = index == 0 ? this.tracks.Count - 1 : --index;
                    break;
                case PlayAction.Selected:
                    break;
                case PlayAction.Next:
                    index = index == this.tracks.Count - 1 ? 0 : ++index;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playAction), playAction, null);
            }

            this.CurrentTrackIndex = index;
        }

        private void StartTracking()
        {
            Task.Factory.StartNew(() =>
            {
                var length = this.Length;
                var track = this.CurrentTrack;
                do
                {
                    var oldPos = this.Position;

                    this.ProgressChanged?.Invoke(track, new ProgressChangedArgs(oldPos, this.Position, length));

                    Thread.Sleep(SleepTime);
                } while (track.Equals(this.CurrentTrack));
            });
        }

        public void AddTrack(string trackToAdd)
        {
            this.tracks.Add(trackToAdd);
        }

        public void AddTracks(IEnumerable<string> tracksToAdd)
        {
            foreach (var track in tracksToAdd)
            {
                this.AddTrack(track);
            }
        }

        public void RemoveTrack(string trackToRemove)
        {
            this.tracks.Remove(trackToRemove);
        }

        public void RemoveTracks(IEnumerable<string> tracksToRemove)
        {
            foreach (var track in tracksToRemove)
            {
                this.RemoveTrack(track);
            }
        }

        public void ClearTracks()
        {
            this.tracks.Clear();
        }

        public void Dispose()
        {
            Settings.Default.LastVolume = this.Volume;
            Settings.Default.Save();

            this.audioFileReader?.Dispose();

            if (this.ProgressChanged != null)
            {
                foreach (var e in this.ProgressChanged.GetInvocationList())
                {
                    this.ProgressChanged -= e as EventHandler<ProgressChangedArgs>;
                }
            }
        }

        #endregion
    }
}
