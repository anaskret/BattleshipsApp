using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.ViewModels
{
    public class PopupInfoViewModel : BaseViewModel
    {
        string _message;
        public string Message { get => _message; set => SetProperty(ref _message, value); }

        public AsyncCommand ClosePopupCommand { get; }

        public PopupInfoViewModel()
        {
            ClosePopupCommand = new AsyncCommand(ClosePopup);
        }


        private async Task ClosePopup()
        {
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}
