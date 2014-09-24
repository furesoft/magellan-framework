using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides information to <see cref="IActionFilter">action filters</see> about an incoming navigation 
    /// request.
    /// </summary>
    public class ActionExecutingContext
    {
        private readonly ControllerContext controllerContext;
        private readonly ModelBinderDictionary modelBinders;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionExecutingContext"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="modelBinders">The model binders.</param>
        public ActionExecutingContext(ControllerContext controllerContext, ModelBinderDictionary modelBinders)
        {
            this.controllerContext = controllerContext;
            this.modelBinders = modelBinders;
        }

        /// <summary>
        /// Gets or sets the navigation result. When this property is set, the controller will be bypassed 
        /// and the action will be rendered. This allows action filters to cancel navigation.
        /// </summary>
        public ActionResult OverrideResult { get; set; }

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
        /// Gets the model binders that will be used in the request.
        /// </summary>
        public ModelBinderDictionary ModelBinders
        {
            get { return modelBinders; }
        }
    }
}