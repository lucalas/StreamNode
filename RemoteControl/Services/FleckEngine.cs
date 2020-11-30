using Fleck;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Services
{
    class FleckEngine : IRemoteControlEngine
    {
        public void connect()
        {
            WebSocketServer server = new WebSocketServer("ws://0.0.0.0:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => socket.Send(message);
            });

        }
    }
}
