using System;
using System.Collections.Generic;
using System.Linq;
using Magellan.Events;
using Magellan.Progress;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// Encapsulates all of the details of an individual navigation request for a frame of navigation.
    /// </summary>
    public class ResolvedNavigationRequest
    {
        private readonly Guid requestId = Guid.NewGuid();
        private readonly Uri uri;
        private readonly string path;
        private readonly bool hasNonUriData;
        private readonly INavigator navigator;
        private readonly IRoute route;
        private readonly RouteValueDictionary routeData;
        private readonly IEnumerable<INavigationProgressListener> progressListeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolvedNavigationRequest"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="path">The path.</param>
        /// <param name="hasNonUriData">Indicates whether the URI is enough to get back to this request, or whether additional data lives in the route data.</param>
        /// <param name="navigator">The navigator.</param>
        /// <param name="route">The route.</param>
        /// <param name="routeData">The route data.</param>
        /// <param name="progressListeners">The progress listeners.</param>
        public ResolvedNavigationRequest(Uri uri, string path, bool hasNonUriData, INavigator navigator, IRoute route, RouteValueDictionary routeData, IEnumerable<INavigationProgressListener> progressListeners)
        {
            Guard.ArgumentNotNullOrEmpty(path, "path");
            Guard.ArgumentNotNull(navigator, "navigator");
            Guard.ArgumentNotNull(route, "route");

            this.uri = uri;
            this.path = path;
            this.hasNonUriData = hasNonUriData;
            this.navigator = navigator;
            this.route = route;
            this.routeData = routeData ?? new RouteValueDictionary();
            this.progressListeners = (progressListeners ?? new INavigationProgressListener[0]).ToList();
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <value>The URI.</value>
        public Uri Uri
        {
            get { return uri; }
        }

        /// <summary>
        /// Gets an identifier that uniquely identifies this request.
        /// </summary>
        /// <value>The request ID.</value>
        public Guid RequestId
        {
            get { return requestId; }
        }

        /// <summary>
        /// Gets the navigator that owns this navigation request.
        /// </summary>
        /// <value>The navigator.</value>
        public INavigator Navigator
        {
            get { return navigator; }
        }

        /// <summary>
        /// Gets the path that was given to create the navigation request.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// Gets the route that this navigation request has been mapped to.
        /// </summary>
        /// <value>The route.</value>
        public IRoute Route
        {
            get { return route; }
        }

        /// <summary>
        /// Gets a dictionary of parameters that were given to this navigation request.
        /// </summary>
        /// <value>The route values.</value>
        public RouteValueDictionary RouteValues
        {
            get { return routeData; }
        }

        /// <summary>
        /// Broadcasts a navigation event to all navigation progress listeners that were registered with the navigation factory.
        /// </summary>
        /// <param name="navigationEvent">The navigation event.</param>
        public void ReportProgress(NavigationEvent navigationEvent)
        {
            ((INavigationEvent) navigationEvent).Request = this;
            foreach (var listener in progressListeners)
            {
                listener.UpdateProgress(navigationEvent);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has non URI data.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has non URI data; otherwise, <c>false</c>.
        /// </value>
        public bool HasNonUriData
        {
            get { return hasNonUriData; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="ResolvedNavigationRequest"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="ResolvedNavigationRequest"/>.
        /// </returns>
        public override string ToString()
        {
            return Uri.ToString();
        }
    }
}
