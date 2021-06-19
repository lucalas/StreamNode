using Newtonsoft.Json;

namespace StreamNodeEngine.Objects
{
    public class VolumeStoreData
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("order")]
        public int order { get; set; }
        [JsonProperty("hidden")]
        public bool hidden { get; set; }
    }
}
