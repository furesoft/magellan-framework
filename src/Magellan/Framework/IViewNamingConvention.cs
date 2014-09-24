using System.Collections.Generic;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides an interface for generating the alternative names for a view. This allows projects to use 
    /// different naming conventions for views. See the source to <see cref="DefaultViewNamingConvention"/>.
    /// </summary>
    public interface IViewNamingConvention
    {
        /// <summary>
        /// Gets the alternative names for a given view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="baseName">Name of the base.</param>
        /// <returns></returns>
        IEnumerable<string> GetAlternativeNames(ControllerContext controllerContext, string baseName);
    }
}