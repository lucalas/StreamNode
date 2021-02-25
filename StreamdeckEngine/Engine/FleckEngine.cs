using Fleck;
using System.Diagnostics;
using StreamdeckEngine.Objects;
using static StreamdeckEngine.Engine.IRemoteControlEngine;

namespace StreamdeckEngine.Engine
{
    class FleckEngine : IRemoteControlEngine
    {
        private WebSocketServer _server;
        private IWebSocketConnection _socket;
        public event OnMessageEventHandler OnMessage;

        public void Connect()
        {
            _server = new WebSocketServer("ws://0.0.0.0:8189");
            _server.Start(Configure);
        }

        public void Configure(IWebSocketConnection socket)
        {
            _socket = socket;
            _socket.OnOpen = () => {
                Trace.WriteLine("Connected");
            };
            _socket.OnClose = () => {
                Trace.WriteLine("Disconnected");
            };
            _socket.OnMessage = HandlerMessage;
        }

        private void HandlerMessage(string message)
        {
            RemoteControlOnMessageArgs args = new RemoteControlOnMessageArgs();
            args.message = message;
            string response = OnMessage(this, args);

            if (response != null)
            {
                _socket.Send(response);
            }
        }
    }
}
