using Battleships.DataAccess;
using Battleships.Models.Entities.Player;
using Battleships.Repositories.Repositories.Generic;
using Battleships.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Repositories.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository (AppDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Player> GetByNameAsync(string name)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(prp => prp.Username == name);
        }
    }
}
