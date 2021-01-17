using NAudio.CoreAudioApi;
using RemoteControl.Objects;
using System;
using System.Collections.Generic;
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

            if (RemoteControlDataType.Volumes.Equals(data.type))
            {
                // FIXME comprendere come castare il tipo data
                RemoteControlVolumes volumes = new RemoteControlVolumes();
                MMDeviceCollection iDev = ac.GetListOfInputDevices();

                foreach (MMDevice dev in ac.GetListOfOutputDevices())
                {
                    foreach (ApplicationController app in ac.GetApplicationsMixer(dev))
                    {
                        ApplicationController appC = ac.GetDeviceController(dev);
                        RemoteControlVolume audio = new RemoteControlVolume();
                        audio.name = app.processName;
                        audio.volume = (int)(app.getVolume() * 100);
                        audio.mute = app.getMute();
                        audio.device = appC.device.FriendlyName;
                        // TODO add icon
                        //volume.icon = ...
                        volumes.Add(audio);
                    }
                }

                // TODO add input devices
                dataResponse = volumes;
            } else if (RemoteControlDataType.ChangeVolume.Equals(data.type)) {
                int i = 0;
            } else
            {
                data.status = "Command not found";
            }

            data.data = dataResponse;
            return JsonSerializer.Serialize(data);
        }
    }
}
