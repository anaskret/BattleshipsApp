using Battleships.MobileApp.Models.Authentication;
using Battleships.MobileApp.Services.Authorization;
using Battleships.MobileApp.Services.Game;
using Battleships.MobileApp.ViewModels.Base;
using Battleships.MobileApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;

        private string _userName;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }

        private string _password;
        public string Password{ get => _password; set => SetProperty(ref _password, value); }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewModel()
        {
            _authService = DependencyService.Get<IAuthService>();

            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                if(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {
                    DisplayErrorMessage("Fill in Username and Password");
                    return;
                }

                await _authService.Login(new AuthModel(UserName, Password));
            }
            catch(Exception ex)
            {
                DisplayErrorMessage(ex.Message);
                return;
            }

            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
        
        private async void OnRegisterClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }

        private async void DisplayErrorMessage(string message)
        {
            var pop = new PopupInfo
            {
                BindingContext = new PopupInfoViewModel()
                {
                    Message = message,
                    Title = "Login Failed"
                }
            };

            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            return;
        }
    }
}
