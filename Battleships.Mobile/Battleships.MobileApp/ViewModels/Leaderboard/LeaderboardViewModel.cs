using Battleships.MobileApp.Models;
using Battleships.MobileApp.Services.Players;
using Battleships.MobileApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels.Leaderboard
{
    public class LeaderboardViewModel : ViewModelBase
    {
        private IPlayerService _playerService;

        private ObservableCollection<PlayerModel> _leaderboard;
        public ObservableCollection<PlayerModel> Leaderboard{ get => _leaderboard; set => SetProperty(ref _leaderboard, value); }

        private PlayerModel _selectedItem;
        public PlayerModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == null)
                    return;
                _selectedItem = null;
                OnPropertyChanged();
            }
        }

        public LeaderboardViewModel()
        {
            _playerService = DependencyService.Get<IPlayerService>();
        }

        public override async void InitializeAsync()
        {
            Leaderboard = new ObservableCollection<PlayerModel>(await _playerService.GetLeaderboard());

            base.InitializeAsync();
        }
    }
}
