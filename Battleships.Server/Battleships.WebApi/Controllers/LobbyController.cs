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
    public class LobbyController : Controller
    {
        private readonly ILobbyService _lobbyService;
        public LobbyController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        [HttpGet("api/allLobbies")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var lobbies = await _lobbyService.GetAll();
                return Ok(lobbies);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/lobby")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var lobby = await _lobbyService.GetLobbyById(id);
                return Ok(lobby);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("api/lobbyByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var lobby = await _lobbyService.GetLobbyByName(name);
                return Ok(lobby);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/lobby")]
        public async Task<IActionResult> Create([FromBody] LobbyDto lobby)
        {
            try
            {
                var createdLobby = await _lobbyService.CreateLobby(lobby);
                return Ok(createdLobby);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("api/lobby")]
        public async Task<IActionResult> Update([FromBody] LobbyDto lobby)
        {
            try
            {
                await _lobbyService.UpdateLobby(lobby);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("api/lobby")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _lobbyService.DeleteLobby(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
