using System;
using Magellan.Diagnostics;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// A simple implementation of <see cref="IControllerFactory"/> that allows controllers to be manually 
    /// registered and created using delegates. If the controllers registered are <see cref="IDisposable"/>, 
    /// they will be disposed when the request has been processed. The controller actions are also executed
    /// asynchronously.
    /// </summary>
    public sealed class AsyncControllerFactory : ControllerFactory
    {
        /// <summary>
        /// Resolves and provides an instance of the specified controller.
        /// </summary>
        /// <param name="request">The request being served.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public override ControllerFactoryResult CreateController(ResolvedNavigationRequest request, string controllerName)
        {
            var result =  base.CreateController(request, controllerName);
            if (result.Controller is ControllerBase)
            {
                ((ControllerBase) result.Controller).ActionInvoker = new AsyncActionInvoker();
            }
            else
            {
                TraceSources.MagellanSource.TraceVerbose("The AsyncControllerFactory is in use, but the controller '{0}' does not derive from ControllerBase.", controllerName);
            }
            return result;
        }
    }
}