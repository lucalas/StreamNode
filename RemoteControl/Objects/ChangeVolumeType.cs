using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    class ChangeVolumeType
    {

        public string name { get; set; }

        public string device { get; set; }
        public int volume { get; set; }
        public bool output { get; set; }
    }
}
