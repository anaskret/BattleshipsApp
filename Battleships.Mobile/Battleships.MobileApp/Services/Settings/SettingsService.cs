using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Services.Settings
{
    public class SettingsService
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
        private string _accessRefreshToken = "access_refresh_token";
        private DateTime _accessRefreshTokenExpired = new DateTime();

        string AuthAccessToken 
        { 
            get => _accessToken; 
            set
            {
                if (value == null)
                    return;

                _accessToken = value;
            }
        }
        string AuthAccessRefreshToken 
        { 
            get => _accessRefreshToken; 
            set
            {
                if (value == null)
                    return;

                _accessRefreshToken = value;
            }
        }
        DateTime AuthAccessRefreshExpired { 
            get => _accessRefreshTokenExpired;
            set
            {
                if (value == null)
                    return;

                _accessRefreshTokenExpired = value;
            }
        }
    }
}
