using System;
using System.IO;
using SongBuilder.Core.Services;

namespace SongBuilder.Core
{
    public class YoutubeDownloader : IDisposable
    {
        private string WorkingDirectory { get; }
        private IYoutubeDownloadService DownloadService { get; }

        public YoutubeDownloader(IYoutubeDownloadService downloadService)
        {
            this.DownloadService = downloadService;

            this.WorkingDirectory = this.CreateWorkingDirectory();

            AppDomain.CurrentDomain.ProcessExit += (sender, args) => this.Dispose();
        }

        private string CreateWorkingDirectory()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            const string appName = "YoutubeDownloader";
            string sessionId = Guid.NewGuid().ToString();

            string workingDirectory = Path.Combine(appDataPath, appName, sessionId);

            if (Directory.Exists(this.WorkingDirectory) == false)
            {
                Directory.CreateDirectory(workingDirectory);
            }

            return workingDirectory;
        }

        public FileInfo DownloadVideo(string videoUrl)
        {
            return this.DownloadService.DownloadVideo(videoUrl, this.WorkingDirectory);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(this.WorkingDirectory, recursive: true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
