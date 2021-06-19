using StreamNodeEngine.Objects;

namespace StreamNodeEngine.Engine.Services.WebSocket
{
    interface IRemoteControlEngine
    {
        IWebSocketSettings settings { get; set; }
        delegate string OnMessageEventHandler(object sender, RemoteControlOnMessageArgs args);
        event OnMessageEventHandler OnMessage;

        void Connect();
        void Disconnect();
        void SendMessage(string data);
    }
}
