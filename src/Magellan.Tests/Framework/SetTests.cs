using System;
using System.Linq;
using System.Threading;
using Magellan.ComponentModel;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class SetTests
    {
        [Test]
        public void ShouldThrowIfSameThreadModifiesCollectionWhileEnumerating()
        {
            var collection = new Set<string>();
            collection.Add("1");
            collection.Add("2");
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            collection.Add("3");
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());

            enumerator.Dispose();
        }

        [Test]
        public void ShouldAdd()
        {
            var collection = new Set<string>();
            collection.Add("1");
            collection.Add("2");
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual("1", collection.First());
            Assert.AreEqual("2", collection.Last());
        }

        [Test]
        public void ShouldNotAddNulls()
        {
            var collection = new Set<string>();
            collection.Add(null);
            collection.Add("1");
            collection.Add(null);
            collection.Add(null);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual("1", collection.First());
        }

        [Test]
        public void ShouldContains()
        {
            var collection = new Set<string>();
            collection.Add("1");
            collection.Add("2");
            collection.Add(null);  // Ignored
            Assert.IsTrue(collection.Contains("1"));
            Assert.IsFalse(collection.Contains("Hello"));
            Assert.IsFalse(collection.Contains(null));
        }

        [Test]
        public void ShouldClear()
        {
            var collection = new Set<string>();
            collection.Add("1");
            collection.Add("2");
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void ShouldRemove()
        {
            var collection = new Set<string>();
            collection.Add("1");
            collection.Add("2");
            collection.Remove("1");
            collection.Remove(null);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual("2", collection.First());
        }

        [Test]
        public void ShouldNotAddDuplicates()
        {
            var collection = new Set<string>();
            collection.Add("1");
            collection.Add("2");
            collection.Add("2");
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual("1", collection.First());
            Assert.AreEqual("2", collection.Last());
        }

        [Test]
        public void ShouldAddRange()
        {
            var collection = new Set<string>();
            collection.AddRange(new[] { "Hello", "Goodbye" });
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual("Hello", collection.OfType<string>().First());
            Assert.AreEqual("Goodbye", collection.OfType<string>().Last());
        }

        [Test]
        public void ShouldLockThreadsFromEditingCollectionWhenEnumerating()
        {
            var collection = new Set<string>();
            collection.Add("1");
            collection.Add("2");
            var enumerator = collection.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    collection.Add("3");
                });

            Assert.IsTrue(enumerator.MoveNext());
            Thread.Sleep(100); // Give the thread about enough time to become locked
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(2, collection.Count);

            enumerator.Dispose();
            Thread.Sleep(100); // Give the thread about enough time to become unlocked
            Assert.AreEqual(3, collection.Count);
        }
    }
}
