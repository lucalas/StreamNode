using Newtonsoft.Json;
using System;
using System.IO;

namespace StreamNodeEngine.Engine.Services
{
    public class StoreService
    {
        private static string tempDir = "./";
        private static string storeFile = "data.json";

        public void store(object data)
        {
            try
            {
                File.WriteAllText(tempDir + storeFile, JsonConvert.SerializeObject(data));
                LogRedirector.debug($"Stored [{tempDir + storeFile}] file");
            }
            catch (Exception ex)
            {
                LogRedirector.error($"{ex}");
            }
        }

        public T read<T>()
        {
            try
            {
                if (File.Exists(tempDir + storeFile))
                {
                    return JsonConvert.DeserializeObject<T>(File.ReadAllText(tempDir + storeFile));
                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                LogRedirector.error($"{ex}");
                return default;
            }
        }
    }
}
