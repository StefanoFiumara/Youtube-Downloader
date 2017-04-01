using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NAudio.Wave;

namespace SongBuilder.Core
{
    public static class AudioManager
    {
        public static FileInfo ExtractAudio(FileInfo videoFile)
        {
            if (videoFile.Exists == false)
                throw new FileNotFoundException($"Could not find video file at {videoFile.FullName}");

            string mp3FilePath = Path.Combine(videoFile.DirectoryName,
                Path.GetFileNameWithoutExtension(videoFile.Name) + ".mp3");
            FileInfo outputFile = new FileInfo(mp3FilePath);

            if (outputFile.Exists) File.Delete(outputFile.FullName);

            var arguments = $@"-i ""{videoFile.FullName}"" -c:a mp3 ""{mp3FilePath}""";

            AudioManager.ExecuteFFMPEG(arguments);

            outputFile.Refresh();
            return outputFile;
        }

        public static FileInfo TrimAudioLength(FileInfo audioFile, TimeSpan begin, TimeSpan end)
        {
            if (audioFile.Exists == false)
                throw new FileNotFoundException($"Could not find video file at {audioFile.FullName}");

            if (begin > end)
                throw new ArgumentOutOfRangeException(nameof(end), "end should be greater than begin");


            string outputPath = Path.Combine(audioFile.DirectoryName,
                Path.GetFileNameWithoutExtension(audioFile.Name) + "_Trimmed.mp3");
            var outputFile = new FileInfo(outputPath);

            if (outputFile.Exists) File.Delete(outputFile.FullName);

            string timeStart = begin.ToString();
            string duration = (end - begin).ToString();
            var arguments = $@"-ss {timeStart} -t {duration} -i ""{audioFile.FullName}"" -acodec copy ""{outputPath}""";

            AudioManager.ExecuteFFMPEG(arguments);

            outputFile.Refresh();
            return outputFile;
        }

        public static FileInfo ApplyFadeIn(FileInfo audioFile, int durationInSeconds)
        {
            if (audioFile.Exists == false)
                throw new FileNotFoundException($"Could not find video file at {audioFile.FullName}");

            string outputPath = Path.Combine(audioFile.DirectoryName,
                Path.GetFileNameWithoutExtension(audioFile.Name) + "_WithFadeIn.mp3");
            var outputFile = new FileInfo(outputPath);
            if (outputFile.Exists) File.Delete(outputFile.FullName);

            var arguments =
                $@"-i ""{audioFile.FullName}"" -af afade=t=in:st=0:d={durationInSeconds} ""{outputFile.FullName}""";

            AudioManager.ExecuteFFMPEG(arguments);

            outputFile.Refresh();
            return outputFile;
        }

        public static FileInfo ApplyFadeOut(FileInfo audioFile, int durationInSeconds)
        {
            if (audioFile.Exists == false)
                throw new FileNotFoundException($"Could not find video file at {audioFile.FullName}");

            string outputPath = Path.Combine(audioFile.DirectoryName,
                Path.GetFileNameWithoutExtension(audioFile.Name) + "_WithFadeOut.mp3");
            var outputFile = new FileInfo(outputPath);
            if (outputFile.Exists) File.Delete(outputFile.FullName);

            var mp3Info = new Mp3FileReader(audioFile.FullName);

            int startTime = (int) (mp3Info.TotalTime.TotalSeconds - 3);

            var arguments =
                $@"-i ""{audioFile.FullName}"" -af afade=t=out:st={startTime}:d={durationInSeconds} ""{outputFile
                    .FullName}""";

            AudioManager.ExecuteFFMPEG(arguments);

            outputFile.Refresh();
            return outputFile;
        }

        public static FileInfo ConcatAudioFiles(List<FileInfo> audioFiles)
        {
            if (audioFiles.Any(f => f.Exists == false) || audioFiles.Count == 0)
                throw new FileNotFoundException("One of the provided video files could not be found.");

            var referenceFile = audioFiles.First();
            string outputPath = Path.Combine(referenceFile.DirectoryName,
                Path.GetFileNameWithoutExtension(referenceFile.Name) + "_Joined.mp3");
            var outputFile = new FileInfo(outputPath);
            if (outputFile.Exists) File.Delete(outputFile.FullName);

            var argList = string.Join("|", audioFiles.Select(f => f.FullName));

            var arguments = $@"-i ""concat:{argList}"" -acodec copy ""{outputFile.FullName}""";

            AudioManager.ExecuteFFMPEG(arguments);

            outputFile.Refresh();
            return outputFile;
        }

        private static void ExecuteFFMPEG(string arguments)
        {
            if (File.Exists("External\\ffmpeg.exe") == false)
                throw new InvalidOperationException("ffmpeg.exe could not be found.");

            var ffmpeg = new Process
            {
                StartInfo = {UseShellExecute = false, RedirectStandardError = true, FileName = "External\\ffmpeg.exe"}
            };

            ffmpeg.StartInfo.Arguments = arguments;

            if (!ffmpeg.Start())
            {
                throw new Exception("Could not start ffmpeg.exe");
            }

            var reader = ffmpeg.StandardError;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Debug.WriteLine(line);
            }

            ffmpeg.WaitForExit();
            ffmpeg.Close();
        }
    }
}