using Battleships.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Repositories.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<LoginResponse> Login(LoginModel model);
        Task<RegisterResponse> Register(RegisterModel model);


    }
}
