using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNode.Services
{
    public class HttpServerService
    {
        WebServer server;
        int port;

        public HttpServerService(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            server = new WebServer(o => o
            // TODO retrieve ip from network config 
                    .WithUrlPrefix($"http://192.168.1.198:{port}")
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithStaticFolder("/", "F:/Progetti/StreamNode/StreamNode/WebClient/build/", true, m => m
                    .WithContentCaching(true))
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));
            server.RunAsync();
        }
    }
}
