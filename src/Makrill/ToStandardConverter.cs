using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Makrill
{
    public class ToStandardConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            var dic = new Dictionary<string, object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        object name = reader.Value;
                        dic.Add(name.ToString(), ExpectDictionaryOrArrayOrPrimitive(reader));
                        break;
                    case JsonToken.EndObject:
                        return dic;
                    default:
                        throw new Exception(reader.TokenType.ToString());
                }
            }
            return dic;
        }

        private static object ExpectDictionaryOrArrayOrPrimitive(JsonReader reader)
        {
            var dic = new Dictionary<string, object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                    case JsonToken.Integer:
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Null:
                        return reader.Value;

                    case JsonToken.StartObject:
                        return ExpectDictionaryOrArrayOrPrimitive(reader);
                    case JsonToken.PropertyName:
                        dic.Add(reader.Value.ToString(), ExpectDictionaryOrArrayOrPrimitive(reader));
                        break;
                    case JsonToken.EndObject:
                        return dic;
                    case JsonToken.StartArray:
                        return ExpectArray(reader);
                    default:
                        throw new Exception("Unrecognized token: " + reader.TokenType.ToString());
                }
            }
            throw new Exception("Missing end");
        }

        private static object ExpectArray(JsonReader reader)
        {
            var array = new List<Object>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                    case JsonToken.Integer:
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Null:
                        array.Add(reader.Value);
                        break;
                    case JsonToken.StartObject:
                        array.Add(ExpectDictionaryOrArrayOrPrimitive(reader));
                        break;
                    case JsonToken.EndArray:
                        return array.ToArray();
                    default:
                        throw new Exception("Array: Unrecognized token: " + reader.TokenType.ToString());
                }
            }
            throw new Exception("Missing end");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Dictionary<string, object>);
        }
    }
}