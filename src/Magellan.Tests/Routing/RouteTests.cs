using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class RouteTests
    {
        [Test]
        public void RouteValuesTest()
        {
            var route = new Route("/blog/{title}", () => null, null, null);
            var match = route.MatchPathToRoute("/blog/hello-world");
            Assert.AreEqual(true, match.Success);
            Assert.AreEqual("hello-world", match.Values["title"]);
        }
    }
}
