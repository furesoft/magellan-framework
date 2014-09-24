using System.Collections.Generic;

namespace Magellan.Framework
{
    /// <summary>
    /// The default implementation of <see cref="IViewNamespaceConvention"/>. Assumes a Views folder is used 
    /// to contain the views.
    /// </summary>
    public class DefaultViewNamespaceConvention : IViewNamespaceConvention
    {
        /// <summary>
        /// Given a controller and the start of a namespace, generates a set of combinations where views
        /// may be kept.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="baseNamespace">The base namespace.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>
        /// A collection of possible view namespaces.
        /// </returns>
        public virtual IEnumerable<string> GetNamespaces(ControllerContext controllerContext, string baseNamespace, string viewName)
        {
            return new[]
                       {
                           baseNamespace + ".Views." + controllerContext.ControllerName,
                           baseNamespace + ".Views." + viewName,
                           baseNamespace + ".Views." + controllerContext.ControllerName + "." + viewName,
                           baseNamespace + ".Views",
                           baseNamespace + "." + controllerContext.ControllerName,
                           baseNamespace
                       };
        }
    }
}