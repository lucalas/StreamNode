using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RemoteControl.Services
{
    class FleckEngine : IRemoteControlEngine
    {
        private WebSocketServer _server;
        private IWebSocketConnection _socket;

        public void Connect()
        {
            _server = new WebSocketServer("ws://0.0.0.0:8181");
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

        void HandlerMessage(String message)
        {
            Trace.WriteLine("printing message:");
            Trace.WriteLine(message);
            _socket.Send(message);
        }
    }
}
