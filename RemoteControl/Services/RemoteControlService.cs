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

        public RemoteControlService() {
            engine = new FleckEngine();
        }

        public void connect()
        {
            engine.OnMessage += MessageHandler;
            engine.Connect();
        }

        public void MessageHandler(object sender, RemoteControlOnMessageArgs message)
        {

            RemoteControlData data = JsonSerializer.Deserialize<RemoteControlData>(message.message);

            if (RemoteControlDataType.Volumes.Equals(data.type))
            {
                // FIXME comprendere come castare il tipo data
                //RemoteControlVolumes volumes = data.data;
            }
        }
    }
}
