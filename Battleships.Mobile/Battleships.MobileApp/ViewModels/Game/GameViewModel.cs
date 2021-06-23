using Battleships.MobileApp.Helpers;
using Battleships.MobileApp.Models;
using Battleships.MobileApp.Models.Enums;
using Battleships.MobileApp.Services.Game;
using Battleships.MobileApp.Services.Lobby;
using Battleships.MobileApp.Services.Players;
using Battleships.MobileApp.Services.Settings;
using Battleships.MobileApp.ViewModels.Base;
using Battleships.MobileApp.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly IPlayerService _playerService;

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
                if (value == null)
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
            _playerService = DependencyService.Get<IPlayerService>();

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
                    await _gameService.GridStatus(LobbyId, x, y, (int)grid.GridStatus);

                    if (!grid.Ship.ShipTiles.Any(prp => prp.GridStatus == GridStatusEnum.NotHit))
                    {
                        var shipSize = grid.Ship.ShipTiles.Count;
                        int[] xCoordinates = new int[shipSize];
                        int[] yCoordinates = new int[shipSize];

                        for(int i = 0; i < grid.Ship.ShipTiles.Count; i++)
                        {
                            grid.Ship.ShipTiles[i].GridStatus = GridStatusEnum.Sunk;
                            xCoordinates[i] = grid.Ship.ShipTiles[i].X;
                            yCoordinates[i] = grid.Ship.ShipTiles[i].Y;
                        }

                        grid.GridStatus = GridStatusEnum.Sunk;
                        DestroyTilesAroundShip(grid.Ship.IsVertical, Grids, grid.Ship.ShipTiles.FirstOrDefault(), grid.Ship.ShipTiles.Count);

                        await _gameService.GridStatusShipSunk(LobbyId, xCoordinates, yCoordinates, (int)grid.GridStatus, grid.Ship.IsVertical);
                    }

                    foreach(var ship in Ships)
                    {
                        if(ship.ShipTiles.Any(prp => prp.GridStatus != GridStatusEnum.Sunk))
                        {
                            return;
                        }
                    }
                    await _gameService.Victory(LobbyId);
                    PopupHelper.DisplayGameOverMessage("You lost :(", "Game Over", LobbyId);
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
                    grid.IsShip = true;
                    turn = player;
                    return;
                }
                if(player == "p1")
                {
                    turn = "p2";
                    return;
                }
                turn = "p1";
                /*if(status == 4)
                {
                    foreach (var tile in grid.Ship.ShipTiles)
                    {
                        tile.GridStatus = GridStatusEnum.Sunk;
                    }

                    DestroyTilesAroundShip(grid.Ship.IsVertical, OpponentGrids, grid.Ship.ShipTiles.FirstOrDefault(), grid.Ship.ShipTiles.Count);
                }*/
            });
            
            MessagingCenter.Subscribe<GameService, string[]>(this, "GridHitStatusShipSunk", (sender, args) =>
            {
                var param = args[0];
                var param1 = args[0].ToArray();
                int[] x = Array.ConvertAll(args[0].ToArray(), prp => (int)char.GetNumericValue(prp));
                int[] y = Array.ConvertAll(args[1].ToArray(), prp => (int)char.GetNumericValue(prp));
                var status = Convert.ToInt32(args[2]);
                bool isVertical = Convert.ToBoolean(args[3]);

                DestroyTilesAroundOpponentShip(isVertical, x, y, x.Length);
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
                try
                {
                    await _playerService.Update();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                PopupHelper.DisplayGameOverMessage("You won! :D", "Game Over", LobbyId);
            });
            
        }

        public override async void InitializeAsync()
        {
            CreateGrid();
            CreateOpponentGrid();

            try
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
            catch(Exception ex)
            {
                await Shell.Current.Navigation.PopToRootAsync();
            }
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
            BattleshipsLeftCount = 1;
            CruisersLeftCount = 1;
            SubmarinesLeftCount = 1;
            DestroyersLeftCount = 1;
            /*BattleshipsLeftCount = 0;
            CruisersLeftCount = 0;
            SubmarinesLeftCount = 0;
            DestroyersLeftCount = 0;*/

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

        private void DestroyTilesAroundShip(bool isVertical,ObservableCollection<GridModel> targetGrid,  GridModel startTile, int size)
        {
            if (isVertical)
            {
                var beforeTile = targetGrid.FirstOrDefault(prp => prp.X == startTile.X && prp.Y == startTile.Y - 1);
                var afterTile = targetGrid.FirstOrDefault(prp => prp.X == startTile.X && prp.Y == startTile.Y + size);

                if(beforeTile != null)
                    beforeTile.GridStatus = GridStatusEnum.Hit;
                if(afterTile != null)
                    afterTile.GridStatus = GridStatusEnum.Hit;

                for(int i = -1; i <= size; i++)
                {
                    var tile1 = targetGrid.FirstOrDefault(prp => prp.X == startTile.X + 1 && prp.Y == startTile.Y + i);
                    var tile2 = targetGrid.FirstOrDefault(prp => prp.X == startTile.X - 1 && prp.Y == startTile.Y + i);
                
                    if(tile1 != null)
                        tile1.GridStatus = GridStatusEnum.Hit;
                    if(tile2 != null)
                    tile2.GridStatus = GridStatusEnum.Hit;
                }

                return;
            }

            var beforeXTile = targetGrid.FirstOrDefault(prp => prp.X == startTile.X - 1 && prp.Y == startTile.Y);
            var afterXTile = targetGrid.FirstOrDefault(prp => prp.X == startTile.X + size && prp.Y == startTile.Y);
            
            if (beforeXTile != null)
                beforeXTile.GridStatus = GridStatusEnum.Hit;
            if (afterXTile != null)
                afterXTile.GridStatus = GridStatusEnum.Hit;

            for (int i = -1; i <= size; i++)
            {
                var tile1 = targetGrid.FirstOrDefault(prp => prp.X == startTile.X + i && prp.Y == startTile.Y + 1);
                var tile2 = targetGrid.FirstOrDefault(prp => prp.X == startTile.X + i && prp.Y == startTile.Y - 1);
                
                if(tile1 != null)
                    tile1.GridStatus = GridStatusEnum.Hit;
                if(tile2 != null)
                    tile2.GridStatus = GridStatusEnum.Hit;
            }
        }
        
        private void DestroyTilesAroundOpponentShip(bool isVertical, int[] x, int[] y, int size)
        {
            if (isVertical)
            {
                var beforeTile = OpponentGrids.FirstOrDefault(prp => prp.X == x[0]  && prp.Y == y[0] - 1);
                var beforeTile1 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] - 1  && prp.Y == y[0] - 1);
                var beforeTile2 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] + 1  && prp.Y == y[0] - 1);
                var afterTile = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] && prp.Y == y[0] + size);
                var afterTile1 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] - 1 && prp.Y == y[0] + size);
                var afterTile2 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] + 1 && prp.Y == y[0] + size);

                if(beforeTile != null)
                    beforeTile.GridStatus = GridStatusEnum.Hit;
                if (beforeTile1 != null)
                    beforeTile1.GridStatus = GridStatusEnum.Hit;
                if(beforeTile2 != null)
                    beforeTile2.GridStatus = GridStatusEnum.Hit;
                if(afterTile != null)
                    afterTile.GridStatus = GridStatusEnum.Hit;
                if(afterTile1 != null)
                    afterTile1.GridStatus = GridStatusEnum.Hit;
                if(afterTile2 != null)
                    afterTile2.GridStatus = GridStatusEnum.Hit;

                for(int i = 0; i <= size; i++)
                {
                    var tile1 = OpponentGrids.FirstOrDefault(prp => prp.X == x[i] + 1 && prp.Y == y[i]);
                    var tile2 = OpponentGrids.FirstOrDefault(prp => prp.X == x[i] - 1 && prp.Y == y[i]);
                
                    if(tile1 != null)
                        tile1.GridStatus = GridStatusEnum.Hit;
                    if(tile2 != null)
                        tile2.GridStatus = GridStatusEnum.Hit;
                }

                return;
            }

            var beforeXTile = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] - 1 && prp.Y == y[0]);
            var beforeXTile1 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] - 1 && prp.Y == y[0] - 1);
            var beforeXTile2 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] - 1 && prp.Y == y[0] + 1);
            var afterXTile = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] + size && prp.Y == y[0]);
            var afterXTile1 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] + size && prp.Y == y[0] + 1);
            var afterXTile2 = OpponentGrids.FirstOrDefault(prp => prp.X == x[0] + size && prp.Y == y[0] - 1);

            if (beforeXTile != null)
                beforeXTile.GridStatus = GridStatusEnum.Hit;
            if (beforeXTile1 != null)
                beforeXTile1.GridStatus = GridStatusEnum.Hit;
            if (beforeXTile2 != null)
                beforeXTile2.GridStatus = GridStatusEnum.Hit;
            if (afterXTile != null)
                afterXTile.GridStatus = GridStatusEnum.Hit;
            if (afterXTile1 != null)
                afterXTile1.GridStatus = GridStatusEnum.Hit;
            if (afterXTile2 != null)
                afterXTile2.GridStatus = GridStatusEnum.Hit;

            if (beforeXTile != null)
                beforeXTile.GridStatus = GridStatusEnum.Hit;
            if (afterXTile != null)
                afterXTile.GridStatus = GridStatusEnum.Hit;

            for (int i = 0; i <= size; i++)
            {
                var tile1 = OpponentGrids.FirstOrDefault(prp => prp.X == x[i] && prp.Y == y[i] + 1);
                var tile2 = OpponentGrids.FirstOrDefault(prp => prp.X == x[i] && prp.Y == y[i] - 1);
                
                if(tile1 != null)
                    tile1.GridStatus = GridStatusEnum.Hit;
                if(tile2 != null)
                    tile2.GridStatus = GridStatusEnum.Hit;
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
