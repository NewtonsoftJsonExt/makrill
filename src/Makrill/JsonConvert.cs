using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Makrill
{
    public class JsonConvert
    {
        public T Deserialize<T>(string val)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ArrayAndDictionaryConverter());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(val, settings);
        }
        public string Serialize<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}