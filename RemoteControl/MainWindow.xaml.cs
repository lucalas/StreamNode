using NAudio.CoreAudioApi;
using OBSWebsocketDotNet.Types;
using RemoteControl.Controllers;
using StreamdeckEngine.Objects;
using System;
using System.Collections.Generic;
using System.Windows;
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

        StreamdeckEngine.StreamdeckSocketManager engine = new StreamdeckEngine.StreamdeckSocketManager();

        public MainWindow()
        {
            _singleton = this;
            InitializeComponent();
            // getOutputMixers();
            // getInputMixers();
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
                OBSScenes.Items.Add(sc);
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            engine.obsService.Connect(ipport.Text, pwd.Text);
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
