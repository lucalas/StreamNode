using StreamdeckEngine.Objects;
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

        public ApplicationMixer(): this("Mixer", "DeviceLabel", null)
        {
        }

        public ApplicationMixer(string _title, string device, ApplicationController _ac)
        {
            if (_title.Length > 20)
            {
                _title = _title.Substring(0, 17) + "...";
            }
            InitializeComponent();
            AppData = new ApplicationMixerData() { title = _title, DeviceName = device, ac = _ac };
            Title.DataContext = AppData;
            DeviceNameLabel.DataContext = AppData;
            updateSliderVolume();
        }

        private void updateSliderVolume()
        {
            VolumeSlider.Value = AppData.ac.getVolume() * (float)VolumeSlider.Maximum;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider) sender;
            AppData.ac.updateVolume((float) slider.Value / (float) slider.Maximum);
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (AppData.ac.getMute())
            {
                button.Content = "Mute";
            } else
            {
                button.Content = "Unmute";
            }

            AppData.ac.toggleMute();
        }
    }

    public class ApplicationMixerData
    {
        public string title { get; set; }
        public string DeviceName { get; set; }

        public ApplicationController ac { get; set; }
    }
}
