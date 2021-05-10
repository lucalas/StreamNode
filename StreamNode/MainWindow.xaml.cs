
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Files;
using OBSWebsocketDotNet.Types;
using StreamNode.Controllers;
using StreamNode.Services;
using System;
using System.Windows;
using System.Diagnostics;
using static StreamNode.Controllers.SceneController;


namespace StreamNode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowContext context = new MainWindowContext();
        private static MainWindow _singleton;
        public static MainWindow singleton { get { return _singleton; } }

        private StreamNodeEngine.StreamNodeSocketManager engine = new StreamNodeEngine.StreamNodeSocketManager();
        private HttpServerService httpServer = new HttpServerService();

        public MainWindow()
        {
            _singleton = this;
            InitializeComponent();
            this.DataContext = context;
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            engine.ConfigOBSWebSocket(context.IpValue, context.PortValue, context.PwdValue);
        }
        private void StartServer(object sender, RoutedEventArgs e)
        {
            httpServer.Start();
            engine.Connect();
        }

        private void StopServer(object sender, RoutedEventArgs e)
        {
            engine.Disconnect();
        }
    }
}
