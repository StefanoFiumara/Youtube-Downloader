using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using Songbuilder.Client.Commands;
using Songbuilder.Client.Models;
using Songbuilder.Client.Services;

namespace Songbuilder.Client
{
    public class MainWindowViewModel
    {
        public MainWindowModel Model { get; set; }

        public RelayCommand AddSongCommand { get; set; }
        public RelayCommand CreateSongMixCommand { get; set; }

        private SongMixCreationService Service { get; }
        
        public MainWindowViewModel()
        {
            this.Model = new MainWindowModel();
            this.Service = new SongMixCreationService();

            this.AddSongCommand = new RelayCommand(this.AddSong);
            this.CreateSongMixCommand = new RelayCommand(this.CreateSongMix, this.CanCreateSongMix);
        }

        private void AddSong()
        {
            this.Model.Songs.Add(new SongModel());
        }

        private async void CreateSongMix()
        {
            this.Model.IsBusy = true;
            var result = await Task.Run(() => this.Service.CreateSongMix(this.Model.Songs));
            this.Model.IsBusy = false;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "mp3 files (*.mp3)|*.mp3|All files (*.*)|*.*",
                FilterIndex = 0,
                OverwritePrompt = true
            };

            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != null && (bool) dialogResult)
            {
                var selectedFile = new FileInfo(saveFileDialog.FileName);
                result.File.CopyTo(selectedFile.FullName, overwrite: true);

                var song = new Process
                {
                    StartInfo = { FileName =  selectedFile.FullName }
                };

                song.Start();
            }
        }

        private bool CanCreateSongMix()
        {
            return this.Model.Songs.Any();
        }
    }
}
