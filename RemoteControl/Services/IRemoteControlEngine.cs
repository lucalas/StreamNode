using RemoteControl.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Services
{
    interface IRemoteControlEngine
    {
        event EventHandler<RemoteControlOnMessageArgs> OnMessage;

        void Connect();
    }
}
