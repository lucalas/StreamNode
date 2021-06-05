using System.ComponentModel;
namespace StreamNode.Services.Settings
{
    public interface ISettings
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

        [DefaultValue("127.0.0.1")]
        string ObsIp {get; set;}

        [DefaultValue(4444)]
        int ObsPort {get; set;}

        [DefaultValue("")]
        string ObsPassword {get; set;}
    }
}
