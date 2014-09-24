namespace Magellan.Routing
{
    /// <summary>
    /// Represents an individual route.
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// Gives the route a chance to inialize itself. This method is called when the route is registered
        /// in a route collection. It should be used for any pre-compilation, caching or validation tasks.
        /// </summary>
        void Validate();

        /// <summary>
        /// Matches a path, such as "/patients/list", to this route, indicating whether or not the route 
        /// successfully matches the path.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An object indicating the result of the path match.</returns>
        RouteMatch MatchPathToRoute(string request);

        /// <summary>
        /// Matches a set of route data to this route, producing the probable path that should be used if 
        /// this route was linked to.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>An object indicating the result of the route match.</returns>
        PathMatch MatchRouteToPath(RouteValueDictionary values);

        /// <summary>
        /// Selects or creates an appropriate route handler for this route.
        /// </summary>
        /// <returns>An <see cref="IRouteHandler"/> that will execute a navigation request to this route.
        /// </returns>
        IRouteHandler CreateRouteHandler();
    }
}