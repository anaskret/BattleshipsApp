using Battleships.Models.Entities.Player;
using Battleships.Repositories.Repositories.Interfaces.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Repositories.Repositories.Interfaces
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<Player> GetByNameAsync(string name);
    }
}
