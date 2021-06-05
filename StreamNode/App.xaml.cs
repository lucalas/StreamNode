using System.Windows;
using StreamNode.Services;
using Serilog;
using StreamNode.Services.Settings;

namespace StreamNode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SettingsService settingsService { get; } = new SettingsService();
        public static StreamNodeEngine.StreamNodeSocketManager engine { get; } = new StreamNodeEngine.StreamNodeSocketManager();
        public static HttpServerService httpServer { get; } = new HttpServerService();
        public App()
        {
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
