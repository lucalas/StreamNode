using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    class RemoteControlVolume
    {
        public string name { get; set; }
        public int volume { get; set; } = -1;
        public bool output { get; set; }
        public bool mute { get; set; }
        public string icon { get; set; }
        public string device { get; set; }
    }
}
