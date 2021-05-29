using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Files;
using System.Net;
using System.Reflection;

namespace StreamNode.Services
{
    public class HttpServerService
    {
        private static int PORT = 8000;
        WebServer server;
        int port;
        public string url { get { return $"http://*:{port}"; } }

        // Default we use hostname
        // TODO make ip configurable
        public string publicUrl { get { return $"http://{Dns.GetHostName()}:{port}"; } }

        public HttpServerService() : this(PORT)
        {
        }

        public HttpServerService(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            server = new WebServer(o => o
            // TODO retrieve ip from network config 
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithZipFileStream("/", Assembly.GetExecutingAssembly().GetManifestResourceStream("StreamNode.wwwroot.WebClient.zip"), m => m
                    .WithContentCaching(true))
                //.WithZipFile("/", "F:/Progetti/StreamNode/StreamNode/build.zip")
                //.WithStaticFolder("/", "F:/Progetti/StreamNode/StreamNode/WebClient/build/", true, m => m
                //.WithContentCaching(true))
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));
            server.RunAsync();
        }

        public void Stop()
        {
            server.Dispose();
        }
    }
}
