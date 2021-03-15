using StreamNodeEngine.Objects;

namespace StreamNodeEngine.Engine
{
    interface IRemoteControlEngine
    {
        delegate string OnMessageEventHandler(object sender, RemoteControlOnMessageArgs args);
        event OnMessageEventHandler OnMessage;

        void Connect();
        void SendMessage(string data);
    }
}
