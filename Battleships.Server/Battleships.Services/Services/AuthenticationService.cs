using Battleships.Models.Dtos;
using Battleships.Models.Models;
using Battleships.Repositories.Repositories.Interfaces;
using Battleships.Services.Services.Interfaces;
//using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IPlayerService _player;

        public AuthenticationService(IAuthenticationRepository repository, IPlayerService player)
        {
            _repository = repository;
            _player = player;
        }
        public async Task<LoginResponse> LoginUser(LoginModel model)
        {
            return await _repository.Login(model);
        }

        public async Task<RegisterResponse> RegisterUser(RegisterModel model)
        {
            PlayerDto newPlayer = new PlayerDto()
            {
                Username = model.Username,
                Wins = 0
            };
            await _player.CreatePlayer(newPlayer);

            return await _repository.Register(model);
        }

    }
}
