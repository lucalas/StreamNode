namespace StreamNode.Services.Settings
{
    public interface ISettings
    {
        /**
            Interface for Config Library to Read Settings
         */
        int WebSocketPort { get; set; }
        string HttpServerIp { get; set; }
        int HttpServerPort { get; set; }
        // TODO add the other default settings
    }
}
