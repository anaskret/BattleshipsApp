using Battleships.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<List<PlayerDto>> GetAll();
        Task<PlayerDto> GetPlayerByName(string name);
        Task<PlayerDto> CreatePlayer(PlayerDto player);
        Task UpdatePlayer(string name);
        Task DeletePlayer(int id);
    }
}
