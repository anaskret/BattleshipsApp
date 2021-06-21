using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Models
{
    public class LobbyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
    }
}
