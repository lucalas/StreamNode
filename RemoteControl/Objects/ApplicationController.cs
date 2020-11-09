using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Objects
{
    public class ApplicationController
    {
        public SimpleAudioVolume session { get; set; }
        public MMDevice device { get; set; }

        public string processName { get; set; }

        public ApplicationController(MMDevice _device, SimpleAudioVolume _session, string _processName)
        {
            session = _session;
            device = _device;
            processName = _processName;
        }

        public float getVolume()
        {
            return session.Volume;
        }

        public void updateVolume(float value)
        {
            session.Volume = value;
        }

        public void toggleMute()
        {
            session.Mute = !session.Mute;
        }

        public void setMute(bool mute)
        {
            session.Mute = mute;
        }

        public bool getMute()
        {
            return session.Mute;
        }
    }
}
