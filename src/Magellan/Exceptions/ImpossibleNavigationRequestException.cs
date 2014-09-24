namespace Magellan.Exceptions
{
    /// <summary>
    /// An exception thrown by the Magellan navigation framework. This exception is usually thrown when 
    /// attempting to navigate to a Page without a navigation service being available in which to host the 
    /// page, or other similarly impossible requests. 
    /// </summary>
    public class ImpossibleNavigationRequestException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImpossibleNavigationRequestException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ImpossibleNavigationRequestException(string message) : base(message)
        {
        }
    }
}