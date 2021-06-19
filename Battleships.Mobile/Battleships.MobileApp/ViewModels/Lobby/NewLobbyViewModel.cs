using Battleships.MobileApp.Models;
using Battleships.MobileApp.Services.Lobby;
using Battleships.MobileApp.Services.Settings;
using Battleships.MobileApp.ViewModels.Base;
using Battleships.MobileApp.Views;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels.Lobby
{
    public class NewLobbyViewModel : ViewModelBase
    {
        private readonly ILobbyService _lobbyService;
        private readonly ISettingsService _settingsService;

        private string _lobbyName;
        public string LobbyName { get => _lobbyName; set => SetProperty(ref _lobbyName, value); }

        public AsyncCommand CreateCommand { get; set; }

        public NewLobbyViewModel()
        {
            _lobbyService = DependencyService.Get<ILobbyService>();
            _settingsService = DependencyService.Get<ISettingsService>();

            CreateCommand = new AsyncCommand(Create);
        }

        private async Task Create()
        {
            var lobby = new LobbyModel() { Id = 2, Name = LobbyName, PlayerOne = _settingsService.UserName };//get lobby from api

            //await _lobbyService.CreateLobby(lobby);

            await Shell.Current.GoToAsync($"//{nameof(GamePage)}?LobbyId={lobby.Id}");
        }
    }
}
