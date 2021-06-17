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

        public AuthenticationService(IAuthenticationRepository repository)
        {
            _repository = repository;
        }
        public async Task<LoginResponse> LoginUser(LoginModel model)
        {
            return await _repository.Login(model);
        }

        public async Task<RegisterResponse> RegisterUser(RegisterModel model)
        {
            return await _repository.Register(model);
        }

    }
}
