using Battleships.Models.Dtos;
using Battleships.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.WebApi.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;
        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("api/allplayers")]
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

        [HttpGet("api/playerByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var player = await _playerService.GetPlayerByName(name);
                return Ok(player);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/player")]
        public async Task<IActionResult> Create([FromBody] PlayerDto player)
        {
            try
            {
                var createdPlayer = await _playerService.CreatePlayer(player);
                return Ok(createdPlayer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("api/player")]
        public async Task<IActionResult> Update([FromBody] string username)
        {
            try
            {
                await _playerService.UpdatePlayer(username);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("api/player")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _playerService.DeletePlayer(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
