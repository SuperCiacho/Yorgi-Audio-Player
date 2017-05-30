using AudioPlayer2.Models;
using AudioPlayer2.Models.Audio;
using AudioPlayer2.Models.Playlist;
using AudioPlayer2.Models.Tag;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace AudioPlayer2.ViewModels.Tests
{
    [TestFixture]
    public class MainViewModelTests
    {
        private Fixture factory;
        private MainViewModel testViewModel;
        private Mock<IAudioPlayer> audioPlayerMock;
        private Mock<ITagManager> tagManagerMock;

        [Test]
        public void Whether_PlayingSound_On_PlayCommand_Execution()
        {
            var track = this.factory.Create<Track>();
            this.audioPlayerMock.Setup(x => x.Play());
            this.audioPlayerMock.Setup(x => x.IsPaused).Returns(false);

            this.testViewModel.PlayCommand.Execute(track);

            this.audioPlayerMock.Verify(x => x.Play(), Times.Once());
        }

        [Test]
        public void Whether_SoundIsPaused_On_PlayCommand_SecondExecution()
        {
            this.audioPlayerMock.Setup(x => x.Play());
            this.audioPlayerMock.Setup(x => x.IsPaused).Returns(false);
            var track = this.factory.Create<Track>();

            this.testViewModel.PlayCommand.Execute(track);
            this.testViewModel.TrackDuration = 100;
            this.testViewModel.PlayCommand.Execute(track);
        
            this.audioPlayerMock.Verify(x => x.Play(), Times.Exactly(2));
            Assert.That(this.testViewModel.TrackDuration, Is.EqualTo(0));
        }

        [Test]
        public void Whether_SoundIsPaused_On_PauseCommand_Execution()
        {
            this.audioPlayerMock.Setup(x => x.IsPlaying).Returns(true);
            this.audioPlayerMock.Setup(x => x.Pause());

            this.testViewModel.PauseCommand.Execute(null);

            this.audioPlayerMock.Verify(x => x.Pause(), Times.Once);
            this.audioPlayerMock.Verify(x => x.IsPlaying, Times.Once);
        }

        [Test]
        public void Whether_SoundIsResumed_On_PauseCommandExecution_When_SoundAlreadyPaused()
        {
            this.audioPlayerMock.Setup(x => x.IsPlaying).Returns(false);
            this.audioPlayerMock.Setup(x => x.IsPaused).Returns(true);
            this.audioPlayerMock.Setup(x => x.Resume());

            this.testViewModel.PauseCommand.Execute(null);

            this.audioPlayerMock.Verify(x => x.Pause(), Times.Never);
            this.audioPlayerMock.Verify(x => x.Resume(), Times.Once);
            this.audioPlayerMock.Verify(x => x.IsPlaying, Times.Once);
            this.audioPlayerMock.Verify(x => x.IsPaused, Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            this.audioPlayerMock = new Mock<IAudioPlayer>(MockBehavior.Strict);
            this.audioPlayerMock.Setup(x => x.Dispose());
            this.tagManagerMock = new Mock<ITagManager>(MockBehavior.Default);
            this.factory = new Fixture();
            this.factory.Register<ITagManager>(() => this.tagManagerMock.Object);

            this.testViewModel = new MainViewModel()
            {
                AudioPlayer = this.audioPlayerMock.Object,
                TagManager = this.tagManagerMock.Object
            };
        }

        [TearDown]
        public void CleanUp()
        {
            this.testViewModel.Cleanup();
            this.testViewModel = null;
            this.audioPlayerMock = null;
        }
    }
}