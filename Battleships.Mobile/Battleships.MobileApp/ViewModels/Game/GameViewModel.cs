using Battleships.MobileApp.Helpers;
using Battleships.MobileApp.Models;
using Battleships.MobileApp.Models.Enums;
using Battleships.MobileApp.Services.Game;
using Battleships.MobileApp.Services.Lobby;
using Battleships.MobileApp.Services.Settings;
using Battleships.MobileApp.ViewModels.Base;
using Battleships.MobileApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Battleships.MobileApp.ViewModels.Game
{
    [QueryProperty(nameof(LobbyId), nameof(LobbyId))]
    public class GameViewModel : ViewModelBase
    {
        private readonly IGameService _gameService;
        private readonly ILobbyService _lobbyService;
        private readonly ISettingsService _settingsService;

        private GridModel _selectedTile;
        public GridModel SelectedTile 
        { 
            get => _selectedTile;
            set
            {
                if (value == null)
                    return;
                else if(IsInPlacement && value.IsShip)
                {
                    _selectedTile = value;
                    RotateShip();
                }
                else if(IsInPlacement)
                {
                    _selectedTile = value;
                    PlaceShip();
                }

                _selectedTile = null;
                OnPropertyChanged();
            }
        }
        
        private GridModel _selectedOpponentTile;
        public GridModel SelectedOpponentTile 
        { 
            get => _selectedOpponentTile;
            set
            {
                if (value == null || player != turn)
                    return;
                if (!IsInPlacement)
                {
                    _selectedOpponentTile = value;
                    Shoot();
                }

                _selectedOpponentTile = null;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<GridModel> _grids;
        public ObservableCollection<GridModel> Grids { get => _grids; set => SetProperty(ref _grids, value); }
        
        private ObservableCollection<GridModel> _opponentGrids;
        public ObservableCollection<GridModel> OpponentGrids { get => _opponentGrids; set => SetProperty(ref _opponentGrids, value); }

        public List<ShipModel> Ships { get; set; }

        private int _carriersLeftCount = 1;
        public int CarriersLeftCount { get => _carriersLeftCount; set => SetProperty(ref _carriersLeftCount, value); }
        
        private int _battleshipsLeftCount = 2;
        public int BattleshipsLeftCount { get => _battleshipsLeftCount; set => SetProperty(ref _battleshipsLeftCount, value); }
        
        private int _cruisersLeftCount = 3;
        public int CruisersLeftCount { get => _cruisersLeftCount; set => SetProperty(ref _cruisersLeftCount, value); }
        
        private int _submarinesLeftCount = 4;
        public int SubmarinesLeftCount { get => _submarinesLeftCount; set => SetProperty(ref _submarinesLeftCount, value); } 
        
        private int _destroyersLeftCount = 5;
        public int DestroyersLeftCount { get => _destroyersLeftCount; set => SetProperty(ref _destroyersLeftCount, value); }

        private ShipsEnum _currentShipForPlacement;

        private bool _isInPlacement = true;
        public bool IsInPlacement { get => _isInPlacement; set => SetProperty(ref _isInPlacement, value); }

        private int _lobbyId;
        public int LobbyId
        {
            get => _lobbyId;
            set
            {
                SetProperty(ref _lobbyId, Convert.ToInt32(Uri.UnescapeDataString(value.ToString())));
                OnPropertyChanged();
            }
        }

        private bool isOpponentReady;
        private bool isPlayerReady;
        private bool _isReady;
        public bool IsReady { get => _isReady; set => SetProperty(ref _isReady, value); }

        private string player = "p1";
        private string turn = "p1";

        public Command PlaceCarrierCommand{ get; set; }
        public Command PlaceBattleshipCommand{ get; set; }
        public Command PlaceCruiserCommand{ get; set; }
        public Command PlaceSubmarineCommand{ get; set; }
        public Command PlaceDestroyerCommand{ get; set; }
        public Command ClearCommand{ get; set; }
        public MvvmHelpers.Commands.AsyncCommand ReadyCommand{ get; set; }

        public GameViewModel()
        {
            _gameService = DependencyService.Get<IGameService>();
            _lobbyService = DependencyService.Get<ILobbyService>();
            _settingsService = DependencyService.Get<ISettingsService>();

            CreateGrid();
            CreateOpponentGrid();

            PlaceCarrierCommand = new Command(PlaceCarrier);
            PlaceBattleshipCommand = new Command(PlaceBattleship);
            PlaceCruiserCommand = new Command(PlaceCruiser);
            PlaceSubmarineCommand = new Command(PlaceSubmarine);
            PlaceDestroyerCommand = new Command(PlaceDestroyer);
            ClearCommand = new Command(CreateGrid);
            ReadyCommand= new MvvmHelpers.Commands.AsyncCommand(Ready);

            MessagingCenter.Subscribe<GameService, string[]>(this, "OpponentShot", async (sender, args) =>
            {
                var x = Convert.ToInt32(args[0]);
                var y = Convert.ToInt32(args[1]);
                var grid = Grids.FirstOrDefault(prp => prp.X == x && prp.Y == y);

                grid.GridStatus = GridStatusEnum.Hit;

                if (grid.IsShip)
                {
                    grid.GridStatus = GridStatusEnum.ShipHit;

                    if (!grid.Ship.ShipTiles.Any(prp => prp.GridStatus == GridStatusEnum.NotHit))
                    { 
                        grid.Ship.ShipTiles.ForEach(prp => prp.GridStatus = GridStatusEnum.Sunk);
                        grid.GridStatus = GridStatusEnum.Sunk;
                    }

                    await _gameService.GridStatus(LobbyId, x, y, (int)grid.GridStatus);

                    foreach(var ship in Ships)
                    {
                        if(ship.ShipTiles.Any(prp => prp.GridStatus == GridStatusEnum.Sunk))
                        {
                            await _gameService.Victory(LobbyId);
                            PopupHelper.DisplayGameOverMessage("You lost :(", "Game Over", LobbyId);
                            return;
                        }
                    }
                    return;
                }
                turn = player;

                await _gameService.GridStatus(LobbyId, x, y, (int)grid.GridStatus);
            });

            MessagingCenter.Subscribe<GameService, string[]>(this, "GridHitStatus", (sender, args) =>
            {
                var x = Convert.ToInt32(args[0]);
                var y = Convert.ToInt32(args[1]);
                var status = Convert.ToInt32(args[2]);
                var grid = OpponentGrids.FirstOrDefault(prp => prp.X == x && prp.Y == y);

                grid.GridStatus = (GridStatusEnum) status;
                if (status > 2)
                {
                    turn = player;
                }
                if(status == 4)
                {
                    foreach(var tile in grid.Ship.ShipTiles)
                    {
                        tile.GridStatus = GridStatusEnum.Sunk;
                    }
                }
            });

            MessagingCenter.Subscribe<GameService>(this, "OpponentReady", async (sender) =>
            {
                isOpponentReady = true;

                if (IsReady)
                {
                    await _gameService.Start(LobbyId);
                    IsInPlacement = false;
                }
            });
            
            MessagingCenter.Subscribe<GameService>(this, "StartGame", (sender) =>
            {
                isOpponentReady = true;
                IsInPlacement = false;
            });
            
            MessagingCenter.Subscribe<GameService>(this, "YouWon", async (sender) =>
            {
                //increment player wins
                PopupHelper.DisplayGameOverMessage("You won! :D", "Game Over", LobbyId);
            });
            
        }

        public override async void InitializeAsync()
        {
            await _gameService.Connect();

            var lobby = await _lobbyService.GetLobbyById(LobbyId);
            await _gameService.JoinGame(LobbyId);

            if (lobby.PlayerOne == _settingsService.UserName)
            {
                player = "p1";

                base.InitializeAsync();
                return;
            }

            player = "p2";
            base.InitializeAsync();
        }

        private async Task Shoot()
        {
            if(player != turn)
            {
                PopupHelper.DisplayErrorMessage("Wait for your turn!", "Game Message");
                return;
            }
            if (SelectedOpponentTile.IsShip)
            {
                PopupHelper.DisplayErrorMessage("Select another tile, this one is already hit!", "Game Message");
            }
            await _gameService.Shoot(LobbyId, SelectedOpponentTile.X, SelectedOpponentTile.Y, player);
        }

        private void CreateGrid()
        {
            Grids = new ObservableCollection<GridModel>();
            Ships = new List<ShipModel>();

            CarriersLeftCount = 1;
            /*BattleshipsLeftCount = 2;
            CruisersLeftCount = 3;
            SubmarinesLeftCount = 4;
            DestroyersLeftCount = 5;*/
            BattleshipsLeftCount = 0;
            CruisersLeftCount = 0;
            SubmarinesLeftCount = 0;
            DestroyersLeftCount = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Grids.Add(new GridModel() { X = j, Y = i });
                }
            }
        }

        private async Task Ready()
        {
            isPlayerReady = true;
            if(isPlayerReady && isOpponentReady)
            {
                await _gameService.Start(LobbyId);
                IsInPlacement = false;
                return;
            }

            await _gameService.Ready(LobbyId);
        }

        private void CreateOpponentGrid()
        {
            OpponentGrids = new ObservableCollection<GridModel>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    OpponentGrids.Add(new GridModel() { X = j, Y = i });
                }
            }
        }

        private void PlaceCarrier()
        {
            if (CarriersLeftCount <= 0)
                return;
            _currentShipForPlacement = ShipsEnum.Carrier;
        }
        
        private void PlaceBattleship()
        {
            if (BattleshipsLeftCount <= 0)
                return;
            _currentShipForPlacement = ShipsEnum.Battleship;

        }

        private void PlaceCruiser()
        {
            if (CruisersLeftCount <= 0)
                return;
            _currentShipForPlacement = ShipsEnum.Cruiser;
        }

        private void PlaceSubmarine()
        {
            if (SubmarinesLeftCount <= 0)
                return;
            _currentShipForPlacement = ShipsEnum.Submarine;

        }

        private void PlaceDestroyer()
        {
            if (DestroyersLeftCount <= 0)
                return;
            _currentShipForPlacement = ShipsEnum.Destroyer;

        }

        private void PlaceShip()
        {
            bool isValidForPlacement = true;
            var xCoordinates = new List<int>();
            int shipSize;

            switch (_currentShipForPlacement)
            {
                case ShipsEnum.Carrier:
                    if (CarriersLeftCount <= 0)
                        return;
                    shipSize = 5;
                    isValidForPlacement = ValidateShipXCoordinates(SelectedTile, shipSize);

                    if (!isValidForPlacement)
                        return;

                    for (int i = 0; i < shipSize; i++)
                    {
                        xCoordinates.Add(SelectedTile.X + i);
                    }

                    CarriersLeftCount--;
                    break;
                case ShipsEnum.Battleship:
                    if (BattleshipsLeftCount <= 0)
                        return;
                    shipSize = 4;
                    isValidForPlacement = ValidateShipXCoordinates(SelectedTile, shipSize);

                    if (!isValidForPlacement)
                        return;

                    for (int i = 0; i < shipSize; i++)
                    {
                        xCoordinates.Add(SelectedTile.X + i);
                    }

                    BattleshipsLeftCount--;
                    break;
                case ShipsEnum.Cruiser:
                    if (CruisersLeftCount <= 0)
                        return;
                    shipSize = 3;
                    isValidForPlacement = ValidateShipXCoordinates(SelectedTile, shipSize);

                    if (!isValidForPlacement)
                        return;

                    for (int i = 0; i < shipSize; i++)
                    {
                        xCoordinates.Add(SelectedTile.X + i);
                    }

                    CruisersLeftCount--;
                    break;
                case ShipsEnum.Submarine:
                    if (SubmarinesLeftCount <= 0)
                        return;
                    shipSize = 2;
                    isValidForPlacement = ValidateShipXCoordinates(SelectedTile, shipSize);

                    if (!isValidForPlacement)
                        return;

                    for (int i = 0; i < shipSize; i++)
                    {
                        xCoordinates.Add(SelectedTile.X + i);
                    }

                    SubmarinesLeftCount--;
                    break;
                case ShipsEnum.Destroyer:
                    if (DestroyersLeftCount <= 0)
                        return;
                    shipSize = 1;
                    isValidForPlacement = ValidateShipXCoordinates(SelectedTile, shipSize);

                    if (!isValidForPlacement)
                        return;

                    for (int i = 0; i < shipSize; i++)
                    {
                        xCoordinates.Add(SelectedTile.X + i);
                    }

                    DestroyersLeftCount--;
                    break;
            }

            if (isValidForPlacement)
            {
                var ship = new ShipModel();
                foreach(var coordinate in xCoordinates)
                {
                    var grid = Grids.FirstOrDefault(prp => prp.X == coordinate && prp.Y == SelectedTile.Y);

                    grid.IsShip = true;
                    ship.ShipTiles.Add(grid);
                    grid.Ship = ship;
                    Ships.Add(ship);
                }

            }

            if (CarriersLeftCount <= 0 && BattleshipsLeftCount <= 0 && CruisersLeftCount <= 0 && SubmarinesLeftCount <= 0 && DestroyersLeftCount <= 0)
                IsReady = true;
        }

        private void RotateShip()
        {
            bool isValidForPlacement = true;
            var Coordinates = new List<int>();
            int shipSize;
            var startTile = SelectedTile.Ship.ShipTiles.FirstOrDefault(tile => tile.X == SelectedTile.Ship.ShipTiles.Min(prp => prp.X) && tile.Y == SelectedTile.Ship.ShipTiles.Min(prp => prp.Y));

            shipSize = SelectedTile.Ship.ShipTiles.Count();
            if (!SelectedTile.Ship.IsVertical)
            {
                isValidForPlacement = ValidateShipYCoordinates(startTile, shipSize);

                if (!isValidForPlacement)
                    return;

                for (int i = 0; i < shipSize; i++)
                {
                    Coordinates.Add(SelectedTile.Y + i);
                }
            }
            else
            {
                isValidForPlacement = ValidateRotateShipXCoordinates(startTile, shipSize);

                if (!isValidForPlacement)
                    return;

                for (int i = 0; i < shipSize; i++)
                {
                    Coordinates.Add(SelectedTile.X + i);
                }
            }

            if (isValidForPlacement)
            {
                foreach (var tile in SelectedTile.Ship.ShipTiles)
                {
                    tile.IsShip = false;
                }

                SelectedTile.Ship.ShipTiles = new List<GridModel>();

                foreach (var coordinate in Coordinates)
                {
                    GridModel grid;
                    if(!SelectedTile.Ship.IsVertical)
                        grid = Grids.FirstOrDefault(prp => prp.Y == coordinate && prp.X == startTile.X);
                    else
                        grid = Grids.FirstOrDefault(prp => prp.X == coordinate && prp.Y == startTile.Y);

                    grid.IsShip = true;
                    SelectedTile.Ship.ShipTiles.Add(grid);
                    grid.Ship = SelectedTile.Ship;
                }
                SelectedTile.Ship.IsVertical = !SelectedTile.Ship.IsVertical;

                //if(CarriersLeftCount <= 0 && BattleshipsLeftCount <= 0 && CruisersLeftCount <= 0 && SubmarinesLeftCount <= 0 && DestroyersLeftCount <= 0)
            }
        }

        private bool ValidateShipXCoordinates(GridModel startTile, int size)
        {
            if (Grids.Any(prp => prp.X == startTile.X - 1 && prp.Y == startTile.Y && prp.IsShip))
                return false;
            if (Grids.Any(prp => prp.X == startTile.X + size && prp.Y == startTile.Y && prp.IsShip))
                return false;

            for (int i = -1; i <= size; i++)
            {
                if (Grids.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y && prp.IsShip))
                    return false;
                if (Grids.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y + 1 && prp.IsShip))
                    return false;
                if (Grids.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y - 1 && prp.IsShip))
                    return false;
                if (startTile.X + i > 9 && i < size)
                    return false;
            }

            return true;
        }
        
        private bool ValidateRotateShipXCoordinates(GridModel startTile, int size)
        {
            if (Grids.Any(prp => prp.X == startTile.X - 1 && prp.Y == startTile.Y && prp.IsShip))
                return false;
            if (Grids.Any(prp => prp.X == startTile.X + size && prp.Y == startTile.Y && prp.IsShip))
                return false;

            for (int i = -1; i <= size; i++)
            {
                if (i > 0)
                {
                    if (Grids.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y && prp.IsShip))
                        return false;
                    if (Grids.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y + 1 && prp.IsShip))
                        return false;
                    if (Grids.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y - 1 && prp.IsShip))
                        return false;
                }
                if (startTile.X + i > 9 && i < size)
                    return false;
            }

            return true;
        }
        
        private bool ValidateShipYCoordinates(GridModel startTile, int size)
        {
            if (Grids.Any(prp => prp.X == startTile.Y - 1 && prp.Y == startTile.X && prp.IsShip))
                return false;
            if (Grids.Any(prp => prp.X == startTile.Y + size && prp.Y == startTile.X && prp.IsShip))
                return false;

            for (int i = -1; i <= size; i++)
            {
                if (i > 0)
                {
                    if (Grids.Any(prp => prp.Y == startTile.Y + i && prp.X == startTile.X && prp.IsShip))
                        return false;
                    if (Grids.Any(prp => prp.Y == startTile.Y + i && prp.X == startTile.X + 1 && prp.IsShip))
                        return false;
                    if (Grids.Any(prp => prp.Y == startTile.Y + i && prp.X == startTile.X - 1 && prp.IsShip))
                        return false;
                }
                if (startTile.Y + i > 9 && i < size)
                    return false;
               // xCoordinates.Add(startTile.Y + i);
            }

            return true;
        }
    }
}
