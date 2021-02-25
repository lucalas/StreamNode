using NAudio.CoreAudioApi;

namespace StreamdeckEngine.Objects
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

        public float getVolume()
        {
            return session.SimpleAudioVolume.Volume;
        }

        public void updateVolume(float value)
        {
            session.SimpleAudioVolume.Volume = value;
        }

        public void toggleMute()
        {
            session.SimpleAudioVolume.Mute = !session.SimpleAudioVolume.Mute;
        }

        public void setMute(bool mute)
        {
            session.SimpleAudioVolume.Mute = mute;
        }

        public bool getMute()
        {
            return session.SimpleAudioVolume.Mute;
        }
    }
}
