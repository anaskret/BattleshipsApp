using Battleships.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services.Services.Interfaces
{
    public interface ILobbyService
    {
        Task<List<LobbyDto>> GetAll();
        Task<LobbyDto> GetLobbyById(int id);
        Task<LobbyDto> GetLobbyByName(string name);
        Task<LobbyDto> CreateLobby(LobbyDto lobby);
        Task UpdateLobby(LobbyDto lobby);
        Task DeleteLobby(int id);
    }
}
