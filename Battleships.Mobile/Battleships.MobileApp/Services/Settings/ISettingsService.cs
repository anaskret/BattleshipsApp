using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Services.Settings
{
    public interface ISettingsService
    {
        string UserName { get; set; }

        string AuthAccessToken { get; set; }
        string AuthAccessRefreshToken { get; set; }
        DateTime AuthAccessRefreshExpired { get; set; }
    }
}
