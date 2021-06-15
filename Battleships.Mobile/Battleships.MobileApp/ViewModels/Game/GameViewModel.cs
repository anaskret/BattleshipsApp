using Battleships.MobileApp.Models;
using Battleships.MobileApp.Models.Enums;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Battleships.MobileApp.ViewModels.Game
{
    public class GameViewModel : BaseViewModel
    {
        private ObservableCollection<TileModel> _tiles;
        public ObservableCollection<TileModel> Tiles { get => _tiles; set => SetProperty(ref _tiles, value); }

        public List<ShipModel> Ships { get; set; }

        //Carriers, Battleships, Cruisers, Submarines, Destroyers
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

        private bool _isReady;
        public bool IsReady { get => _isReady; set => SetProperty(ref _isReady, value); }

        private ShipsEnum _currentShipForPlacement;

        private bool isInPlacement = true;


        private TileModel _selectedTile;
        public TileModel SelectedTile 
        { 
            get => _selectedTile;
            set
            {
                if (value == null)
                    return;
                if (!isInPlacement)
                {
                    _selectedTile = value;
                    //Shoot()
                }
                if(isInPlacement && value.IsShip)
                {
                    _selectedTile = value;
                    RotateShip();
                }
                if(isInPlacement)
                {
                    _selectedTile = value;
                    PlaceShip();
                }

                _selectedTile = null;
                OnPropertyChanged();
            }
        }

        public Command PlaceCarrierCommand{ get; set; }
        public Command PlaceBattleshipCommand{ get; set; }
        public Command PlaceCruiserCommand{ get; set; }
        public Command PlaceSubmarineCommand{ get; set; }
        public Command PlaceDestroyerCommand{ get; set; }
        public Command ClearCommand{ get; set; }
        public Command ReadyCommand{ get; set; }

        public GameViewModel()
        {
            Tiles = new ObservableCollection<TileModel>();
            Ships = new List<ShipModel>();

            for (int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    Tiles.Add(new TileModel() { X = j, Y = i});
                }
            }

            PlaceCarrierCommand = new Command(PlaceCarrier);
            PlaceBattleshipCommand = new Command(PlaceBattleship);
            PlaceCruiserCommand = new Command(PlaceCruiser);
            PlaceSubmarineCommand = new Command(PlaceSubmarine);
            PlaceDestroyerCommand = new Command(PlaceDestroyer);
        }

        private void PlaceCarrier()
        {
            if (CarriersLeftCount <= 0)
                return;
            _currentShipForPlacement = ShipsEnum.Carrier;
        }
        
        private void PlaceBattleship()
        {
            _currentShipForPlacement = ShipsEnum.Battleship;

        }

        private void PlaceCruiser()
        {
            _currentShipForPlacement = ShipsEnum.Cruiser;

        }

        private void PlaceSubmarine()
        {
            _currentShipForPlacement = ShipsEnum.Submarine;

        }

        private void PlaceDestroyer()
        {
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

                    if (isValidForPlacement)
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
                    var tile = Tiles.FirstOrDefault(prp => prp.X == coordinate && prp.Y == SelectedTile.Y);

                    tile.IsShip = true;
                    ship.ShipTiles.Add(tile);
                    tile.Ship = ship;
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

                SelectedTile.Ship.ShipTiles = new List<TileModel>();

                foreach (var coordinate in Coordinates)
                {
                    TileModel tile;
                    if(!SelectedTile.Ship.IsVertical)
                        tile = Tiles.FirstOrDefault(prp => prp.Y == coordinate && prp.X == startTile.X);
                    else
                        tile = Tiles.FirstOrDefault(prp => prp.X == coordinate && prp.Y == startTile.Y);

                    tile.IsShip = true;
                    SelectedTile.Ship.ShipTiles.Add(tile);
                    tile.Ship = SelectedTile.Ship;
                }
                SelectedTile.Ship.IsVertical = !SelectedTile.Ship.IsVertical;

                //if(CarriersLeftCount <= 0 && BattleshipsLeftCount <= 0 && CruisersLeftCount <= 0 && SubmarinesLeftCount <= 0 && DestroyersLeftCount <= 0)
            }
        }

        private bool ValidateShipXCoordinates(TileModel startTile, int size)
        {
            if (Tiles.Any(prp => prp.X == startTile.X - 1 && prp.Y == startTile.Y && prp.IsShip))
                return false;
            if (Tiles.Any(prp => prp.X == startTile.X + size && prp.Y == startTile.Y && prp.IsShip))
                return false;

            for (int i = 0; i < size; i++)
            {
                if (Tiles.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y && prp.IsShip))
                    return false;
                if (Tiles.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y + 1 && prp.IsShip))
                    return false;
                if (Tiles.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y - 1 && prp.IsShip))
                    return false;
                if (startTile.X + i > 9)
                    return false;
            }

            return true;
        }
        
        private bool ValidateRotateShipXCoordinates(TileModel startTile, int size)
        {
            if (Tiles.Any(prp => prp.X == startTile.X - 1 && prp.Y == startTile.Y && prp.IsShip))
                return false;
            if (Tiles.Any(prp => prp.X == startTile.X + size && prp.Y == startTile.Y && prp.IsShip))
                return false;

            for (int i = 0; i < size; i++)
            {
                if (i > 0)
                {
                    if (Tiles.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y && prp.IsShip))
                        return false;
                    if (Tiles.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y + 1 && prp.IsShip))
                        return false;
                    if (Tiles.Any(prp => prp.X == startTile.X + i && prp.Y == startTile.Y - 1 && prp.IsShip))
                        return false;
                }
                if (startTile.X + i > 9)
                    return false;
            }

            return true;
        }
        
        private bool ValidateShipYCoordinates(TileModel startTile, int size)
        {
            if (Tiles.Any(prp => prp.X == startTile.Y - 1 && prp.Y == startTile.X && prp.IsShip))
                return false;
            if (Tiles.Any(prp => prp.X == startTile.Y + size && prp.Y == startTile.X && prp.IsShip))
                return false;

            for (int i = 0; i < size; i++)
            {
                if (i > 0)
                {
                    if (Tiles.Any(prp => prp.Y == startTile.Y + i && prp.X == startTile.X && prp.IsShip))
                        return false;
                    if (Tiles.Any(prp => prp.Y == startTile.Y + i && prp.X == startTile.X + 1 && prp.IsShip))
                        return false;
                    if (Tiles.Any(prp => prp.Y == startTile.Y + i && prp.X == startTile.X - 1 && prp.IsShip))
                        return false;
                }
                if (startTile.Y + i > 9)
                    return false;
               // xCoordinates.Add(startTile.Y + i);
            }

            return true;
        }
    }
}
