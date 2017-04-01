using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Songbuilder.Client.Annotations;

namespace Songbuilder.Client.Models
{
    public class SongModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SongModel()
        {
            this.SongStartTime = new TimeStampModel();
            this.SongEndTime = new TimeStampModel(0, 4, 0);
        }

        private string _youtubeLink;
        private bool _applyFadeIn;
        private bool _applyFadeOut;
        private TimeStampModel _songEndTime;
        private TimeStampModel _songStartTime;

        public string YoutubeLink
        {
            get { return this._youtubeLink; }
            set
            {
                if (value == this._youtubeLink) return;
                this._youtubeLink = value;
                this.OnPropertyChanged();
            }
        }

        public bool ApplyFadeIn
        {
            get { return this._applyFadeIn; }
            set
            {
                if (value == this._applyFadeIn) return;
                this._applyFadeIn = value;
                this.OnPropertyChanged();
            }
        }

        public bool ApplyFadeOut
        {
            get { return this._applyFadeOut; }
            set
            {
                if (value == this._applyFadeOut) return;
                this._applyFadeOut = value;
                this.OnPropertyChanged();
            }
        }

        public TimeStampModel SongStartTime
        {
            get { return this._songStartTime; }
            set
            {
                if (value.Equals(this._songStartTime)) return;
                this._songStartTime = value;
                this.OnPropertyChanged();
            }
        }

        public TimeStampModel SongEndTime
        {
            get { return this._songEndTime; }
            set
            {
                if (value.Equals(this._songEndTime)) return;
                this._songEndTime = value;
                this.OnPropertyChanged();
            }
        }
    }
}
