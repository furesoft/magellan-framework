namespace Magellan.Exceptions
{
    /// <summary>
    /// An exception thrown when a constraint is given that is not supported.
    /// </summary>
    public sealed class UnsupportedConstraintException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedConstraintException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnsupportedConstraintException(string message) : base(message)
        {
        }
    }
}
