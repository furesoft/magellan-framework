using System;
using System.Collections.Generic;
using System.Linq;
using Magellan.ComponentModel;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// Keeps track of a collection of routes. 
    /// </summary>
    public class RouteResolver : IRouteResolver
    {
        private readonly Set<RouteCatalog> catalogs = new Set<RouteCatalog>();
        private readonly object routesLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteResolver"/> class.
        /// </summary>
        /// <param name="catalogs">The catalogs.</param>
        public RouteResolver(params RouteCatalog[] catalogs)
        {
            this.catalogs.AddRange(catalogs);
        }

        private List<IRoute> GetRoutes()
        {
            var results = new List<IRoute>();
            lock (routesLock)
            {
                foreach (var catalog in catalogs)
                {
                    results.AddRange(catalog.GetRoutes());
                }
            }
            return results;
        }

        /// <summary>
        /// Matches a path, such as "/patients/list", to route, by finding the first route that could
        /// possibly resolve to that path.
        /// </summary>
        /// <param name="requestPath">The request path.</param>
        /// <returns>
        /// A result containing the route, if matched, or a reason for failure if not matched.
        /// </returns>
        public RouteMatch MatchPathToRoute(string requestPath)
        {
            var routes = GetRoutes();

            if (routes.Count == 0)
            {
                return RouteMatch.Failure(null, "No routes are registered in this route collection.");
            }

            var fails = new List<RouteMatch>();
            foreach (var route in routes)
            {
                var match = route.MatchPathToRoute(requestPath);
                if (match.Success)
                {
                    return match;
                }
                fails.Add(match);
            }

            return RouteMatch.Failure(
                null,
                string.Join(Environment.NewLine,
                    fails.Select(fail => string.Format("- Route with specification '{0}' did not match: {1}.", fail.Route, fail.FailReason).CleanErrorMessage()).ToArray()
                    ));
        }

        /// <summary>
        /// Matches a collection of route values to a path. This method is primarily used when producing a
        /// link or navigating programmatically.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A result containing the route, if matched, or a reason for failure if not matched.
        /// </returns>
        public PathMatch MatchRouteToPath(RouteValueDictionary values)
        {
            var routes = GetRoutes();
            var fails = new List<PathMatch>();

            if (routes.Count == 0)
            {
                return PathMatch.Failure(null, "No routes are registered in this route collection.");
            }

            foreach (var route in routes)
            {
                var match = route.MatchRouteToPath(values);
                if (match.Success)
                {
                    return match;
                }

                fails.Add(match);
            }

            return PathMatch.Failure(
                null,
                string.Join(Environment.NewLine,
                    fails.Select(fail => string.Format("- Route with specification '{0}' did not match: {1}.", fail.Route, fail.FailReason).CleanErrorMessage()).ToArray()
                    ));
        }
    }
}