using Magellan.Diagnostics;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Represents a <see cref="ActionResult"/> that redirects to another controller action.
    /// </summary>
    public class RedirectResult : ActionResult
    {
        private readonly RouteValueDictionary request;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectResult"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public RedirectResult(RouteValueDictionary request)
        {
            this.request = request;
        }

        /// <summary>
        /// Gets the new request.
        /// </summary>
        /// <value>The new request.</value>
        public RouteValueDictionary NewRequest
        {
            get { return request; }
        }

        /// <summary>
        /// When implemented in a derived class, performs the bulk of the action rendering.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        protected override void ExecuteInternal(ControllerContext controllerContext)
        {
            TraceSources.MagellanSource.TraceVerbose("The RedirectResult is navigating to the request '{0}'.", NewRequest);

            var dispatcher = controllerContext.Request.Navigator.Dispatcher;
            dispatcher.Dispatch(
                delegate
                {
                    controllerContext.Request.Navigator.Navigate(NewRequest);
                });
        }
    }
}
