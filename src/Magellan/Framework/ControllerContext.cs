using System;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides information about the current request and the controller that is serving it.
    /// </summary>
    public class ControllerContext : IDisposable
    {
        private readonly IController controller;
        private readonly ResolvedNavigationRequest request;
        private readonly ViewEngineCollection viewEngines;
        private readonly ModelBinderDictionary modelBinders;
        private readonly Action releaseCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerContext"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="request">The request.</param>
        /// <param name="viewEngines">The view engines.</param>
        public ControllerContext(IController controller, ResolvedNavigationRequest request, ViewEngineCollection viewEngines)
            : this(controller, request, viewEngines, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerContext"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="request">The request.</param>
        /// <param name="viewEngines">The view engines.</param>
        /// <param name="modelBinders"></param>
        /// <param name="releaseCallback">A callback that is invoked when the current request needs to be 
        /// cleaned up.</param>
        public ControllerContext(IController controller, ResolvedNavigationRequest request, ViewEngineCollection viewEngines, ModelBinderDictionary modelBinders, Action releaseCallback)
        {
            this.controller = controller;
            this.request = request;
            this.viewEngines = viewEngines;
            this.modelBinders = modelBinders;
            this.releaseCallback = releaseCallback;
        }

        /// <summary>
        /// Gets the model binders.
        /// </summary>
        /// <value>The model binders.</value>
        public ModelBinderDictionary ModelBinders
        {
            get { return modelBinders; }
        }

        /// <summary>
        /// Gets the active controller.
        /// </summary>
        /// <value>The controller.</value>
        public IController Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        public string ControllerName
        {
            get { return Request.RouteValues.GetOrDefault<string>("controller"); }
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName
        {
            get { return Request.RouteValues.GetOrDefault<string>("action"); }
        }

        /// <summary>
        /// Gets the current request.
        /// </summary>
        /// <value>The request.</value>
        public ResolvedNavigationRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the view engines.
        /// </summary>
        /// <value>The view engines.</value>
        public ViewEngineCollection ViewEngines
        {
            get { return viewEngines; }
        }

        /// <summary>
        /// Cleans up and releases the current controller context.
        /// </summary>
        public virtual void Dispose()
        {
            if (releaseCallback != null) releaseCallback();
        }
    }
}