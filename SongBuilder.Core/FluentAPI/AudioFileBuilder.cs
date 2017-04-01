using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBuilder.Core.FluentAPI
{
    public class AudioFileBuilder
    {
        protected internal List<Func<AudioFile, AudioFile>> _builderActions;

        private readonly AudioFile _initial; 

        public AudioFileBuilder(AudioFile initial)
        {
            this._initial = initial;
            this._builderActions = new List<Func<AudioFile, AudioFile>>();
        }

        public AudioFile Build()
        {
            return this._builderActions.Aggregate(this._initial, (file, func) => func(file));
        }

        public ConditionalAudioFileBuilder Trim(TimeSpan begin, TimeSpan end)
        {
            this._builderActions.Add(audioFile => new AudioFile(AudioManager.TrimAudioLength(audioFile.File, begin, end)));
            return new ConditionalAudioFileBuilder(this, this._initial);
        }

        public ConditionalAudioFileBuilder ApplyFadeIn(int durationSeconds)
        {
            this._builderActions.Add(audioFile => new AudioFile(AudioManager.ApplyFadeIn(audioFile.File, durationSeconds)));
            return new ConditionalAudioFileBuilder(this, this._initial);
        }

        public ConditionalAudioFileBuilder ApplyFadeOut(int durationSeconds)
        {
            this._builderActions.Add(audioFile => new AudioFile(AudioManager.ApplyFadeOut(audioFile.File, durationSeconds)));
            return new ConditionalAudioFileBuilder(this, this._initial);
        }
    }

    public class ConditionalAudioFileBuilder : AudioFileBuilder
    {
        public ConditionalAudioFileBuilder(AudioFileBuilder wrappedBuilder, AudioFile initial) : base(initial)
        {
            this._builderActions = wrappedBuilder._builderActions;
        }

        public AudioFileBuilder When(bool condition)
        {
            if (!condition)
            {
                this._builderActions.RemoveAt(this._builderActions.Count - 1);
            }
            return this;
        }
    }
}
