using System;
using Magellan.Exceptions;

namespace Magellan.Composite.Framework
{
    /// <summary>
    /// An exception thrown by the Magellan Composite WPF extensions library. This usually indicates that
    /// a request has been made to display a Composite WPF view, but no region was specified in which to
    /// display the view.
    /// </summary>
    public class RegionNotProvidedException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNotProvidedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public RegionNotProvidedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNotProvidedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public RegionNotProvidedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
