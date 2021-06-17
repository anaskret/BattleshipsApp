using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp
{
    public class GlobalSetting
    {
        private readonly string _defaultEndpoint = "http://192.168.1.10:5000/api";

        public static GlobalSetting Instance { get; } = new GlobalSetting();

        public string DefaultEndpoint { get { return _defaultEndpoint; } }
    }
}
