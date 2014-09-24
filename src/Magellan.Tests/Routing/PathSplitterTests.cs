using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class PathSplitterTests
    {
        [Test]
        public void SplitNullTest()
        {
            var path = ((string)null).SplitUrlPath();
            Assert.AreEqual(0, path.Length);
        }

        [Test]
        public void SplitEmptyTest()
        {
            var path = "".SplitUrlPath();
            Assert.AreEqual(0, path.Length);
        }

        [Test]
        public void SplitInconsistentSlashesTest()
        {
            var path = "//var///\\//\\//\\//\\usr///www\\\\\\bin\\debug".SplitUrlPath();
            Assert.AreEqual(5, path.Length);
            Assert.AreEqual("var", path[0]);
            Assert.AreEqual("usr", path[1]);
            Assert.AreEqual("www", path[2]);
            Assert.AreEqual("bin", path[3]);
            Assert.AreEqual("debug", path[4]);
        }
    }
}
