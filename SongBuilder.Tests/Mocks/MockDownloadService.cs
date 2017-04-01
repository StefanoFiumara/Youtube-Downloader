using System;
using System.IO;
using SongBuilder.Core.Services;

namespace SongBuilder.Tests.Mocks
{
    internal class MockDownloadService : IYoutubeDownloadService
    {
        public FileInfo DownloadVideo(string videoUrl, string downloadLocation)
        {
            string fileLocation = Path.Combine(downloadLocation, $"{Guid.NewGuid()}.txt");
            File.WriteAllText(fileLocation, "test file");

            return new FileInfo(fileLocation);
        }
    }
}