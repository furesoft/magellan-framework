namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by all <see cref="IController">controllers</see>.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="context">Context information about the current navigation request.</param>
        void Execute(ControllerContext context);
    }
}