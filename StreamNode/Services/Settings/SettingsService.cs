using Config.Net;
using System.IO;
using Newtonsoft.Json;

namespace StreamNode.Services.Settings
{
    public class SettingsService
    {

        private string PATH = "config.json";
        public ISettings settings { get {return _settings;} }
        private ISettings _settings { get; set; }

        public SettingsService()
        {
            InitSettings();
        }
        

        private void InitSettings()
        {
            this._settings = new ConfigurationBuilder<ISettings>().UseJsonFile(this.PATH).Build();
        }

        public void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(this.settings, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(this.PATH))
            {
                sw.WriteLine(json);
            }

            this.InitSettings();
        }
    }
}
