using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    public class ApplicationController
    {
        public AudioSessionControl session { get; set; }
        public MMDevice device { get; set; }

        public string processName { get; set; }

        public ApplicationController(MMDevice _device, AudioSessionControl _session, string _processName)
        {
            session = _session;
            device = _device;
            processName = _processName;
        }
    }
}
