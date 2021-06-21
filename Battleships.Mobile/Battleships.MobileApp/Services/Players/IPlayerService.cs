using Battleships.MobileApp.Models;
using Battleships.MobileApp.ViewModels.Leaderboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.Services.Players
{
    public interface IPlayerService
    {
        Task Update();
        Task<List<PlayerModel>> GetLeaderboard();
    }
}
