using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class PathIteratorTests
    {
        [Test]
        public void ConsumeTest()
        {
            var eater = new PathIterator("foo/bar/baz");
            Assert.AreEqual("foo", eater.Next());
            Assert.IsFalse(eater.IsAtEnd);
            Assert.AreEqual("bar", eater.Next());
            Assert.AreEqual("baz", eater.Next());
            Assert.IsTrue(eater.IsAtEnd);
        }

        [Test]
        public void ConsumeEmptyTest()
        {
            var eater = new PathIterator("");
            Assert.AreEqual("", eater.Next());
            Assert.IsTrue(eater.IsAtEnd);
        }

        [Test]
        public void ConsumeDoubleSeperatorTest()
        {
            var eater = new PathIterator("/foo//bar////baz");
            Assert.AreEqual("foo", eater.Next());
            Assert.IsFalse(eater.IsAtEnd);
            Assert.AreEqual("bar", eater.Next());
            Assert.AreEqual("baz", eater.Next());
            Assert.IsTrue(eater.IsAtEnd);
        }

        [Test]
        public void ReadAllConsumesPath()
        {
            var eater = new PathIterator("/foo//bar////baz");
            Assert.AreEqual("foo/bar/baz", eater.ReadAll());
            Assert.IsTrue(eater.IsAtEnd);
        }
    }
}
