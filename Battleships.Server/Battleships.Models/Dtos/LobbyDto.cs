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
        public string PlayerOneId { get; set; }
        public string PlayerTwoId { get; set; }
    }
}
