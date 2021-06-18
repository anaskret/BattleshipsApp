using Battleships.Models.Dtos;
using Battleships.Models.Entities.Lobby;
using Battleships.Repositories.Repositories.Interfaces.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Repositories.Repositories.Interfaces
{
    public interface ILobbyRepository: IRepository<Lobby>
    {
        Task<Lobby> GetByNameAsync (string name);
    }
}
