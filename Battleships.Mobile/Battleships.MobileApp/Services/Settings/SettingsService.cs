using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private string _userName;
        public string UserName 
        {
            get => _userName; 
            set 
            {
                if (value == null)
                    return;

                _userName = value; 
            } 
        }

        private string _accessToken = "access_token";

        public string AuthAccessToken 
        { 
            get => _accessToken; 
            set
            {
                if (value == null)
                    return;

                _accessToken = value;
            }
        }
    }
}
