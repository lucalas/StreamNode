using System.ComponentModel;
using StreamNodeEngine.Engine.Services.Obs;
using StreamNodeEngine.Engine.Services.WebSocket;

namespace StreamNode.Services.Settings
{
    public interface ISettings : IObsSettings, IWebSocketSettings
    {

        [DefaultValue("*")]
        string HttpServerIp { get; set; }

        [DefaultValue(8000)]
        int HttpServerPort { get; set; }
    }
}
