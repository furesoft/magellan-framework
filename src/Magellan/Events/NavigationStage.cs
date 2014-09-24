using Magellan.Routing;

namespace Magellan.Events
{
    /// <summary>
    /// A base class for navigation events.
    /// </summary>
    public abstract class NavigationEvent : INavigationEvent
    {
        private ResolvedNavigationRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationEvent"/> class.
        /// </summary>
        protected NavigationEvent()
        {
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public ResolvedNavigationRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>The request.</value>
        ResolvedNavigationRequest INavigationEvent.Request
        {
            get
            {
                return Request;
            }
            set
            {
                request = value;
            }
        }
    }
}