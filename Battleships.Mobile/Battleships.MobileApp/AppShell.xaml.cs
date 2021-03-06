using Battleships.MobileApp.ViewModels;
using Battleships.MobileApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Battleships.MobileApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
            Routing.RegisterRoute(nameof(NewLobby), typeof(NewLobby));
            Routing.RegisterRoute(nameof(JoinLobby), typeof(JoinLobby));
            Routing.RegisterRoute(nameof(PhotosListPage), typeof(PhotosListPage));
            Routing.RegisterRoute(nameof(LeaderboardPage), typeof(LeaderboardPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
