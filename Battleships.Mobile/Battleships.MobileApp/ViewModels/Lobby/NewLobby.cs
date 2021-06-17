using Battleships.MobileApp.Models;
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
    public class NewLobby : ViewModelBase
    {
        private string _lobbyName;
        public string LobbyName { get => _lobbyName; set => SetProperty(ref _lobbyName, value); }

        public AsyncCommand CreateCommand { get; set; }

        public NewLobby()
        {
            CreateCommand = new AsyncCommand(Create);
        }

        private async Task Create()
        {
            var lobby = new LobbyModel() { Id = 0, Name = "Test", PlayerOne = "Test" };//get lobby from api
            await Shell.Current.GoToAsync($"//{nameof(GamePage)}?id={lobby.Id}");
        }
    }
}
