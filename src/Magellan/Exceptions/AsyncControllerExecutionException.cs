using System;
using Magellan.Framework;

namespace Magellan.Exceptions
{
    /// <summary>
    /// Occurs when an exception is thrown when execuing a controller request asynchronously using the 
    /// <see cref="AsyncActionInvoker"/>.
    /// </summary>
    public class AsyncControllerExecutionException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncControllerExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public AsyncControllerExecutionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}