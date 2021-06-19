using Fleck;
using StreamNodeEngine.Objects;
using static StreamNodeEngine.Engine.Services.WebSocket.IRemoteControlEngine;

namespace StreamNodeEngine.Engine.Services.WebSocket
{
    class FleckEngine : IRemoteControlEngine
    {
        public IWebSocketSettings settings { get; set; }
        private WebSocketServer _server;
        private IWebSocketConnection _socket;
        public event OnMessageEventHandler OnMessage;

        public string wsUrl { get { return $"ws://{settings.WebSocketIp}:{settings.WebSocketPort}"; } }

        public void Connect()
        {
            _server = new WebSocketServer(wsUrl);

            _server.Start(Configure);
        }

        public void Disconnect()
        {
            _server.Dispose();
        }

        public void Configure(IWebSocketConnection socket)
        {
            _socket = socket;
            _socket.OnOpen = () =>
            {
                LogRedirector.info($"WebSocket connected [{wsUrl}]");
            };
            _socket.OnClose = () =>
            {
                LogRedirector.info($"WebSocket disconnected [{wsUrl}]");
            };
            _socket.OnMessage = HandlerMessage;
        }

        public void SendMessage(string data)
        {
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
