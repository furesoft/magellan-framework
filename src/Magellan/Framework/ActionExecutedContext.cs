using System;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides information to <see cref="IActionFilter">action filters</see> about the current request.
    /// </summary>
    public class ActionExecutedContext
    {
        private readonly ControllerContext controllerContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionExecutedContext"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="result">The result.</param>
        /// <param name="exception">The exception.</param>
        public ActionExecutedContext(ControllerContext controllerContext, ActionResult result, Exception exception)
        {
            this.controllerContext = controllerContext;
            Exception = exception;
            Result = result;
        }

        /// <summary>
        /// Gets or sets the result of the navigation request. The <see cref="IActionFilter"/> can write to 
        /// this to change or redirect the navigation result.
        /// </summary>
        public ActionResult Result { get; set; }

        /// <summary>
        /// Gets information about the controller and request.
        /// </summary>
        public ControllerContext ControllerContext
        {
            get { return controllerContext; }
        }

        /// <summary>
        /// Gets the navigation request.
        /// </summary>
        public ResolvedNavigationRequest Request
        {
            get { return controllerContext.Request; }
        }

        /// <summary>
        /// Gets the exception that was thrown by the controller during the navigation request, and allows 
        /// the <see cref="IActionFilter"/> to override the exception (to mask it, for example).
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets whether any of the <see cref="IActionFilter">action filters</see> have handled the 
        /// request. If this is set to true, the exception will not be rethrown by the 
        /// <see cref="IActionInvoker"/>.
        /// </summary>
        public bool ExceptionHandled { get; set; }
    }
}