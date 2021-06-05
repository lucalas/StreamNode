using System.ComponentModel;
using StreamNodeEngine.Engine.Services.Obs;

namespace StreamNode.Services.Settings
{
    public interface ISettings : IObsSettings
    {
        /**
         * Interface for Config Library to Read Settings
         */
        [DefaultValue(8189)]
        int WebSocketPort { get; set; }

        [DefaultValue("*")]
        string HttpServerIp { get; set; }

        [DefaultValue(8000)]
        int HttpServerPort { get; set; }
    }
}
