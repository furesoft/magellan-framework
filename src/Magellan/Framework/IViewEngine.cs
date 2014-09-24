namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by the service that resolves and navigates to views.
    /// </summary>
    public interface IViewEngine
    {
        /// <summary>
        /// Attempts to find the view, or returns information about the locations that were searched.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The options.</param>
        /// <param name="view">The view.</param>
        /// <returns>
        /// A <see cref="ViewEngineResult"/> containing the resolved view or information about the locations that were searched.
        /// </returns>
        ViewEngineResult FindView(ControllerContext controllerContext, ViewResultOptions options, string view);
    }
}