using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    class RemoteControlOnMessageArgs : EventArgs
    {
        public string message { get; set; }
    }
}
