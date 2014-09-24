using Magellan.Routing;

namespace Magellan.Tests.Helpers
{
    public class TestRouteCatalog : RouteCatalog
    {
        public void Register(string path, object defaults = null, object constraints = null)
        {
            var route = new Route(path, () => new TestRouteHandler(), new RouteValueDictionary(defaults), new RouteValueDictionary(constraints));
            Add(route);
        }
    }
}