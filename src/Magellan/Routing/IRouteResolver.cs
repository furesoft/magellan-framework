namespace Magellan.Routing
{
    /// <summary>
    /// Implemented by services that can resolve navigation requests to a route.
    /// </summary>
    public interface IRouteResolver
    {
        /// <summary>
        /// Matches a path, such as "/patients/list", to route, by finding the first route that could
        /// possibly resolve to that path.
        /// </summary>
        /// <param name="requestPath">The request path.</param>
        /// <returns>
        /// A result containing the route, if matched, or a reason for failure if not matched.
        /// </returns>
        RouteMatch MatchPathToRoute(string requestPath);

        /// <summary>
        /// Matches a collection of route values to a path. This method is primarily used when producing a
        /// link or navigating programmatically.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A result containing the route, if matched, or a reason for failure if not matched.
        /// </returns>
        PathMatch MatchRouteToPath(RouteValueDictionary values);
    }
}