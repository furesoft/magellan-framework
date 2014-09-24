
namespace Magellan.Framework
{
    /// <summary>
    /// Serves as a base class for view engine results that rely on WPF objects. Provides a helper method for 
    /// automatically wiring up the data context and <see cref="INavigationAware"/> support.
    /// </summary>
    public abstract class FrameworkElementViewEngineResult : ViewEngineResult
    {
        private readonly ControllerContext controllerContext;
        private readonly ViewResultOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkElementViewEngineResult"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The options.</param>
        protected FrameworkElementViewEngineResult(ControllerContext controllerContext, ViewResultOptions options) 
            : base(true, new string[0])
        {
            this.controllerContext = controllerContext;
            this.options = options;
            ViewInitializer = new DefaultViewInitializer(controllerContext.ModelBinders);
        }

        /// <summary>
        /// Gets or sets the view initializer.
        /// </summary>
        /// <value>The view initializer.</value>
        public IViewInitializer ViewInitializer { get; set; }

        /// <summary>
        /// Gets the controller context.
        /// </summary>
        /// <value>The controller context.</value>
        public ControllerContext ControllerContext
        {
            get { return controllerContext; }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public object Model
        {
            get { return Options.GetModel(); }
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        public ViewResultOptions Options
        {
            get { return options; }
        }
    }
}