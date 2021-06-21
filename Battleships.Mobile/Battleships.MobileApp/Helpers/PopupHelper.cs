using Battleships.MobileApp.ViewModels;
using Battleships.MobileApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Helpers
{
    public class PopupHelper
    {
        public static async void DisplayErrorMessage(string message, string title)
        {
            var pop = new PopupInfo
            {
                BindingContext = new PopupInfoViewModel()
                {
                    Message = message,
                    Title = title
                }
            };

            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            return;
        }

        public static async void DisplayGameOverMessage(string message, string title, int lobbyId)
        {
            var pop = new PopupGameOver
            {
                BindingContext = new PopupGameOverViewModel()
                {
                    LobbyId = lobbyId,
                    Message = message,
                    Title = title
                }
            };

            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            return;
        }
    }
}
