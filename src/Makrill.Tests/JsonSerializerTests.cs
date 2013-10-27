using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using WrappedNewtonsoft = Makrill.JsonConvert;
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

        [Test]
        public void RegressionTest()
        {
            var jsonConvert = new WrappedNewtonsoft();
            var result = jsonConvert.Deserialize<object[]>("[['1',['1','2',[1,3,null,{'1':1,'2':[10,20,30]}]]],[['3'],['4'],[5]]]");

            var r = result.Select(o => jsonConvert.Deserialize((JArray)o)).ToArray();
            var condition1 = r.Select(r1 => r1.Where(c => !c.GetType().IsArray && !(c is string))).Where(r1 => r1.Any()).SelectMany(r1 => r1);
            Assert.That(condition1, Is.EquivalentTo(new object[0]));
        }
    }
}