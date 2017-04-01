using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Wave;
using SongBuilder.Core;

namespace SongBuilder.Tests
{
    [TestClass]
    public class AudioManagerTests
    {
        private const string TEST_VIDEO = @"..\..\TestData\video.mp4";
        private const string TEST_AUDIO = @"..\..\TestData\audio.mp3";
        private const string TEST_AUDIO_TRIMMED = @"..\..\TestData\audio_Trimmed.mp3";

        [TestMethod]
        public void ExtractAudioTest()
        {
            var videoFile = new FileInfo(AudioManagerTests.TEST_VIDEO);

            FileInfo extractedAudio =  AudioManager.ExtractAudio(videoFile);

            Assert.IsTrue(extractedAudio.Exists);
        }

        [TestMethod]
        public void TrimAudioTest()
        {
            var audioFile = new FileInfo(AudioManagerTests.TEST_AUDIO);

            FileInfo trimmedAudio = AudioManager.TrimAudioLength(audioFile, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(20));

            var original = new Mp3FileReader(audioFile.FullName);
            var trimmed = new Mp3FileReader(trimmedAudio.FullName);
            
            Assert.IsTrue(trimmed.TotalTime < original.TotalTime);
        }

        [TestMethod]
        public void FadeInAudioTest()
        {
            var audioFile = new FileInfo(AudioManagerTests.TEST_AUDIO);

            FileInfo fadedAudio =  AudioManager.ApplyFadeIn(audioFile, 3);

            Assert.IsTrue(fadedAudio.Exists);
        }

        [TestMethod]
        public void FadeOutAudioTest()
        {
            var audioFile = new FileInfo(AudioManagerTests.TEST_AUDIO_TRIMMED);

            FileInfo fadedAudio =  AudioManager.ApplyFadeOut(audioFile, 3);

            Assert.IsTrue(fadedAudio.Exists);
        }

        [TestMethod]
        public void ConcatAudioFilesTest()
        {
            var audioFiles = new List<FileInfo>
            {
                new FileInfo(AudioManagerTests.TEST_AUDIO),
                new FileInfo(AudioManagerTests.TEST_AUDIO_TRIMMED),
            };

            FileInfo joinedFile =  AudioManager.ConcatAudioFiles(audioFiles);

            var firstMp3 = new Mp3FileReader(audioFiles[0].FullName);
            var secondMp3 = new Mp3FileReader(audioFiles[1].FullName);

            var joinedMp3 = new Mp3FileReader(joinedFile.FullName);

            Assert.IsTrue(joinedFile.Exists);

            var difference = joinedMp3.TotalTime.TotalSeconds - (firstMp3.TotalTime + secondMp3.TotalTime).TotalSeconds;
            Assert.IsTrue(Math.Abs(difference) < 0.1);
        }
    }
}
