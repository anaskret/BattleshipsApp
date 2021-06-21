using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Dtos
{
    public class LobbyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
    }
}
