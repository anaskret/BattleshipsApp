using Battleships.MobileApp.Models;
using Battleships.MobileApp.Services.Lobby;
using Battleships.MobileApp.Services.Settings;
using Battleships.MobileApp.ViewModels.Base;
using Battleships.MobileApp.Views;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels.Lobby
{
    public class JoinLobbyViewModel : ViewModelBase
    {
        private readonly ILobbyService _lobbyService;
        private readonly ISettingsService _settingsService;

        private string _lobbyName;
        public string LobbyName { get => _lobbyName; set => SetProperty(ref _lobbyName, value); }

        public AsyncCommand JoinCommand { get; set; }

        public JoinLobbyViewModel()
        {
            _lobbyService = DependencyService.Get<ILobbyService>();
            _settingsService = DependencyService.Get<ISettingsService>();

            JoinCommand = new AsyncCommand(Join);
        }

        private async Task Join()
        {
            var lobby = new LobbyModel() { Id = 2, Name = "Test", PlayerOne = "Test" };

            if(lobby.PlayerTwo != null)
            {
                DisplayErrorMessage("Lobby is full");
                return;
            }

            //lobby.PlayerTwo = _settingsService.UserName;
            //await _lobbyService.UpdateLobby(lobby);

            await Shell.Current.GoToAsync($"//{nameof(GamePage)}?LobbyId={lobby.Id}");
        }

        private async void DisplayErrorMessage(string message)
        {
            var pop = new PopupInfo
            {
                BindingContext = new PopupInfoViewModel()
                {
                    Message = message,
                    Title = "Failed to Join"
                }
            };

            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            return;
        }
    }
}
