using Battleships.MobileApp.Models.Authentication;
using Battleships.MobileApp.Services.RequestProvider;
using Battleships.MobileApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public AuthService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
            _settingsService = DependencyService.Get<ISettingsService>();
        }

        public async Task Login(AuthModel model)
        {
            var response = await _requestProvider.PostAsync(GlobalSetting.Instance.DefaultEndpoint + "/login", model);

            _settingsService.UserName = model.Username;
            _settingsService.AuthAccessToken = response.Token;
        }

        public async Task Register(AuthModel model)
        {
            try
            {
                await _requestProvider.PostAsync(GlobalSetting.Instance.DefaultEndpoint + "/register", model);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
