using Battleships.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.WebApi.Controllers
{
    public class ExampleController : Controller
    {
        private readonly IExampleService _exampleService;

        public ExampleController(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }

        [HttpGet("api/example")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _exampleService.GetExampleList());
        }
    }
}
