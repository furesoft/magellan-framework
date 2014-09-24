using System;

namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects that support adding temporary notifications for users.
    /// </summary>
    public interface IFlasher
    {
        /// <summary>
        /// Flashes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Flash(string message);
        
        /// <summary>
        /// Flashes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="disappears">The disappears.</param>
        void Flash(string message, TimeSpan disappears);
        
        /// <summary>
        /// Flashes the specified flash.
        /// </summary>
        /// <param name="flash">The flash.</param>
        void Flash(Flash flash);
        
        /// <summary>
        /// Clears the flashes.
        /// </summary>
        void ClearFlashes();
    }
}