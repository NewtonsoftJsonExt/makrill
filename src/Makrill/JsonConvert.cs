using Newtonsoft.Json;

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

        public string Serialize<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}