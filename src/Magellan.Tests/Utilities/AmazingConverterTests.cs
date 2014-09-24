using System;
using System.Xml.Linq;
using Magellan.Utilities;
using NUnit.Framework;

namespace Magellan.Tests.Utilities
{
    [TestFixture]
    public class AmazingConverterTests
    {
        [Test]
        public void CanConvert()
        {
            Assert.AreEqual(null, AmazingConverter.Convert(null, typeof(string)), "reference type");
            Assert.AreEqual(0, AmazingConverter.Convert(null, typeof(int)), "value type");
            Assert.AreEqual(0.0f, AmazingConverter.Convert(0, typeof(float)), "int to float");
            Assert.AreEqual(0, AmazingConverter.Convert(0L, typeof(int)), "long to int");
            Assert.AreEqual("0", AmazingConverter.Convert(0, typeof(string)), "int to string");
            Assert.AreEqual(DateTime.MinValue, AmazingConverter.Convert(null, typeof(DateTime)), "date");
            Assert.AreEqual(35, AmazingConverter.Convert("35", typeof(int)), "string to int");
            Assert.AreEqual((XName)"button", AmazingConverter.Convert("button", typeof(XName)), "op_Implicit");
        }
    }
}
