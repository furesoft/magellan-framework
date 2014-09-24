namespace Magellan.Framework
{
    /// <summary>
    /// Implemented usually by attributes that handle pre and post-execution of action results.
    /// </summary>
    public interface IResultFilter
    {
        /// <summary>
        /// Called before the result has been executed, providing the filter with a chance to redirect, 
        /// cancel or otherwise inspect the current result.
        /// </summary>
        /// <param name="context">The context.</param>
        void OnResultExecuting(ResultExecutingContext context);

        /// <summary>
        /// Called after the result has been executed, providing the filter with a chance to suppress or 
        /// replace any exceptions thrown by the result or to make use of the result in other ways.
        /// </summary>
        /// <param name="context">The context.</param>
        void OnResultExecuted(ResultExecutedContext context);
    }
}