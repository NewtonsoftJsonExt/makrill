using Newtonsoft.Json;

namespace Makrill
{
    public class JsonConvert
    {
        /// <summary>
        /// Using array and dictionary converter
        /// </summary>
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