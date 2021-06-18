using Battleships.DataAccess;
using Battleships.Models.Entities.Lobby;
using Battleships.Repositories.Repositories.Generic;
using Battleships.Repositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Repositories.Repositories
{
    public class LobbyRepository : Repository<Lobby>, ILobbyRepository
    {
        public LobbyRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Lobby> GetByNameAsync(string name)
        {
            return await _dbContext.Lobbies.FirstOrDefaultAsync(prp => prp.Name == name);
        }
    }
}
