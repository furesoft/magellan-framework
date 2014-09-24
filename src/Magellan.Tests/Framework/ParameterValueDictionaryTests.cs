using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class RouteValueDictionaryTests
    {
        [Test]
        public void WhenConstructedWithDictionaryShouldCopyItems()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Key1", "Value1");
            dictionary.Add("Key2", "Value2");

            var parameters = new RouteValueDictionary(dictionary);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("Value1", parameters["Key1"]);
            Assert.AreEqual("Value2", parameters["Key2"]);
        }

        [Test]
        public void WhenConstructedShouldBeEmpty()
        {
            var parameters = new RouteValueDictionary();
            Assert.AreEqual(0, parameters.Count);
        }

        [Test]
        public void WhenConstructedWithDictionaryShouldHandleNullDictionary()
        {
            var parameters = new RouteValueDictionary((IDictionary)null);
            Assert.AreEqual(0, parameters.Count);
        }

        [Test]
        public void WhenConstructedWithObjectShouldHandleNull()
        {
            var parameters = new RouteValueDictionary((object)null);
            Assert.AreEqual(0, parameters.Count);
        }

        [Test]
        public void WhenConstructedWithObjectShouldUsePropertiesAsKeys()
        {
            var parameters = new RouteValueDictionary(new { foo = "bar", abc = "123"});
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("bar", parameters["foo"]);
            Assert.AreEqual("123", parameters["abc"]);
        }

        [Test]
        public void WhenEnumeratedShouldYieldKeyValuePairs()
        {
            var parameters = new RouteValueDictionary(new { foo = "bar", abc = "123" });
            var items = parameters.Select(x => x).ToArray();
            Assert.AreEqual(2, items.Count());
            Assert.AreEqual("foo", items[0].Key);
            Assert.AreEqual("bar", items[0].Value);
            Assert.AreEqual("abc", items[1].Key);
            Assert.AreEqual("123", items[1].Value);
        }

        [Test]
        public void CannotAddRangeSelf()
        {
            var parameters = new RouteValueDictionary();
            Assert.Throws<InvalidOperationException>(() => parameters.AddRange(parameters));
        }

        [Test]
        public void WhenItemsAreRemovedShouldRemoveItem()
        {
            var parameters = new RouteValueDictionary(new { foo = "bar", abc = "123" });
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("bar", parameters["foo"]);
            Assert.AreEqual("123", parameters["abc"]);

            parameters.Remove("foo");
            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("123", parameters["abc"]);
        }

        [Test]
        public void ShouldOverrideAddItemsWhenToldTo()
        {
            var original = new RouteValueDictionary(new { foo = "1", abc = "3" });
            var replacement = new RouteValueDictionary(new { foo = "2", abc = "4", def = "6" });
            original.AddRange(replacement, true);
            Assert.AreEqual("2", original["foo"]);
            Assert.AreEqual("4", original["abc"]);
            Assert.AreEqual("6", original["def"]);
        }

        [Test]
        public void ShouldNotOverrideAddItemsWhenToldNotTo()
        {
            var original = new RouteValueDictionary(new { foo = "1", abc = "3" });
            var replacement = new RouteValueDictionary(new { foo = "2", abc = "4", def = "6" });
            original.AddRange(replacement, false);
            Assert.AreEqual("1", original["foo"]);
            Assert.AreEqual("3", original["abc"]);
            Assert.AreEqual("6", original["def"]);
        }

        [Test]
        public void ShouldIgnoreNullAddRange()
        {
            var original = new RouteValueDictionary(new { foo = "1", abc = "3" });
            original.AddRange(null);
        }

        [Test]
        public void CannotBeCreatedFromString()
        {
            Assert.Throws<InvalidOperationException>(() => new RouteValueDictionary("Hello"));
        }

        [Test]
        public void CannotBeCreatedFromValueType()
        {
            Assert.Throws<InvalidOperationException>(() => new RouteValueDictionary(15));
        }
    }
}
