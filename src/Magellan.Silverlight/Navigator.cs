using System;
using System.Linq;
using System.Text;
using Magellan.Abstractions;
using Magellan.Exceptions;
using Magellan.Progress;
using Magellan.Routing;
using Magellan.Utilities;

namespace Magellan
{
    /// <summary>
    /// The one and only implementation of <see cref="INavigator"/>, produced by <see cref="NavigatorFactory"/>.
    /// </summary>
    internal class Navigator : NavigationServiceDecorator, INavigator
    {
        private readonly INavigatorFactory _parent;
        private readonly string _scheme;
        private readonly IRouteResolver _routes;
        private readonly NavigationProgressListenerCollection _progressListeners = new NavigationProgressListenerCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="Navigator"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="scheme">The URI scheme.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="navigationService">The navigation service.</param>
        public Navigator(INavigatorFactory parent, string scheme, IRouteResolver routes, Func<INavigationService> navigationService)
            : base(navigationService)
        {
            Guard.ArgumentNotNull(parent, "root");
            Guard.ArgumentNotNull(routes, "routes");
            Guard.ArgumentNotNull(navigationService, "navigationService");

            _parent = parent;
            _scheme = scheme;
            _routes = routes;
        }

        /// <summary>
        /// Gets the scheme that will be used for any URI's built by this navigator.
        /// </summary>
        /// <value>The scheme.</value>
        public string Scheme
        {
            get { return _scheme; }
        }

        /// <summary>
        /// Gets the <see cref="INavigatorFactory"/> that produced this <see cref="INavigator"/>.
        /// </summary>
        /// <value>The factory.</value>
        public INavigatorFactory Factory
        {
            get { return _parent; }
        }

        /// <summary>
        /// Gets a collection of registered progress observers.
        /// </summary>
        /// <value>The progress listeners.</value>
        public NavigationProgressListenerCollection ProgressListeners
        {
            get { return _progressListeners; }
        }

        /// <summary>
        /// Resolves and navigates to the given route request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void ProcessRequest(NavigationRequest request)
        {
            if (request.Uri == null)
            {
                ExecuteRequestWithoutUri(request.RouteData);
            }
            else
            {
                ExecuteRequestWithUri(request.Uri, request.RouteData);
            }
        }

        /// <summary>
        /// Navigates to the specified URI, resolving the first matching route.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="additionalData">Additional data (like post data) that is not in the URI but is used
        /// for navigation.</param>
        private void ExecuteRequestWithUri(Uri requestUri, RouteValueDictionary additionalData)
        {
            additionalData = additionalData ?? new RouteValueDictionary();
            var path = requestUri.GetComponents(UriComponents.Host | UriComponents.Path, UriFormat.Unescaped);
            var queryString = requestUri.GetComponents(UriComponents.Query, UriFormat.Unescaped);

            var route = _routes.MatchPathToRoute(path);
            if (!route.Success)
            {
                throw new UnroutableRequestException(string.Format("The request URI '{0}' could not be routed. {1}{2}", requestUri, Environment.NewLine, route.FailReason));
            }

            // Create a value dictionary with all of the known information - post data, query string data, 
            // and data extracted from the route. We'll use this for the navigation request.
            var data = new RouteValueDictionary(additionalData);
            data.AddRange(route.Values, true);
            var queryValues = new QueryValueCollection(queryString);
            foreach (var pair in queryValues)
            {
                if (!data.ContainsKey(pair.Key))
                {
                    data[pair.Key] = pair.Value;
                }
            }

            var handler = route.Route.CreateRouteHandler();

            handler.ProcessRequest(
                new ResolvedNavigationRequest(
                    requestUri,
                    path,
                    additionalData.Count > 0,
                    this,
                    route.Route,
                    data,
                    _progressListeners.Union(Factory.ProgressListeners ?? new NavigationProgressListenerCollection()).ToList()
                    ));
        }

        /// <summary>
        /// Resolves and navigates to the first route that matches the given set of route values.
        /// </summary>
        /// <param name="request">The request.</param>
        private void ExecuteRequestWithoutUri(RouteValueDictionary request)
        {
            var path = _routes.MatchRouteToPath(request);
            if (!path.Success)
            {
                throw new UnroutableRequestException(string.Format("The request values '{0}' could not be routed. {1}{2}", request, Environment.NewLine, path.FailReason));
            }

            var handler = path.Route.CreateRouteHandler();

            var uriBuilder = new StringBuilder();
            uriBuilder.Append(_scheme).Append("://").Append(path.Path);

            var queryValues = path.LeftOverValues.Where(x => x.Value is IFormattable || x.Value is string)
                .Select(x => x.Key + "=" + Uri.EscapeDataString(x.Value.ToString()))
                .ToArray();

            if (queryValues.Length > 0)
            {
                uriBuilder.Append("?");
                uriBuilder.Append(string.Join("&", queryValues));
            }

            var uri = new Uri(uriBuilder.ToString());

            handler.ProcessRequest(
                new ResolvedNavigationRequest(
                    uri,
                    path.Path,
                    path.LeftOverValues.Count > queryValues.Length,
                    this,
                    path.Route,
                    path.RouteValues,
                    _progressListeners.Union(Factory.ProgressListeners ?? new NavigationProgressListenerCollection()).ToList()
                    ));
        }
    }
}