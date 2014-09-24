using Magellan.Framework;
using Magellan.Views;

namespace Magellan.Exceptions
{
    /// <summary>
    /// An exception thrown by the Magellan navigation framework. This exception is thrown when the default 
    /// <see cref="DefaultViewInitializer"/> cannot assign a Model to a View. The View should implement the 
    /// <see cref="IModelBound"/> interface.
    /// </summary>
    public class ViewNotSupportedException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewNotSupportedException"/> class.
        /// </summary>
        public ViewNotSupportedException(string message) 
            : base(message)
        {
        }
    }
}