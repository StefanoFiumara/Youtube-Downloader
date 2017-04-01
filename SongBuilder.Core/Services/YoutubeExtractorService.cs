using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YoutubeExtractor;

namespace SongBuilder.Core.Services
{
    public class YoutubeExtractorService : IYoutubeDownloadService
    {
        public FileInfo DownloadVideo(string videoUrl, string downloadLocation)
        {
            //TODO: Parametrize the download preferences
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(videoUrl, false);

            return this.DownloadVideo(videoInfos, downloadLocation);
        }

        private FileInfo DownloadVideo(IEnumerable<VideoInfo> videoInfos, string downloadLocation)
        {
            VideoInfo video = videoInfos
                .OrderByDescending(info => info.AudioBitrate)
                .ThenBy(info => info.Resolution)
                .First();

            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            string fileName = this.RemoveIllegalPathCharacters(Guid.NewGuid().ToString()) + video.VideoExtension;
            FileInfo downloadedFileInfo = new FileInfo(Path.Combine(downloadLocation, fileName));

            var downloader = new VideoDownloader(video, downloadedFileInfo.FullName);

            downloader.Execute();

            downloadedFileInfo.Refresh();
            return downloadedFileInfo;
        }

        private string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex($"[{Regex.Escape(regexSearch)}]");
            return r.Replace(path, "");
        }
    }
}
