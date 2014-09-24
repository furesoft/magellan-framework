using System;
using Magellan.Utilities;

namespace Magellan.Exceptions
{
    /// <summary>
    /// Base class for exceptions thrown by the Magellan navigation framework.
    /// </summary>
    public abstract class NavigationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        protected NavigationException(string message) 
            : base(message.CleanErrorMessage())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        protected NavigationException(string message, Exception inner)
            : base(message.CleanErrorMessage(), inner)
        {
        }
    }
}
