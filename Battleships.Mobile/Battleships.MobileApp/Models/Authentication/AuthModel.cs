using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Models.Authentication
{
    public class AuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public AuthModel()
        {

        }

        public AuthModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
