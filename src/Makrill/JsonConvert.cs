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
            settings.Converters.Add(new ToStandardConverter());
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(val, settings);
        }
        public object[] Deserialize(JArray array)
        {
            return ToStandardConverter.ExpectArray(array);
        }
        public object Deserialize(JObject obj)
        {
            return ToStandardConverter.ExpectObject(obj);
        }
        public object Deserialize(JToken obj)
        {
            return ToStandardConverter.ExpectValue(obj);
        }
        public string Serialize<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}