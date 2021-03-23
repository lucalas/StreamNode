using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNodeEngine.Objects
{
    public class VolumeOrderData
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("order")]
        public int order { get; set; }
    }
}
