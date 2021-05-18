using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using StreamNode.Services;
using Serilog;

namespace StreamNode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static StreamNodeEngine.StreamNodeSocketManager engine {get;} = new StreamNodeEngine.StreamNodeSocketManager();
        public static HttpServerService httpServer {get;} = new HttpServerService();
        public App() {
            LoggerManager.Init();
            Log.Information(Logo.LOGO_ASCII);
            Log.Information("StreamNode started");
        }

        void StopApp(object sender, ExitEventArgs e)
        {
            Log.Information("StreamNode Stopped");
        }
    }
}
