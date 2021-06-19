namespace StreamNodeEngine.Objects
{
    class ChangeVolumeType
    {

        public string name { get; set; }

        public string device { get; set; }
        public int volume { get; set; }
        public bool output { get; set; }
        public bool mute { get; set; }
    }
}
