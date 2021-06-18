using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Entities.Lobby
{
    public class Lobby : BaseEntity
    {
        public string Name { get; set; }
        public string PlayerOneId { get; set; }
        public string PlayerTwoId { get; set; }
    }
}
