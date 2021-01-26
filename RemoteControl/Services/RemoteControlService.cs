using NAudio.CoreAudioApi;
using RemoteControl.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace RemoteControl.Services
{
    class RemoteControlService
    {
        private IRemoteControlEngine engine;
        public event EventHandler<RemoteControlOnMessageArgs> OnMessage;
        private AudioService ac;
        private OBSService os;

        public RemoteControlService(AudioService ac, OBSService os) {
            this.ac = ac;
            this.os = os;
            engine = new FleckEngine();
        }

        public void connect()
        {
            engine.OnMessage += MessageHandler;
            engine.Connect();
        }

        public string MessageHandler(object sender, RemoteControlOnMessageArgs message)
        {

            RemoteControlData data = JsonSerializer.Deserialize<RemoteControlData>(message.message);

            IData dataResponse = null;

            // TODO create method for RemoteControlDataType logics
            if (RemoteControlDataType.Volumes.Equals(data.type))
            {
                RemoteControlVolumes volumes = new RemoteControlVolumes();

                foreach (MMDevice dev in ac.GetListOfOutputDevices())
                {
                    foreach (ApplicationController appOut in ac.GetApplicationsMixer(dev))
                    {
                        ApplicationController appDev = ac.GetDeviceController(dev);
                        RemoteControlVolume audio = new RemoteControlVolume();
                        audio.name = appOut.processName;
                        audio.volume = (int)(appOut.getVolume() * 100);
                        audio.mute = appOut.getMute();
                        audio.device = appDev.device.FriendlyName;
                        audio.output = true;

                        // TODO add icon
                        //volume.icon = ...
                        volumes.Add(audio);
                    }
                }

                foreach (MMDevice dev in ac.GetListOfInputDevices())
                {
                    // FIXME retrieve devices instead of application mixer because we can't control microphone of an application
                    //foreach (ApplicationController appIn in ac.GetApplicationsMixer(dev))
                    //{
                        RemoteControlVolume audio = new RemoteControlVolume();
                        
                        audio.name = dev.FriendlyName;
                        audio.mute = dev.AudioEndpointVolume.Mute;
                        audio.volume = (int)(dev.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                        audio.device = dev.FriendlyName;
                        audio.output = false;
                        volumes.Add(audio);
                    //}
                }

                // TODO add input devices
                dataResponse = volumes;
            } else if (RemoteControlDataType.ChangeVolume.Equals(data.type)) {
                ChangeVolumeType ChangeVolume = JsonSerializer.Deserialize<ChangeVolumeType>(data.data.ToString());
                if (ChangeVolume.output)
                {
                    ApplicationController app = ac.GetApplicationOutput(ChangeVolume.name, ChangeVolume.device);
                    app.updateVolume((float)ChangeVolume.volume / 100);
                } else
                {
                    // TODO change mic volume
                    MMDevice mic = ac.GetDeviceInput(ChangeVolume.device);
                    mic.AudioEndpointVolume.MasterVolumeLevelScalar = (float) ChangeVolume.volume / 100;
                }
                Trace.WriteLine(ChangeVolume.name + ": " + ChangeVolume.volume);
            } else
            {
                data.status = "Command not found";
            }

            data.data = dataResponse;
            return JsonSerializer.Serialize(data);
        }
    }
}
