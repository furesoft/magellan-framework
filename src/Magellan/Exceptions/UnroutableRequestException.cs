namespace Magellan.Exceptions
{
    /// <summary>
    /// An exception thrown when an attempt is made to navigate using a route, but the route cannot be 
    /// resolved.
    /// </summary>
    public class UnroutableRequestException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnroutableRequestException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnroutableRequestException(string message) : base(message)
        {
        }
    }


}
