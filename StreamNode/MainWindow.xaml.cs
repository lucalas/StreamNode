
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Files;
using OBSWebsocketDotNet.Types;
using StreamNode.Controllers;
using StreamNode.Services;
using System;
using System.Windows;
using static StreamNode.Controllers.SceneController;


namespace StreamNode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _singleton;
        public static MainWindow singleton { get { return _singleton; } }

        StreamNodeEngine.StreamNodeSocketManager engine = new StreamNodeEngine.StreamNodeSocketManager();

        HttpServerService server;
        public MainWindow()
        {
            _singleton = this;
            InitializeComponent();
            StartServer();
            // getOutputMixers();
            // getInputMixers();
        }

        private void StartServer()
        {
            server = new HttpServerService(8000);
            server.Start();
        }

        private static WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                // First, we will configure our web server by adding Modules.
                .WithLocalSessionManager()
                .WithStaticFolder("/", "F:/Progetti/StreamNode/StreamNode/WebClient/build/", true, m => m
                    .WithContentCaching(true))
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" }))); ;

            // Listen for state changes.
            //server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}";

            return server;
        }

        /*public void getOutputMixers()
        {
            foreach (MMDevice device in engine.audioService.GetListOfOutputDevices())
            {
                List<ApplicationController> appController = engine.audioService.GetApplicationsMixer(device);
                foreach (ApplicationController app in appController)
                {
                    ApplicationMixer mixer = new ApplicationMixer(app.processName, device.FriendlyName, app);
                    MixerOutput.Items.Add(mixer);
                }
            }
        }*/

        /*public void getInputMixers()
        {
            foreach (MMDevice device in engine.audioService.GetListOfInputDevices())
            {
                ApplicationController deviceController = engine.audioService.GetDeviceController(device);
                ApplicationMixer mixerDevice = new ApplicationMixer(deviceController.processName, device.FriendlyName, deviceController);
                MixerInput.Items.Add(mixerDevice);
                List<ApplicationController> appController = engine.audioService.GetApplicationsMixer(device);
                foreach (ApplicationController app in appController)
                {
                    ApplicationMixer mixer = new ApplicationMixer(app.processName, device.FriendlyName, app);
                    MixerInput.Items.Add(mixer);
                }
            }
        }*/

        public void getOBSScenes()
        {
            GetSceneListInfo scenes = engine.obsService.getScenes();

            foreach (OBSScene scene in scenes.Scenes)
            {
                SceneController sc = new SceneController(scene);
                sc.onButtonClick += OnButtonClickEventHandler;
            }
        }

        static void e_OBSConnected(object sender, EventArgs e)
        {
            MainWindow.singleton.getOBSScenes();
        }

        void OnButtonClickEventHandler(object sender, OnButtonClickEventArgs e)
        {
            engine.obsService.setCurrentScene(e.sceneName);
        }
    }
}
