using Config.Net;
using System.IO;
using Newtonsoft.Json;

namespace StreamNode.Services.Settings
{
    public class SettingsService
    {

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
                this.settings = new Settings();
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
