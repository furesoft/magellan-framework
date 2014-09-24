using System;
using Magellan.Routing;

namespace Magellan
{
    /// <summary>
    /// Represents a route request, which is given to an <see cref="INavigator"/> to execute.
    /// </summary>
    public class NavigationRequest
    {
        private readonly Uri uri;
        private readonly RouteValueDictionary routeData;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRequest"/> class.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        public NavigationRequest(RouteValueDictionary routeData) : this(null, routeData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRequest"/> class.
        /// </summary>
        /// <param name="uri">The URI being navigated to. Can be null to resolve a path from route data.</param>
        public NavigationRequest(Uri uri) : this(uri, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRequest"/> class.
        /// </summary>
        /// <param name="uri">The URI being navigated to. Can be null to resolve a path from route data.</param>
        /// <param name="routeData">The route data.</param>
        public NavigationRequest(Uri uri, RouteValueDictionary routeData)
        {
            this.uri = uri;
            this.routeData = routeData;
        }

        /// <summary>
        /// Gets the URI. Can be null if the route is meant to be resolved from route data.
        /// </summary>
        /// <value>The URI.</value>
        public Uri Uri
        {
            get { return uri; }
        }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <value>The route data.</value>
        public RouteValueDictionary RouteData
        {
            get { return routeData; }
        }
    }
}