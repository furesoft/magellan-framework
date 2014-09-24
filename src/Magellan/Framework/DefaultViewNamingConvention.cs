using System.Collections.Generic;

namespace Magellan.Framework
{
    /// <summary>
    /// The default implementation of <see cref="IViewNamingConvention"/>. Assumes a Views folder is used 
    /// to contain the views.
    /// </summary>
    public class DefaultViewNamingConvention : IViewNamingConvention
    {
        /// <summary>
        /// Gets the alternative names for a given view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="baseName">Name of the base.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetAlternativeNames(ControllerContext controllerContext, string baseName)
        {
            return new[] {baseName, baseName + "View"};
        }
    }
}