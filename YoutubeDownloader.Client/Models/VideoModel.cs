using System.ComponentModel;
using System.Runtime.CompilerServices;
using Songbuilder.Client.Annotations;
using YoutubeAPI.Core;

namespace Songbuilder.Client.Models
{
    public class VideoModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private string _youtubeLink;
        private bool _shouldConvertToMp3;

        public string YoutubeLink
        {
            get => _youtubeLink;
            set
            {
                if (value == _youtubeLink) return;
                _youtubeLink = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldConvertToMp3
        {
            get => _shouldConvertToMp3;
            set
            {
                if (value == _shouldConvertToMp3) return;
                _shouldConvertToMp3 = value;
                OnPropertyChanged();
            }
        }

        public VideoFormat Format => ShouldConvertToMp3 ? VideoFormat.Mp3 : VideoFormat.Mp4;
    }
}
