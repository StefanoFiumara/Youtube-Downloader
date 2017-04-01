using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Songbuilder.Client.Annotations;

namespace Songbuilder.Client.Models
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private ObservableCollection<SongModel> _songs;

        public ObservableCollection<SongModel> Songs
        {
            get { return this._songs; }
            set
            {
                if (Equals(value, this._songs)) return;
                this._songs = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                if (value == this._isBusy) return;
                this._isBusy = value;
                this.OnPropertyChanged();
            }
        }

        public MainWindowModel()
        {
            this.Songs = new ObservableCollection<SongModel> {new SongModel()};
            this.IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
