using StreamdeckEngine.Objects;

namespace StreamdeckEngine.Engine
{
    interface IRemoteControlEngine
    {
        delegate string OnMessageEventHandler(object sender, RemoteControlOnMessageArgs args);
        event OnMessageEventHandler OnMessage;

        void Connect();
    }
}
