using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    class RemoteControlData
    {
        public string type { get; set; }
        public object data { get; set; }

        public string status { get; set; } = "ok";
    }
}
