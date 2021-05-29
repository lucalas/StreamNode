using Config.Net;
using System.IO;
using Newtonsoft.Json;

namespace StreamNode.Services.Settings
{
    public class SettingsService
    {
        private const string _HTTP_SERVER_IP = "0.0.0.0";
        private const int _HTTP_SERVER_PORT = 0;
        private const int _WEBSOCKET_PORT = 0;
        // TODO add the other default settings

        private string PATH = "config.json";
        public ISettings settings { get; set; }

        public SettingsService()
        {
            Init();
            GetSettings();
        }

        private void Init()
        {
            if(!File.Exists(this.PATH))
            {
                this.settings = new Settings(_WEBSOCKET_PORT, _HTTP_SERVER_IP, _HTTP_SERVER_PORT);
                SaveSettings();
            }
        }
        

        private void GetSettings()
        {
            this.settings = new ConfigurationBuilder<ISettings>().UseJsonFile(this.PATH).Build();
        }

        public void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(this.settings);

            using (StreamWriter sw = new StreamWriter(this.PATH))
            {
                sw.WriteLine(json);
            }

            this.GetSettings();
        }
    }
}
