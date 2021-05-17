using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Serilog;
using StreamNode.Services;

namespace StreamNode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LoggerManager.Init();
            Log.Logger.Information("StreamNode starting...");
        }

        void StopApp(object sender, ExitEventArgs e)
        {
        }
    }
}
