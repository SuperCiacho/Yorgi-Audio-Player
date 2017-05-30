using NUnit.Framework;
using AudioPlayer2.Models.Tag;
using Moq;
using Ploeh.AutoFixture;

namespace AudioPlayer2.Models.Playlist.Comparers.Tests
{
    [TestFixture]
    public class ByArtistTrackComparerTests
    {
        private ByArtistTrackComparer testComparer;
        private Fixture factory;

        [SetUp]
        public void Setup()
        {
            this.factory = new Fixture();
            this.testComparer = new ByArtistTrackComparer();
        }

        [Test]
        public void Whether_ByArtistTrackComparer_ReturnsZero_On_ComparingTheSameTrack()
        {
            var taggedFileMock = new Mock<ITaggedFile>(MockBehavior.Strict);
            var tagManagerMock = new Mock<ITagManager>(MockBehavior.Strict);
            taggedFileMock.Setup(x => x.Artist).Returns("Test Artist");
            tagManagerMock.Setup(x => x.GetTaggedFile(It.IsAny<Track>())).Returns(taggedFileMock.Object);
            var trackPath = this.factory.Create<string>();

            var trackA = new Track(tagManagerMock.Object, trackPath);
            var trackB = new Track(tagManagerMock.Object, trackPath);

            var result = this.testComparer.Compare(trackA, trackB);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Whether_ByArtistTrackComparer_ReturnsZero_On_ComparingTwoNulls()
        {
            var result = this.testComparer.Compare(null, null);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Whether_ByArtistTrackComparer_ReturnsLessThanZero_On_ComparingWithTrack_Which_ArtistNameStartsOnFurtherLetter()
        {
            var taggedFileAMock = new Mock<ITaggedFile>(MockBehavior.Strict);
            var taggedFileBMock = new Mock<ITaggedFile>(MockBehavior.Strict);
            var tagManagerMock = new Mock<ITagManager>(MockBehavior.Strict);
            var trackPath = this.factory.Create<string>();
            var trackA = new Track(tagManagerMock.Object, trackPath);
            var trackB = new Track(tagManagerMock.Object, trackPath);
            taggedFileAMock.Setup(x => x.Artist).Returns("A Test Artist");
            taggedFileBMock.Setup(x => x.Artist).Returns("The Test Artist");
            tagManagerMock.Setup(x => x.GetTaggedFile(trackA)).Returns(taggedFileAMock.Object);
            tagManagerMock.Setup(x => x.GetTaggedFile(trackB)).Returns(taggedFileBMock.Object);
            
            var result = this.testComparer.Compare(trackA, trackB);

            Assert.That(result, Is.LessThan(0));
        }

        [Test]
        public void Whether_ByArtistTrackComparer_ReturnsMinusOne_On_ComparingNullWithTrack()
        {
            var taggedFileAMock = new Mock<ITaggedFile>(MockBehavior.Strict);
            var tagManagerMock = new Mock<ITagManager>(MockBehavior.Strict);
            var trackPath = this.factory.Create<string>();
            var track = new Track(tagManagerMock.Object, trackPath);
            taggedFileAMock.Setup(x => x.Artist).Returns("A Test Artist");
            tagManagerMock.Setup(x => x.GetTaggedFile(track)).Returns(taggedFileAMock.Object);

            var result = this.testComparer.Compare(null, track);

            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void Whether_ByArtistTrackComparer_ReturnsMoreThanZero_On_ComparingWithTrack_Which_ArtistNameStartsOnEarlierLetter()
        {
            var taggedFileAMock = new Mock<ITaggedFile>(MockBehavior.Strict);
            var taggedFileBMock = new Mock<ITaggedFile>(MockBehavior.Strict);
            var tagManagerMock = new Mock<ITagManager>(MockBehavior.Strict);
            var trackPath = this.factory.Create<string>();
            var trackA = new Track(tagManagerMock.Object, trackPath);
            var trackB = new Track(tagManagerMock.Object, trackPath);
            taggedFileAMock.Setup(x => x.Artist).Returns("The Test Artist");
            taggedFileBMock.Setup(x => x.Artist).Returns("A Test Artist");
            tagManagerMock.Setup(x => x.GetTaggedFile(trackA)).Returns(taggedFileAMock.Object);
            tagManagerMock.Setup(x => x.GetTaggedFile(trackB)).Returns(taggedFileBMock.Object);

            var result = this.testComparer.Compare(trackA, trackB);

            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void Whether_ByArtistTrackComparer_ReturnsOne_On_ComparingTrackWithNull()
        {
            var taggedFileAMock = new Mock<ITaggedFile>(MockBehavior.Strict);
            var tagManagerMock = new Mock<ITagManager>(MockBehavior.Strict);
            var trackPath = this.factory.Create<string>();
            var track = new Track(tagManagerMock.Object, trackPath);
            taggedFileAMock.Setup(x => x.Artist).Returns("A Test Artist");
            tagManagerMock.Setup(x => x.GetTaggedFile(track)).Returns(taggedFileAMock.Object);

            var result = this.testComparer.Compare(track, null);

            Assert.That(result, Is.EqualTo(1));
        }
    }
}