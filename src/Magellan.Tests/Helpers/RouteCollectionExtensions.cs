using Magellan.Routing;

namespace Magellan.Tests.Helpers
{
    public static class RouteCollectionExtensions
    {
        public static PathMatch MatchRouteToPath(this RouteResolver routes, object routeValues)
        {
            return routes.MatchRouteToPath(new RouteValueDictionary(routeValues));
        }
    }
}
