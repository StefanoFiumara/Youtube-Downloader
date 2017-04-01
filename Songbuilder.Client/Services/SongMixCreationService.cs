using System.Collections.Generic;
using System.Linq;
using Songbuilder.Client.Models;
using SongBuilder.Core;
using SongBuilder.Core.FluentAPI;
using SongBuilder.Core.Services;

namespace Songbuilder.Client.Services
{
    public class SongMixCreationService
    {
        public YoutubeDownloader Downloader { get; set; }

        public SongMixCreationService()
        {
            this.Downloader = new YoutubeDownloader(new YoutubeExtractorService());
        }

        public AudioFile CreateSongMix(IEnumerable<SongModel> songs)
        {
            return songs.AsParallel()
                .AsOrdered()
                .Select(this.ProcessSongRequest)
                .Select(builder => builder.Build())
                .Aggregate((current, next) => current.Concat(next));
        }

        private AudioFileBuilder ProcessSongRequest(SongModel request)
        {
            var videoFile = this.Downloader.DownloadVideo(request.YoutubeLink);
            var audioFile = new AudioFile( AudioManager.ExtractAudio(videoFile) );

            return new AudioFileBuilder(audioFile)
                .Trim(request.SongStartTime.AsTimeSpan(), request.SongEndTime.AsTimeSpan())
                .ApplyFadeIn(3).When(request.ApplyFadeIn)
                .ApplyFadeOut(3).When(request.ApplyFadeOut);
        }
    }
}
