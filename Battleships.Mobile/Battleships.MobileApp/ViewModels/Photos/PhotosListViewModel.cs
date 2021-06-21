using Battleships.MobileApp.Models.Db;
using Battleships.MobileApp.Services.SqliteService;
using Battleships.MobileApp.ViewModels.Base;
using MvvmHelpers.Commands;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.ViewModels.Photos
{
    public class PhotosListViewModel : ViewModelBase
    {
        private ObservableCollection<Photo> _photos;
        public ObservableCollection<Photo> Photos { get => _photos; set => SetProperty(ref _photos, value); }

        public AsyncCommand TakePhotoCommand { get; set; }

        public PhotosListViewModel()
        {
            TakePhotoCommand = new AsyncCommand(OpenCamera);
        }

        public override async void InitializeAsync()
        {
            Photos = new ObservableCollection<Photo>(await SqliteService.GetPhotos());
            base.InitializeAsync();
        }

        public async Task OpenCamera()
        {
            try
            {
                var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Battleships",
                    Name = $"{DateTime.Now}.jpg"
                });

                await SqliteService.AddPhoto(photo.Path);

                InitializeAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
