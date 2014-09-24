namespace Magellan.Framework
{
    /// <summary>
    /// The action invoker that is used by the <see cref="Controller"/> to process the request.
    /// </summary>
    public interface IActionInvoker
    {
        /// <summary>
        /// Executes the action on the specified controller.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="modelBinders">The model binders.</param>
        void ExecuteAction(ControllerContext controllerContext, string actionName, ModelBinderDictionary modelBinders);
    }
}