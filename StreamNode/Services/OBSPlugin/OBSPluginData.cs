using System;

namespace StreamNode.Services.OBSPlugin
{
    public class OBSPluginData
    {
        public string name { get; set; }
        public string tagName { get; set; }
        public DateTime publishedAt { get; set; }
        public OBSPluginAssetsData[] assets { get; set; }
    }

    public class OBSPluginAssetsData
    {
        public string url { get; set; }
        public string name { get; set; }
        public string browserDownloadUrl { get; set; }
    }
}