
namespace StreamdeckEngine.Objects
{
    class RemoteControlData
    {
        public string type { get; set; }
        public object data { get; set; }

        public string status { get; set; } = "ok";
    }
}
