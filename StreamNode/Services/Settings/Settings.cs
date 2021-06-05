using Config.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNode.Services.Settings
{
    class Settings : ISettings
    {
        /*
            Settings Class, this is required because we need to instatiate the interface
         */
         
        [Option(DefaultValue = 0)]
        public int WebSocketPort { get; set; }
        [Option(DefaultValue = "")]
        public string HttpServerIp { get; set; }
        [Option(DefaultValue = 0)]
        public int HttpServerPort { get; set; }

        // TODO add the other default settings


        public Settings() {}
    }
}
