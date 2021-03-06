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
                await Clients.OthersInGroup(lobbyId.ToString()).Ready(lobbyId);
               
            }

            public async Task Start(int lobbyId)
            {
                await Clients.OthersInGroup(lobbyId.ToString()).Start(lobbyId);
            }

            public async Task Victory(int lobbyId)
            {
               await Clients.OthersInGroup(lobbyId.ToString()).Victory(lobbyId);
            }

            public async Task Shoot(int lobbyId, int x, int y, string player)
            {
                await Clients.OthersInGroup(lobbyId.ToString()).Shoot(lobbyId, x, y, player);
            }

            public async Task GridStatus(int lobbyId, int x, int y, int status)
            {
                await Clients.OthersInGroup(lobbyId.ToString()).GridStatus(lobbyId, x, y, status);
            }
        
            public async Task GridStatusShipSunk(int lobbyId, int[] x, int[] y, int status, bool isVertical)
            {
                await Clients.OthersInGroup(lobbyId.ToString()).GridStatusShipSunk(lobbyId, x, y, status, isVertical);
            }
        }
    }

