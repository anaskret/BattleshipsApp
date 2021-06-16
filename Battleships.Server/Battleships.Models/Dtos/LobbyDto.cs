using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Dtos
{
    class LobbyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PlayerOneId { get; set; }
        public int? PlayerTwoId { get; set; }
    }
}
