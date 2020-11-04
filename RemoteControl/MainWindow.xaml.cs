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
        public MainWindow()
        {
            InitializeComponent();
            AudioService ac = new AudioService();
            List<ApplicationController> appController = ac.GetApplicationsMixer(ac.GetDefaultOutputDevice());
            foreach (ApplicationController app in appController)
            {
                ApplicationMixer mixer = new ApplicationMixer(app.processName, app);
                Mixer.Items.Add(mixer);
            }
        }
    }
}
