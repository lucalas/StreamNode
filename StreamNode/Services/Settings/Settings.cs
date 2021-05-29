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
        public int WebSocketPort { get; set; }

        public string HttpServerIp { get; set; }

        public int HttpServerPort { get; set; }

        // TODO add the other default settings


        public Settings() { }
        public Settings(int wsPort, string httpIp, int httpPort)
        {
            this.WebSocketPort = wsPort;
            this.HttpServerIp = httpIp;
            this.HttpServerPort = httpPort;
        }
    }
}
