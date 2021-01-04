using NAudio.CoreAudioApi;
using OBSWebsocketDotNet.Types;
using RemoteControl.Controllers;
using RemoteControl.Objects;
using RemoteControl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static RemoteControl.Controllers.SceneController;

namespace RemoteControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _singleton;
        public static MainWindow singleton { get { return _singleton; } }

        AudioService ac = new AudioService();
        OBSService os = new OBSService();
        RemoteControlService rcs;

        public MainWindow()
        {
            _singleton = this;
            InitializeComponent();
            getOutputMixers();
            getInputMixers();
            rcs = new RemoteControlService(ac, os);
            InitOBS();
            rcs.connect();
        }

        public void InitOBS()
        {
            os.Connected += e_OBSConnected;
        }

        public void getOutputMixers()
        {
            foreach (MMDevice device in ac.GetListOfOutputDevices())
            {
                List<ApplicationController> appController = ac.GetApplicationsMixer(device);
                foreach (ApplicationController app in appController)
                {
                    ApplicationMixer mixer = new ApplicationMixer(app.processName, device.FriendlyName, app);
                    MixerOutput.Items.Add(mixer);
                }
            }
        }

        public void getInputMixers()
        {
            foreach (MMDevice device in ac.GetListOfInputDevices())
            {
                ApplicationController deviceController = ac.GetDeviceController(device);
                ApplicationMixer mixerDevice = new ApplicationMixer(deviceController.processName, device.FriendlyName, deviceController);
                MixerInput.Items.Add(mixerDevice);
                List<ApplicationController> appController = ac.GetApplicationsMixer(device);
                foreach (ApplicationController app in appController)
                {
                    ApplicationMixer mixer = new ApplicationMixer(app.processName, device.FriendlyName, app);
                    MixerInput.Items.Add(mixer);
                }
            }
        }

        public void getOBSScenes()
        {
            GetSceneListInfo scenes = os.getScenes();

            foreach (OBSScene scene in scenes.Scenes)
            {
                SceneController sc = new SceneController(scene);
                sc.onButtonClick += OnButtonClickEventHandler;
                OBSScenes.Items.Add(sc);
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            os.Connect(ipport.Text, pwd.Text);
        }

        static void e_OBSConnected(object sender, EventArgs e)
        {
            MainWindow.singleton.getOBSScenes();
        }

        void OnButtonClickEventHandler(object sender, OnButtonClickEventArgs e)
        {
            os.setCurrentScene(e.sceneName);
        }
    }
}
