using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNodeEngine.Engine.Services
{
    public class StoreService
    {
        private static string tempDir = "./";
        private static string storeFile = "data.json";

        public void store(object data) => System.IO.File.WriteAllText(tempDir + storeFile, JsonConvert.SerializeObject(data));

        public T read<T>()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(tempDir + storeFile));
            }catch (Exception ex)
            {
                // TODO report exception
                return default;
            }
        }
    }
}
