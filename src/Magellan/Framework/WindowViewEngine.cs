using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Magellan.Framework
{
    /// <summary>
    /// A <see cref="IViewEngine"/> for Window-derived UI elements.
    /// </summary>
    public class WindowViewEngine : ReflectionBasedViewEngine, IViewNamingConvention
    {
        private readonly IViewActivator viewActivator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowViewEngine"/> class.
        /// </summary>
        /// <param name="viewActivator">The view activator.</param>
        /// <param name="additionalViewAssemblies">Any additional view assemblies. These will be searched when attempting to resolve a view.</param>
        public WindowViewEngine(IViewActivator viewActivator, params Assembly[] additionalViewAssemblies)
            : base(additionalViewAssemblies)
        {
            this.viewActivator = viewActivator;
            NamingConvention = this;
        }

        /// <summary>
        /// Gets the alternative names for a given view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="baseName">Name of the base.</param>
        /// <returns></returns>
        public IEnumerable<string> GetAlternativeNames(ControllerContext controllerContext, string baseName)
        {
            return new[] { baseName, baseName + "View", baseName + "Window", baseName + "Dialog", baseName + "ViewWindow", baseName + "ViewDialog" };
        }

        /// <summary>
        /// When implemented in a derived class, indicated whether this view engine should even attempt to locate views and handle the request.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        protected override bool ShouldHandle(ControllerContext controllerContext, ViewResultOptions options, string viewName)
        {
            var viewType = options.GetViewType();
            return viewType == "Dialog" || viewType == "Window";
        }

        /// <summary>
        /// When implemented in a derived class, allows the derived class to restrict the criteria used to select potential types.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="candidates">The list of candidate types.</param>
        /// <returns></returns>
        protected override IEnumerable<Type> FilterCandidateTypes(ControllerContext controllerContext, ViewResultOptions options, string viewName, IEnumerable<Type> candidates)
        {
            return candidates.Where(type => typeof (Window).IsAssignableFrom(type));
        }

        /// <summary>
        /// Creates the view result for the specified view type.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected override ViewEngineResult CreateViewResult(ControllerContext controllerContext, ViewResultOptions options, Type type)
        {
            return new WindowViewEngineResult(type, options, controllerContext, viewActivator);
        }
    }
}
