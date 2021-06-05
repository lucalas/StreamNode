using Config.Net;

namespace StreamNode.Services.Settings
{
    public interface ISettings
    {
        /**
         * Interface for Config Library to Read Settings
         */
        [Option(DefaultValue=0)]
        int WebSocketPort { get; set; }
        string HttpServerIp { get; set; }
        int HttpServerPort { get; set; }
        // TODO add the other default settings
    }
}
