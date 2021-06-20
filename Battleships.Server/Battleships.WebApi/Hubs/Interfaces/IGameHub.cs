using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.WebApi.Hubs.Interfaces
{
    public interface IGameHub
    {
        Task Shoot(int lobbyId, int x, int y, string player);
        Task GridStatus(int lobbyId, int x, int y, int status);
        Task JoinGame(int lobbyId);
        Task LeaveGame(int lobbyId);
        Task Ready(int lobbyId);
        Task Start(int lobbyId);
        Task Victory(int lobbyId);
    }
}
