using System.Windows.Navigation;
using Magellan.Exceptions;
using Magellan.Utilities;

namespace Magellan.Framework
{
    /// <summary>
    /// An action result that renders a page to a <see cref="NavigationService"/>.
    /// </summary>
    public abstract class ViewResult : ActionResult
    {
        private readonly ViewResultOptions options = new ViewResultOptions();
        private readonly string viewName;
        private readonly ViewEngineCollection viewEngines;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model that will be bound to the view.</param>
        /// <param name="viewEngines">The set of view engines that can help to activate this view.</param>
        protected ViewResult(string viewName, object model, ViewEngineCollection viewEngines)
        {
            this.viewName = viewName;
            this.viewEngines = viewEngines;
            Options.SetModel(model);
        }

        /// <summary>
        /// Gets the name of the view that will be rendered.
        /// </summary>
        /// <value>The name of the view.</value>
        public string ViewName
        {
            get { return viewName; }
        }

        /// <summary>
        /// Gets the model object passed to this view result.
        /// </summary>
        /// <value>The model.</value>
        public object Model
        {
            get { return Options.GetModel(); }
        }

        /// <summary>
        /// Gets a set of parameters that are shared with the View Engine when this view is rendered. This allows the view engine to 
        /// make use of additional context information when rendering the view.
        /// </summary>
        /// <value>The view parameters.</value>
        public ViewResultOptions Options
        {
            get { return options; }
        }

        /// <summary>
        /// Gets the view engine result (only available after the view result has been executed).
        /// </summary>
        /// <value>The view engine result.</value>
        public ViewEngineResult ViewEngineResult { get; private set; }

        /// <summary>
        /// Executes the action result.
        /// </summary>
        /// <param name="controllerContext"></param>
        protected override void ExecuteInternal(ControllerContext controllerContext)
        {
            Guard.ArgumentNotNull(controllerContext, "controllerContext");

            var viewName = this.viewName ?? controllerContext.ActionName;
            ViewEngineResult = viewEngines.FindView(controllerContext, Options, viewName);
            if (ViewEngineResult.Success)
            {
                ViewEngineResult.Render();
            }
            else
            {
                throw new ViewNotFoundException(controllerContext.ControllerName, controllerContext.ActionName, viewName, ViewEngineResult.SearchLocations);
            }
        }
    }
}