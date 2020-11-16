using NAudio.CoreAudioApi;
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

namespace RemoteControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AudioService ac = new AudioService();
        OBSService os = new OBSService();
        public MainWindow()
        {
            InitializeComponent();
            getOutputMixers();
            getInputMixers();
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

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            os.Connect(ipport.Text, pwd.Text);
        }
    }
}
