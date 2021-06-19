using Battleships.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.WebApi.Controllers
{
    public class GetLeaderboardController : Controller
    {
        private readonly IPlayerService _playerService;
        public GetLeaderboardController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("api/allwins")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var players = await _playerService.GetAll();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/ranking")]
        public async Task<IActionResult> GetRanking()
        {
            try
            {
                var players = await _playerService.GetAll();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
