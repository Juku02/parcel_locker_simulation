using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ParcelBox.Configurations
{
    public class Configuration
    {
        public List<T> ReadFromJSON<T>(string path)
        {
            string text = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<T>>(text);
        }

        public int WriteToJSON<T>(List<T> list)
        {
            var jsonList = JsonConvert.SerializeObject(list);
            File.WriteAllText(@"./log.json", jsonList);
            return 0;
        }
    }
}