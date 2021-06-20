using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Entities.Player
{
    public class Player : BaseEntity
    {
        public string Username { get; set; }
        public int Wins { get; set; }

        public virtual ICollection<Battleships.Models.Entities.Lobby.Lobby> Lobbies { get; set; }
    }
}
