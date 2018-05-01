using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Songbuilder.Client.Annotations;

namespace Songbuilder.Client.Models
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private ObservableCollection<VideoModel> _videos;
        
        public ObservableCollection<VideoModel> Videos
        {
            get => _videos;
            set
            {
                if (Equals(value, _videos)) return;
                _videos = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (value == _isBusy) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public MainWindowModel()
        {
            Videos = new ObservableCollection<VideoModel> {new VideoModel()};
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
