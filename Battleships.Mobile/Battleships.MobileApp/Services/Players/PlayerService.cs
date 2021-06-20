using Battleships.MobileApp.Models;
using Battleships.MobileApp.Services.RequestProvider;
using Battleships.MobileApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.Services.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public PlayerService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
            _settingsService = DependencyService.Get<ISettingsService>();
        }

        public async Task<List<PlayerModel>> GetLeaderboard()
        {
            return await _requestProvider.GetAsync<List<PlayerModel>>(GlobalSetting.Instance.DefaultEndpoint + $"/player?userName={_settingsService.UserName}", _settingsService.AuthAccessToken);
        }

        public async Task Update()
        {
            await _requestProvider.PutAsync(GlobalSetting.Instance.DefaultEndpoint + $"/player", _settingsService.UserName, _settingsService.AuthAccessToken);
        }
    }
}
