using System;

namespace Magellan.Framework
{
    /// <summary>
    /// An exception raised to wrap an exception thrown on a background thread (to preseve the stack trace).
    /// </summary>
    public class BackgroundOperationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BackgroundOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}