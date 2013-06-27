using System.Collections.Generic;
using NUnit.Framework;

namespace Makrill.Tests
{
    [TestFixture]
    public class JsonSerializerTests
    {
        private string expected =
            @"{
    ""level"":""ALERT"",
    ""message"":""Message"",
    ""properties"":{""prop"":""msg""}
}";

        [Test]
        public void Test()
        {
            var d = new Dictionary<string, object>();
            d["prop"] = "msg";
            d["dic"] = new Dictionary<string, object>
                           {
                               {"key1", "val1"},
                               {"key2", "val2"},
                               {"key3", new[] {"val3"}},
                               {"key4", new[] {1, 12345}},
                               {"key5", new[] {new Dictionary<string, object> {{"key6", "val6"}}}}
                           };
            d["array"] = new[] {"val4"};
            d["int"] = 1;
            d["double"] = 1.2;
            d["null"] = null;
            d["bool"] = true;
            var serializer = new WrappedNewtonsoft();
            var obj = serializer.Deserialize<Dictionary<string, object>>(serializer.Serialize(d));
            Assert.That(obj, Is.EqualTo(d));
        }

        [Test]
        public void WillSerializeOk()
        {
            string serialized = new WrappedNewtonsoft().Serialize(new
                                                                      {
                                                                          level = "ALERT",
                                                                          message = "Message",
                                                                          properties =
                                                                      new Dictionary<string, object> {{"prop", "msg"}}
                                                                      });
            Assert.That(serialized, Json.IsEqualTo(expected));
        }

        [Test]
        public void WillSerializeOk2()
        {
            string serialized = new WrappedNewtonsoft().Serialize(new
                                                                      {
                                                                          level = "ALERT",
                                                                          message = "Message",
                                                                          properties =
                                                                      new Dictionary<string, object> {{"prop", "msg"}}
                                                                      });
            Assert.That(serialized.Replace(" ", "").Replace("\r", "").Replace("\n", ""),
                        Is.EqualTo(expected.Replace(" ", "").Replace("\n", "").Replace("\n", "")));
        }
    }
}