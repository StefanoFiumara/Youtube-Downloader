using System.IO;

namespace SongBuilder.Core.Services
{
    public interface IYoutubeDownloadService
    {
        FileInfo DownloadVideo(string videoUrl, string downloadLocation);
    }
}