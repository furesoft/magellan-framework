using System.Collections.Generic;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides an interface for determining which namespaces should be searched when looking for a view. 
    /// This allows projects to use different naming conventions for storing views. See the source to 
    /// <see cref="DefaultViewNamespaceConvention"/> 
    /// </summary>
    public interface IViewNamespaceConvention
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
        IEnumerable<string> GetNamespaces(ControllerContext controllerContext, string baseNamespace, string viewName);
    }
}