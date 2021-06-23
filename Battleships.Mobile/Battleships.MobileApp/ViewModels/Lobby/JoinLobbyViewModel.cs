using Battleships.MobileApp.Helpers;
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
            try
            {
                var lobby = await _lobbyService.GetLobbyByName(LobbyName);

                if(lobby.PlayerTwo != null)
                {
                    PopupHelper.DisplayErrorMessage("Lobby is full", "Failed to Join");
                    return;
                }

                lobby.PlayerTwo = _settingsService.UserName;
                await _lobbyService.UpdateLobby(lobby);

                await Shell.Current.GoToAsync($"{nameof(GamePage)}?LobbyId={lobby.Id}");
            }
            catch(Exception ex)
            {
                PopupHelper.DisplayErrorMessage("Lobby wasn't found", "Failed to Join");
            }
        }
    }
}
