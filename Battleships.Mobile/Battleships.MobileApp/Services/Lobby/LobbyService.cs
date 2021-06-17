using Battleships.MobileApp.Models;
using Battleships.MobileApp.Services.RequestProvider;
using Battleships.MobileApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.Services.Lobby
{
    public class LobbyService : ILobbyService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public LobbyService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
            _settingsService = DependencyService.Get<ISettingsService>();
        }

        public async Task CreateLobby(LobbyModel lobby)
        {
            await _requestProvider.PostAsync(GlobalSetting.Instance.DefaultEndpoint + "lobbies/create", lobby, _settingsService.AuthAccessToken);
        }
        
        public async Task UpdateLobby(LobbyModel lobby)
        {
            await _requestProvider.PutAsync(GlobalSetting.Instance.DefaultEndpoint + "lobbies/create", lobby, _settingsService.AuthAccessToken);
        }

        public async Task<LobbyModel> GetLobbyById(int id)
        {
            var response = await _requestProvider.GetAsync<LobbyModel>(GlobalSetting.Instance.DefaultEndpoint + $"/lobbies/get?id={id}", _settingsService.AuthAccessToken);
            return response;
        }
    }
}
