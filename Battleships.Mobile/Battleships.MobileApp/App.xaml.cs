using Battleships.MobileApp.Services.Authorization;
using Battleships.MobileApp.Services.Game;
using Battleships.MobileApp.Services.Lobby;
using Battleships.MobileApp.Services.RequestProvider;
using Battleships.MobileApp.Services.Settings;
using DLToolkit.Forms.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Battleships.MobileApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton<ISettingsService>(new SettingsService());

            DependencyService.Register<IRequestProvider, RequestProvider>();
            DependencyService.Register<IAuthService, AuthService>();
            DependencyService.Register<IGameService, GameService>();
            DependencyService.Register<ILobbyService, LobbyService>();

            FlowListView.Init();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
