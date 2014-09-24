using System.Linq;
using Magellan.Utilities;
using NUnit.Framework;

namespace Magellan.Tests.Utilities
{
    [TestFixture]
    public class PropertyValueListerTests
    {
        [Test]
        public void GetPropertiesWithNullTarget()
        {
            var result = PropertyValueLister.GetProperties(null);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.GetEnumerator().MoveNext());
        }

        [Test]
        public void GetPropertiesOnAnoymousObject()
        {
            var result = PropertyValueLister.GetProperties(new {name = "123", jack = 100}).ToList();
            Assert.IsNotNull(result);
            Assert.IsTrue(result[0].Key == "name" && (string)result[0].Value == "123");
            Assert.IsTrue(result[1].Key == "jack" && (int)result[1].Value == 100);
        }
    }
}