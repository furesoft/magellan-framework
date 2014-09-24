using Magellan.Framework;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class WindowResultTests
    {
        [Test]
        public void ShouldAllowNullWindowName()
        {
            new WindowResult(null, null, new ViewEngineCollection());
        }

        [Test]
        public void ShouldSetViewTypeToWindow()
        {
            var result = new WindowResult("MyWindow", null, new ViewEngineCollection());
            Assert.AreEqual("Window", result.Options["ViewType"]);
        }
    }
}
