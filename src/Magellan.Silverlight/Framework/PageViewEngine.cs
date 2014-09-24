using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace Magellan.Framework
{
    /// <summary>
    /// A view engine for rendering Silverlight pages using the Microsoft Silverlight navigation framework.
    /// </summary>
    public class PageViewEngine : ReflectionBasedViewEngine, IViewNamingConvention
    {
        private readonly IViewActivator _viewActivator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewEngine"/> class.
        /// </summary>
        /// <param name="viewActivator">The view activator.</param>
        public PageViewEngine(IViewActivator viewActivator, params Assembly[] additionalViewAssemblies)
            : base(additionalViewAssemblies)
        {
            _viewActivator = viewActivator;
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
            return new[] { baseName, baseName + "View", baseName + "Page" };
        }

        /// <summary>
        /// When implemented in a derived class, indicated whether this view engine should even attempt to
        /// locate views and handle the request. If this method returns false, no reflection will be
        /// performed.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        protected override bool ShouldHandle(ControllerContext controllerContext, ViewResultOptions options, string viewName)
        {
            return options.GetViewType() == "Page"
                   && base.ShouldHandle(controllerContext, options, viewName);
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
            return candidates.Where(c => typeof(UserControl).IsAssignableFrom(c));
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
            return new PageViewEngineResult(_viewActivator, type, options, controllerContext);
        }
    }
}