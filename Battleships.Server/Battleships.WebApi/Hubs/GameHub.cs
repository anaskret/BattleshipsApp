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
            public async Task JoinGame(int lobbyId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId.ToString());
            }

            public async Task LeaveGame(int lobbyId)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId.ToString());
            }

            public async Task Ready(int lobbyId)
            {
                //await Clients.OthersInGroup(lobbyId.ToString()).Ready(lobbyId);
                await Clients.All.Ready(lobbyId);
            }

            public async Task Start(int lobbyId)
            {
                //await Clients.OthersInGroup(lobbyId.ToString()).Start(lobbyId);
                await Clients.All.Start(lobbyId);
            }

            public async Task Victory(int lobbyId)
            {
                //await Clients.OthersInGroup(lobbyId.ToString()).Victory(lobbyId);
                await Clients.All.Victory(lobbyId);
            }

            public async Task Shoot(int lobbyId, int x, int y, string player)
            {
                var turn = "p1";
                if (player == "p1")
                    turn = "p2";
                else
                    turn = "p1";

                await Clients.All.Shoot(lobbyId, x, y, turn);
            }

            public async Task GridStatus(int lobbyId, int x, int y, int status)
            {
                await Clients.All.GridStatus(lobbyId, x, y, status);
            }
        }
    }

