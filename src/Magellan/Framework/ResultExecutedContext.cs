using System;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides information to <see cref="IResultFilter">result filters</see> about a navigation result
    /// after it is executed.
    /// </summary>
    public class ResultExecutedContext
    {
        private readonly ControllerContext controllerContext;
        private readonly ActionResult result;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultExecutedContext"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="result">The result.</param>
        /// <param name="exception">An exception that may have been thrown.</param>
        public ResultExecutedContext(ControllerContext controllerContext, ActionResult result, Exception exception)
        {
            this.result = result;
            Exception = exception;
            this.controllerContext = controllerContext;
        }

        /// <summary>
        /// Gets the controller context.
        /// </summary>
        /// <value>The controller context.</value>
        public ControllerContext ControllerContext
        {
            get { return controllerContext; }
        }

        /// <summary>
        /// Gets the navigation request.
        /// </summary>
        /// <value>The request.</value>
        public ResolvedNavigationRequest Request
        {
            get { return controllerContext.Request; }
        }

        /// <summary>
        /// Gets or sets the result of the navigation request.
        /// </summary>
        /// <value>The result.</value>
        public ActionResult Result
        {
            get { return result; }
        }

        /// <summary>
        /// Gets or sets the exception that was thrown.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets whether any of the <see cref="IResultFilter">result filters</see> have handled the 
        /// result. If this is set to true, the exception will not be rethrown by the 
        /// <see cref="IActionInvoker"/>.
        /// </summary>
        public bool ExceptionHandled { get; set; }
    }
}