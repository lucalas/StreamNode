using RemoteControl.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Services
{
    interface IRemoteControlEngine
    {
        delegate string OnMessageEventHandler(object sender, RemoteControlOnMessageArgs args);
        event OnMessageEventHandler OnMessage;

        void Connect();
    }
}
