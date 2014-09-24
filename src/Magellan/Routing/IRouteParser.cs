namespace Magellan.Routing
{
    /// <summary>
    /// Implemented by objects which can take a route specification, crunch it and produce a 
    /// <see cref="ParsedRoute"/> which will later be used for matching.
    /// </summary>
    public interface IRouteParser
    {
        /// <summary>
        /// Parses the given route specification, producing a <see cref="ParsedRoute"/>.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="routeSpecification">The route specification.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <returns></returns>
        ParsedRoute Parse(IRoute route, string routeSpecification, RouteValueDictionary defaults, RouteValueDictionary constraints);
    }
}