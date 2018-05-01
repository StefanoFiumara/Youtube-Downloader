using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FanoMvvm.Commands;
using Ookii.Dialogs.Wpf;
using Songbuilder.Client.Models;
using YoutubeAPI.Core.Services;

namespace Songbuilder.Client
{
    public class MainWindowViewModel
    {
        public MainWindowModel Model { get; set; }

        public RelayCommand AddVideoCommand { get; set; }
        public RelayCommand DownloadVideosCommand { get; set; }
        public RelayCommand<VideoModel> RemoveVideoCommand { get; set; }
        
        public YoutubeDownloader Downloader { get; set; }
        
        public MainWindowViewModel()
        {
            Model = new MainWindowModel();
            Downloader = new YoutubeDownloader();

            AddVideoCommand = new RelayCommand(AddSong);
            DownloadVideosCommand = new RelayCommand(DownloadSongs, CanDownloadVideos);
            RemoveVideoCommand = new RelayCommand<VideoModel>(RemoveSong);
        }

        private void RemoveSong(VideoModel video)
        {
            if (Model.Videos.Count != 1)
            {
                Model.Videos.Remove(video);
            }
            else
            {
                Model.Videos.Single().YoutubeLink = string.Empty;
            }
        }

        private void AddSong()
        {
            Model.Videos.Add(new VideoModel());
        }

        private async void DownloadSongs()
        {
            try
            {
                Model.IsBusy = true;

                var downloadedFiles = await Task.Run(() =>
                    Model.Videos.AsParallel()
                        .AsOrdered()
                        .Select(s => Downloader.DownloadVideo(s.YoutubeLink, s.Format))
                        .ToList()
                );

                Model.IsBusy = false;

                var folderDialog = new VistaFolderBrowserDialog();

                var selectSaveFolderDialog = folderDialog.ShowDialog();
                if (selectSaveFolderDialog == true)
                {
                    var selectedPath = new DirectoryInfo(folderDialog.SelectedPath);

                    foreach (var file in downloadedFiles)
                    {
                        file.CopyTo(Path.Combine(selectedPath.FullName, file.Name), overwrite: true);
                    }

                    Process.Start(selectedPath.FullName);
                }
            }
            catch (AggregateException ex)
            {
                string message = ex.Flatten().InnerExceptions.Aggregate("", (msg, e) => msg + (e.Message + '\n'));
                MessageBox.Show($"Errors:\n{message}", "Multiple Errors Occured", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error Has Occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Model.IsBusy = false;
            }
            
        }
        
        private bool CanDownloadVideos()
        {
            var duplicateLinks = Model.Videos.GroupBy(x => x.YoutubeLink).Any(g => g.Count() > 1);

            bool allLinksFilled = Model.Videos.All(s => !string.IsNullOrEmpty(s?.YoutubeLink?.Trim()));

            return Model.Videos.Any() && allLinksFilled && !duplicateLinks;
        }
    }
}
