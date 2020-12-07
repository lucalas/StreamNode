using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Services
{
    class RemoteControlService
    {
        private IRemoteControlEngine engine;

        public RemoteControlService() {
            engine = new FleckEngine();
        }

        public void connect()
        {
            engine.Connect();
        }
    }
}
