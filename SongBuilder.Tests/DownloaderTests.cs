using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongBuilder.Core;
using SongBuilder.Tests.Mocks;

namespace SongBuilder.Tests
{
    [TestClass]
    public class DownloaderTests
    {
        private const string TEST_DOWNLOAD_LINK = @"https://www.youtube.com/watch?v=aHjpOzsQ9YI";

        private readonly MockDownloadService _mockService = new MockDownloadService();

        [TestMethod]
        public void DownloadVideoTest()
        {
            using (var downloader = new YoutubeDownloader(this._mockService))
            {
                FileInfo file = downloader.DownloadVideo(DownloaderTests.TEST_DOWNLOAD_LINK);

                Assert.IsTrue(file.Exists);
            }
        }
    }
}
