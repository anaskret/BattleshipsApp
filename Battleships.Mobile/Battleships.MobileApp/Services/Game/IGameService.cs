using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.Services.Game
{
    public interface IGameService
    {
        Task Connect();
        Task Disconnect();
        Task Shoot(int lobbyId, int x, int y, string player);
        Task GridStatus(int lobbyId, int x, int y, int status);
        Task GridStatusShipSunk(int lobbyId, int[] x, int[] y, int status, bool isVertical);
        Task JoinGame(int lobbyId);
        Task LeaveGame(int lobbyId);
        Task Ready(int lobbyId);
        Task Start(int lobbyId);
        Task Victory(int lobbyId);
    }
}
