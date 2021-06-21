using Battleships.MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.Services.Lobby
{
    public interface ILobbyService
    {
        Task<LobbyModel> GetLobbyById(int id);
        Task<LobbyModel> GetLobbyByName(string name);
        Task<LobbyModel> CreateLobby(LobbyModel lobby);
        Task UpdateLobby(LobbyModel lobby);
        Task Delete(int id);
    }
}
