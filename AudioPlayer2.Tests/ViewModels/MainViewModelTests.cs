using AudioPlayer2.Models;
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
            var isPlaying = false;
            var trackPath = this.factory.Create<string>();
            this.audioPlayerMock.Setup(x => x.Play(trackPath)).Callback(() => isPlaying = true);
            this.audioPlayerMock.Setup(player => player.IsPlaying).Returns(isPlaying);
            Track track = new Track(this.tagManagerMock.Object, trackPath);

            this.testViewModel.PlayCommand.Execute(track);

            this.audioPlayerMock.Verify(x => x.Play(trackPath), Times.Once());
            this.audioPlayerMock.Verify(x => x.IsPlaying, Times.Once());

            Assert.That(isPlaying, Is.True);
        }

        [Test]
        public void Whether_SoundIsPaused_On_PlayCommand_SecondExecution()
        {
            var trackPath = this.factory.Create<string>();
            var isPaused = false;
            this.audioPlayerMock.Setup(x => x.Pause()).Callback(() => isPaused = true);
            this.audioPlayerMock.Setup(player => player.IsPlaying).Returns(!isPaused);
            this.audioPlayerMock.Setup(player => player.IsPaused).Returns(isPaused);
            this.audioPlayerMock.Setup(player => player.CurrentTrack).Returns(trackPath);
            Track track = new Track(this.tagManagerMock.Object, trackPath);
            this.testViewModel.SelectedTrack = track;

            this.testViewModel.PlayCommand.Execute(track);

            this.audioPlayerMock.Verify(x => x.Pause(), Times.Once());
            this.audioPlayerMock.Verify(x => x.IsPlaying, Times.Once());
            Assert.That(isPaused, Is.True);
        }

        [SetUp]
        public void SetUp()
        {
            this.factory = new Fixture();
            this.audioPlayerMock = new Mock<IAudioPlayer>();
            this.tagManagerMock = new Mock<ITagManager>();
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