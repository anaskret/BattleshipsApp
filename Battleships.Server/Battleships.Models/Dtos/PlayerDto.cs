using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Wins { get; set; }
    }
}
