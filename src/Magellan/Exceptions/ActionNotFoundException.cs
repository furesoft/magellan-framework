using Magellan.Framework;

namespace Magellan.Exceptions
{
    /// <summary>
    /// An exception thrown by the Magellan navigation framework when an action is not found on the 
    /// controller. This usually indicates that the controller exists, but the name of the action is 
    /// incorrect. Generally, actions should be public instance methods, and must return 
    /// <see cref="ActionResult"/> types.
    /// </summary>
    public class ActionNotFoundException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ActionNotFoundException(string message) : base(message)
        {
        }
    }
}