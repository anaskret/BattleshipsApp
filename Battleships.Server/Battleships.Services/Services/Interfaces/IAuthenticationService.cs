using Battleships.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginUser(LoginModel model);
        Task<RegisterResponse> RegisterUser(RegisterModel model);
    }
}
