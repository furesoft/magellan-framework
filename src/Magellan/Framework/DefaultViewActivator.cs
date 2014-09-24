using System;
using Magellan.Diagnostics;
using Magellan.Utilities;

namespace Magellan.Framework
{
    /// <summary>
    /// The default implementation of <see cref="IViewActivator"/> which creates views using the <see cref="Activator"/> class.
    /// </summary>
    public class DefaultViewActivator : IViewActivator
    {
        /// <summary>
        /// Resolves an instance of the specified view type.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <returns>An instance of the view.</returns>
        public object Instantiate(Type viewType)
        {
            Guard.ArgumentNotNull(viewType, "viewType");
            var constructor = viewType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                TraceSources.MagellanSource.TraceError("The view type '{0}' cannot be instantiated because it does not have a public parameterless constructor.", viewType.FullName);
                throw new NotSupportedException(string.Format("The view type '{0}' must have a public parameterless constructor.", viewType.FullName));
            }
            return Activator.CreateInstance(viewType);
        }
    }
}