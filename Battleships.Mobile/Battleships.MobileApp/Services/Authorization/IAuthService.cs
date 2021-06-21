using Battleships.MobileApp.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.Services.Authorization
{
    public interface IAuthService
    {
        Task Register(AuthModel model);
        Task Login(AuthModel model);
    }
}
