using RemoteControl.Objects;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Logica di interazione per ApplicationMixer.xaml
    /// </summary>
    public partial class ApplicationMixer : UserControl
    {

        ApplicationMixerData AppData;

        public ApplicationMixer(): this("Mixer", null)
        {
        }

        public ApplicationMixer(string _title, ApplicationController _ac)
        {
            InitializeComponent();
            AppData = new ApplicationMixerData() { title = _title, ac = _ac };
            Title.DataContext = AppData;

        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider) sender;
            AppData.ac.session.SimpleAudioVolume.Volume = (float) slider.Value / (float) slider.Maximum;
        }
    }

    public class ApplicationMixerData
    {
        public string title { get; set; }
        public ApplicationController ac { get; set; }
    }
}
