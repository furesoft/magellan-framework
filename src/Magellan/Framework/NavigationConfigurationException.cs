using System;

namespace Magellan.Framework
{
    /// <summary>
    /// An exception thrown by the Magellan navigation framework, usually when part of the Magellan framework 
    /// has not been configured properly. Common examples of this error include not having any view engines 
    /// registered, or not having any controllers registered.
    /// </summary>
    public class NavigationConfigurationException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NavigationConfigurationException(string message) : base(message)
        {
        }
    }
}