using Fleck;
using System.Diagnostics;
using StreamNodeEngine.Objects;
using static StreamNodeEngine.Engine.IRemoteControlEngine;
using StreamNodeEngine.Engine.Services;

namespace StreamNodeEngine.Engine
{
    class FleckEngine : IRemoteControlEngine
    {
        private string ip = "0.0.0.0";
        private string port = "8189";
        private WebSocketServer _server;
        private IWebSocketConnection _socket;
        public event OnMessageEventHandler OnMessage;

        public string wsUrl {get {return $"ws://{ip}:{port}";}}

        public void Connect()
        {
            _server = new WebSocketServer(wsUrl);
            
            _server.Start(Configure);
        }

        public void Disconnect() {
            _server.Dispose();
        }

        public void Configure(IWebSocketConnection socket)
        {
            _socket = socket;
            _socket.OnOpen = () => {
                LogRedirector.info($"WebSocket connected [{wsUrl}]");
            };
            _socket.OnClose = () => {
                LogRedirector.info($"WebSocket disconnected [{wsUrl}]");
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
