using Battleships.MobileApp.Helpers;
using Battleships.MobileApp.Models.Authentication;
using Battleships.MobileApp.Services.Authorization;
using Battleships.MobileApp.ViewModels.Base;
using Battleships.MobileApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels.Register
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;

        private string _userName;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        
        private string _confrimPassword;
        public string ConfirmPassword { get => _confrimPassword; set => SetProperty(ref _confrimPassword, value); }

        public Command RegisterCommand { get; }

        public RegisterViewModel()
        {
            _authService = DependencyService.Get<IAuthService>();

            RegisterCommand = new Command(OnRegisterClicked);
        }

        private async void OnRegisterClicked(object obj)
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {

                PopupHelper.DisplayErrorMessage("Password cannot be empty", "Incorrect Password");
                return;
            }
            if (!Password.Equals(ConfirmPassword))
            {
                PopupHelper.DisplayErrorMessage("Passwords are different", "Incorrect Password");
                return;
            }
            if (Password.Length < 8 || !Password.Any(char.IsDigit) || !Password.Any(char.IsUpper))
            {
                PopupHelper.DisplayErrorMessage("Password has to have at least 8 characters, including 1 digit and 1 upper letter", "Incorrect Password");
                return;
            }

            try
            {
                await _authService.Register(new AuthModel(UserName, Password));
            }
            catch(Exception ex)
            {
                PopupHelper.DisplayErrorMessage(ex.Message, "Something went wrong");
                return;
            }

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
