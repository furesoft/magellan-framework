namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects that wrap an action on a controller and contain all the information necessary to invoke it.
    /// </summary>
    public interface IActionDescriptor
    {
        /// <summary>
        /// Executes the action on the controller using the parameters and model binders in the current request.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="modelBinders">The model binders.</param>
        /// <returns>The <see cref="ActionResult"/> returned by the controller action.</returns>
        ActionResult Execute(ControllerContext controllerContext, ModelBinderDictionary modelBinders);
    }
}