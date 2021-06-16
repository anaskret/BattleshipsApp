using Battleships.Models.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships.Services.Services.Interfaces; 

namespace Battleships.WebApi.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly Services.Services.Interfaces.IAuthenticationService _service;

        public AuthenticationController(Services.Services.Interfaces.IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var response = await _service.LoginUser(model);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var response = await _service.RegisterUser(model);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
