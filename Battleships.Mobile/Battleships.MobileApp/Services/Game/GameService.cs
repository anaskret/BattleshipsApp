using Battleships.MobileApp.Services.RequestProvider;
using Battleships.MobileApp.Services.Settings;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.Services.Game
{
    public class GameService : IGameService
    {
        private HubConnection _connection;
        private readonly ISettingsService _settingsService;

        public GameService()
        {
            _settingsService = DependencyService.Get<ISettingsService>();

            _connection = new HubConnectionBuilder()
                .WithUrl(GlobalSetting.Instance.DefaultEndpoint + "/gameHub")
                .Build();
        }


        public async Task Connect()
        {
            if (_connection.State == HubConnectionState.Connected) return;

            _connection.On<int, int, int, string>("Shoot", (lobbyId, x, y, player) =>
            {
                string[] args = { x.ToString(), y.ToString(), player };
                MessagingCenter.Send(Application.Current, "OpponentShot", args);
            });

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task Disconnect()
        {
            await _connection.DisposeAsync();

            _connection = new HubConnectionBuilder()
                .WithUrl(GlobalSetting.Instance.DefaultEndpoint + "/gameHub")
                .Build();
        }

        public async Task Shoot(int lobbyId, int x, int y, string player)
        {
             await _connection.InvokeAsync("Shoot", lobbyId, x, y, player);
        }

        public async Task GridStatus(int lobbyId, int x, int y, int status)
        {
             await _connection.InvokeAsync("Shoot", lobbyId, x, y, status);
        }
    }
}
