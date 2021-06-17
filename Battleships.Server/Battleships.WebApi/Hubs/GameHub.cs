using Battleships.WebApi.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.WebApi.Hubs
{
    public class GameHub : Hub<IGameHub>
    {
        public async Task Shoot(int lobbyId, int x, int y, string player)
        {
            var turn = "p1";
            if (player == "p1")
                turn = "p2";
            else
                turn = "p1";

            await Clients.Caller.Shoot(lobbyId, x, y, turn);
            await Clients.All.Shoot(lobbyId, x, y, turn);
            //await Clients.Group(lobbyId.ToString()).Shoot(lobbyId, x, y, turn);
        }
        
        public async Task GridStatus(int lobbyId, int x, int y, int status)
        {
            await Clients.All.GridStatus(lobbyId, x, y, status);
        }
    }
}
