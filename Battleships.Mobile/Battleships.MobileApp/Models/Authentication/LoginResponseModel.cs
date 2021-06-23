using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Models.Authentication
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
