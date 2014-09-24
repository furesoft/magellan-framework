namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects (typically attributes) that intercept the request pipeline and can perform actions such as 
    /// redirecting, manipulating the results or blocking access to actions on a controller.
    /// </summary>
    public interface IActionFilter
    {
        /// <summary>
        /// Called before the action has been executed, providing the filter with a chance to redirect, cancel or otherwise inspect the current request.
        /// </summary>
        /// <param name="context">The context.</param>
        void OnActionExecuting(ActionExecutingContext context);

        /// <summary>
        /// Called after the action has been executed, providing the filter with a chance to suppress or replace any exceptions thrown by the action, or to manipulate the final 
        /// result of the navigation.
        /// </summary>
        /// <param name="context">The context.</param>
        void OnActionExecuted(ActionExecutedContext context);
    }
}