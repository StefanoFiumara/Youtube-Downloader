using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Songbuilder.Client.Annotations;

namespace Songbuilder.Client.Models
{
    public class TimeStampModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _hours;
        private int _minutes;
        private int _seconds;

        public TimeStampModel(int hours, int minutes, int seconds)
        {
            this.Hours = hours;
            this.Minutes = minutes;
            this.Seconds = seconds;
        }

        public TimeStampModel() : this(0,0,0)
        {
            
        }
        
        public TimeSpan AsTimeSpan() => new TimeSpan(0, this.Hours, this.Minutes, this.Seconds);

        public int Hours
        {
            get { return this._hours; }
            set
            {
                if (value == this._hours) return;
                this._hours = value;
                this.OnPropertyChanged();
            }
        }

        public int Minutes
        {
            get { return this._minutes; }
            set
            {
                if (value == this._minutes) return;
                this._minutes = value;
                this.OnPropertyChanged();
            }
        }

        public int Seconds
        {
            get { return this._seconds; }
            set
            {
                if (value == this._seconds) return;
                this._seconds = value;
                this.OnPropertyChanged();
            }
        }
    }
}
