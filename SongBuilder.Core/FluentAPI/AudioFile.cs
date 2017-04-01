using System;
using System.Collections.Generic;
using System.IO;

namespace SongBuilder.Core.FluentAPI
{
    public class AudioFile
    {
        public FileInfo File { get; }

        public AudioFile(FileInfo file)
        {
            if (file.Extension.Contains("mp3") == false)
            {
                throw new ArgumentException($"{file.Name} is not a supported audio file!");
            }
            this.File = file;
        }

        public AudioFile Concat(AudioFile next)
        {
            return new AudioFile( AudioManager.ConcatAudioFiles(new List<FileInfo> { this.File, next.File }) );
        }
    }
}