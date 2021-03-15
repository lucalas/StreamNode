using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNodeEngine.Objects
{
    public class AudioServiceUpdate
    {
        public string hashUpdate { get; set; }
        public int tsUpdate { get; set; }

        public RemoteControlVolumes volumes { get; set; }
    }
}
