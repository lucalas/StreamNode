using Fleck;
using System.Diagnostics;
using StreamNodeEngine.Objects;
using static StreamNodeEngine.Engine.IRemoteControlEngine;

namespace StreamNodeEngine.Engine
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

        public void Disconnect() {
            _server.Dispose();
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

        public void SendMessage(string data) {
            if (_socket != null)
            {
                _socket.Send(data);
            }
        }

        private void HandlerMessage(string message)
        {
            if (_socket != null)
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
}
