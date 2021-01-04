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
                        RemoteControlVolume volume = new RemoteControlVolume();
                        volume.name = app.processName;
                        volume.volume = (int)(app.session.Volume * 100);
                        volume.mute = app.session.Mute;
                        volumes.Add(volume);
                    }
                }
                dataResponse = volumes;
            }

            data.data = dataResponse;
            return JsonSerializer.Serialize(data);
        }
    }
}
