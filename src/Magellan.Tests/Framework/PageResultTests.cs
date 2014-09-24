using Magellan.Framework;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class PageResultTests
    {
        [Test]
        public void ShouldAllowNullPageName()
        {
            new PageResult(null, false, new ViewEngineCollection());
        }

        [Test]
        public void ShouldSetViewTypeToPage()
        {
            var result = new PageResult("MyWindow", false, new ViewEngineCollection());
            Assert.AreEqual("Page", result.Options["ViewType"]);
        }

        [Test]
        public void ShouldSetResetHistory()
        {
            var result = new PageResult("MyWindow", null, new ViewEngineCollection()).ClearNavigationHistory();
            Assert.AreEqual(true, result.Options["ResetNavigationHistory"]);
        }
    }
}