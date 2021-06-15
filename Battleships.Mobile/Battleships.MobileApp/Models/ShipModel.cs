using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Models
{
    public class ShipModel : ObservableProperty
    {
        public bool IsVertical { get; set; }
        public List<TileModel> ShipTiles{ get; set; }

        public ShipModel()
        {
            ShipTiles = new List<TileModel>();
        }
    }
}
