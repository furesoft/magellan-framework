using System.Collections.Generic;
using Magellan.ComponentModel;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// A base class that contains a set of registered routes.
    /// </summary>
    public abstract class RouteCatalog
    {
        private readonly Set<IRoute> routes = new Set<IRoute>();

        /// <summary>
        /// Registers the specified route.
        /// </summary>
        /// <param name="route">The route.</param>
        public virtual void Add(IRoute route)
        {
            Guard.ArgumentNotNull(route, "route");
            route.Validate();
            routes.Add(route);
        }

        /// <summary>
        /// Unregisters the specified route.
        /// </summary>
        /// <param name="route">The route.</param>
        public virtual void Remove(IRoute route)
        {
            routes.Remove(route);
        }

        /// <summary>
        /// Gets the registered routes.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IRoute> GetRoutes()
        {
            return routes;
        }
    }
}