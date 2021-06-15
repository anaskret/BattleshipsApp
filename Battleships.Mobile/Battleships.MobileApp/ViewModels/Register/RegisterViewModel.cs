using Battleships.MobileApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels.Register
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _userName;
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        
        private string _confrimPassword;
        public string ConfirmPassword { get => _confrimPassword; set => SetProperty(ref _confrimPassword, value); }

        public Command RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
        }

        private async void OnRegisterClicked(object obj)
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                DisplayErrorMessage("Password cannot be empty");
                return;
            }
            if (!Password.Equals(ConfirmPassword))
            {
                DisplayErrorMessage("Passwords are different");
                return;
            }
            if (Password.Length < 8 || !Password.Any(char.IsDigit) || !Password.Any(char.IsUpper))
            {
                DisplayErrorMessage("Password has to have at least 8 characters, including 1 digit and 1 upper letter");
                return;
            }

            //check if username is already occupied

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void DisplayErrorMessage(string message)
        {
            var pop = new PopupInfo
            {
                BindingContext = new PopupInfoViewModel()
                {
                    Message = message,
                    Title = "Incorrect Password"
                }
            };

            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            return;
        }
    }
}
