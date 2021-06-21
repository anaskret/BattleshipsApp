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
                MessagingCenter.Send(this, "OpponentShot", args);
            });
            
            _connection.On<int, int, int, int>("GridStatus", (lobbyId, x, y, status) =>
            {
                string[] args = { x.ToString(), y.ToString(), status.ToString() };
                MessagingCenter.Send(this, "GridHitStatus", args);
            });
            
            _connection.On<int, int[], int[], int, bool>("GridStatusShipSunk", (lobbyId, x, y, status, isVertical) =>
            {
                string[] args = { string.Join("", x), string.Join("", y), status.ToString(), isVertical.ToString() };
                MessagingCenter.Send(this, "GridHitStatusShipSunk", args);
            });
            
            _connection.On<int>("Ready", (lobbyId) =>
            {
                MessagingCenter.Send(this, "OpponentReady");
            });
            
            _connection.On<int>("Start", (lobbyId) =>
            {
                MessagingCenter.Send(this, "StartGame");
            });
            
            _connection.On<int>("Victory", (lobbyId) =>
            {
                MessagingCenter.Send(this, "YouWon");
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
             await _connection.InvokeAsync("GridStatus", lobbyId, x, y, status);
        }
        
        public async Task GridStatusShipSunk(int lobbyId, int[] x, int[] y, int status, bool isVertical)
        {
             await _connection.InvokeAsync("GridStatusShipSunk", lobbyId, x, y, status, isVertical);
        }

        public async Task JoinGame(int lobbyId)
        {
            await _connection.InvokeAsync("JoinGame", lobbyId);
        }

        public async Task LeaveGame(int lobbyId)
        {
            await _connection.InvokeAsync("LeaveGame", lobbyId);
        }

        public async Task Ready(int lobbyId)
        {
            await _connection.InvokeAsync("Ready", lobbyId);
        }

        public async Task Start(int lobbyId)
        {
            await _connection.InvokeAsync("Start", lobbyId);
        }
        
        public async Task Victory(int lobbyId)
        {
            await _connection.InvokeAsync("Victory", lobbyId);
        }
    }
}
