using System;
using System.Text.RegularExpressions;
using CSC578_Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class JsonParserTests
    {
        /// <summary>
        /// Try to deserialize an incomplete JSON string
        /// </summary>
        [TestMethod]
        public void TestDeserializeBadJSON()
        {
            var json = "{'bad': 3, 'json':true";
            var gameObjects = JsonParser.Deserialize(json, "test");
            Assert.AreEqual(gameObjects.Count, 0);
        }

        [TestMethod]
        public void TestDeserializeMetaBadJSON()
        {
            var json = "{'bad': 3, 'json':true, 'author': 'Group 8'";
            var meta = JsonParser.DeserializeMeta(json);
            Assert.AreEqual(meta, null);
        }
    }
}
