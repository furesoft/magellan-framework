using Magellan.Routing;

namespace Magellan.Exceptions
{
    /// <summary>
    /// An exception thrown when an invalid route (for example, a route with a specifcation using illegal 
    /// characters) is registered with a route collection.
    /// </summary>
    public sealed class InvalidRouteException : NavigationException
    {
        private readonly IRoute route;
        private readonly RouteValidationResult result;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRouteException"/> class.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="message">The message.</param>
        public InvalidRouteException(IRoute route, string message) : this(route, null, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRouteException"/> class.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="result">The result.</param>
        /// <param name="message">The message.</param>
        public InvalidRouteException(IRoute route, RouteValidationResult result, string message) : base(message)
        {
            this.route = route;
            this.result = result;
        }

        /// <summary>
        /// Gets the route that was invalid.
        /// </summary>
        /// <value>The route.</value>
        public IRoute Route
        {
            get { return route; }
        }

        /// <summary>
        /// Gets the details about the validation error that occurred.
        /// </summary>
        /// <value>The result.</value>
        public RouteValidationResult Result
        {
            get { return result; }
        }
    }
}