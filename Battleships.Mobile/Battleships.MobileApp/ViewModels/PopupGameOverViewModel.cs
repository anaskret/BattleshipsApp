using Battleships.MobileApp.Services.Game;
using Battleships.MobileApp.Services.Lobby;
using Battleships.MobileApp.Services.SqliteService;
using Battleships.MobileApp.Views;
using MvvmHelpers.Commands;
using Plugin.Media;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels
{
    public class PopupGameOverViewModel : BaseViewModel
    {
        private readonly IGameService _gameService;
        private readonly ILobbyService _lobbyService;

        string _message;
        public string Message { get => _message; set => SetProperty(ref _message, value); }
        int _lobbyId;
        public int LobbyId { get => _lobbyId; set => SetProperty(ref _lobbyId, value); }


        public AsyncCommand ClosePopupCommand { get; }
        public AsyncCommand TakePhotoCommand { get; }

        public PopupGameOverViewModel()
        {
            _gameService = DependencyService.Get<IGameService>();
            _lobbyService = DependencyService.Get<ILobbyService>();

            ClosePopupCommand = new AsyncCommand(ClosePopup);
            TakePhotoCommand = new AsyncCommand(TakePhoto);
        }

        private async Task TakePhoto() 
        {
            try
            {
                var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Win",
                    Name = $"{DateTime.Now}.jpg"
                });

                await SqliteService.AddPhoto(photo.Path);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task ClosePopup()
        {
            try
            {
                await _gameService.Connect();
                await _gameService.LeaveGame(LobbyId);
                await _lobbyService.Delete(LobbyId);

                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.Navigation.PopPopupAsync(true);
            }
            catch(Exception ex)
            {
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.Navigation.PopPopupAsync(true);
            }
        }
    }
}
