using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects that manage the lifecycle of controllers.
    /// </summary>
    public interface IControllerFactory
    {
        /// <summary>
        /// Resolves and provides an instance of the specified controller.
        /// </summary>
        /// <param name="request">The request being served.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>An object containing the controller and potentially the logic for cleaning up and 
        /// releasing resources allocated when the controller was allocated.</returns>
        ControllerFactoryResult CreateController(ResolvedNavigationRequest request, string controllerName);
    }
}