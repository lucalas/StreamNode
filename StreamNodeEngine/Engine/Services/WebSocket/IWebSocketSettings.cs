using System.ComponentModel;
namespace StreamNodeEngine.Engine.Services.WebSocket
{
    public interface IWebSocketSettings
    {
        [DefaultValue("0.0.0.0")]
        int WebSocketIp { get; set; }
        
        [DefaultValue(8189)]
        int WebSocketPort { get; set; }
    }
}