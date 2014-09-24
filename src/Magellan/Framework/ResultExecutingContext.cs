using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides information to <see cref="IResultFilter">result filters</see> about a navigation result 
    /// before is is executed.
    /// </summary>
    public class ResultExecutingContext
    {
        private readonly ControllerContext controllerContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultExecutingContext"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="result">The result.</param>
        public ResultExecutingContext(ControllerContext controllerContext, ActionResult result)
        {
            Result = result;
            this.controllerContext = controllerContext;
        }

        /// <summary>
        /// Gets or sets the action result. This property can be set to override the final result before it 
        /// is executed.
        /// </summary>
        /// <value>The result.</value>
        public ActionResult Result { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the result should be cancelled and not executed at all.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        public bool Cancel { get; set; }

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
    }
}