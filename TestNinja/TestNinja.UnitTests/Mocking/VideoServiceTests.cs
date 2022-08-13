using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        public Mock<IFileReader> _mockFileReader;
        public Mock<IVideoRepository> _repository;
        public VideoService _videoService;
        [SetUp]
        public void SetUp()
        {
            _mockFileReader = new Mock<IFileReader>();
            _repository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_mockFileReader.Object, _repository.Object);
        }
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            _mockFileReader = new Mock<IFileReader>();
            _videoService = new VideoService(_mockFileReader.Object);
            _mockFileReader.Setup(from => from.Read("video.txt")).Returns("");

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }
        [Test]
        public void GetUnProcessedVideosAsCsv_AllVideosAreProcessed_ReturnAnEmptyString()
        {
            _repository.Setup(r => r.GetUnProcessedVideos()).Returns(new List<Video>());

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo(""));
        }
        [Test]
        public void GetUnProcessedVideosAsCsv_AFewProcessedVideos_ReturnAnAStringWithIdOfUnprocessedVideos()
        {
            _repository.Setup(r => r.GetUnProcessedVideos()).Returns(new List<Video>
            {
                new Video(){ Id = 1},
                new Video(){ Id = 2},
                new Video(){ Id = 3},
            });

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("1,2,3"));
        }
    }
}
