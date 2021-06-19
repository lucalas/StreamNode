namespace StreamNodeEngine.Objects
{
    public class AudioServiceUpdate
    {
        public string hashUpdate { get; set; }
        public int tsUpdate { get; set; }

        public RemoteControlVolumes volumes { get; set; }
    }
}
