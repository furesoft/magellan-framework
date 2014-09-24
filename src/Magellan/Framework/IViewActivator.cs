using System;

namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by services that can resolve and prepare a view given its type.
    /// </summary>
    public interface IViewActivator
    {
        /// <summary>
        /// Resolves an instance of the specified view type.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <returns>An instance of the view.</returns>
        object Instantiate(Type viewType);
    }
}