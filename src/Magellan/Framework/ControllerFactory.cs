using System;
using System.Collections.Generic;
using Magellan.Routing;
using Magellan.Utilities;
using Magellan.Diagnostics;
using System.Globalization;

namespace Magellan.Framework
{
    /// <summary>
    /// A simple implementation of <see cref="IControllerFactory"/> that allows controllers to be manually 
    /// registered and created using delegates. If the controllers registered are <see cref="IDisposable"/>, 
    /// they will be disposed when the request has been processed.
    /// </summary>
    public class ControllerFactory : IControllerFactory
    {
        private readonly Dictionary<string, Func<IController>> controllerBuilders = new Dictionary<string, Func<IController>>();

        /// <summary>
        /// Registers the specified controller using a delegate to create it.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerBuilder">The controller builder.</param>
        public void Register(string controllerName, Func<IController> controllerBuilder)
        {
            Guard.ArgumentNotNull(controllerName, "controllerName");
            Guard.ArgumentNotNull(controllerBuilder, "controllerBuilder");

            TraceSources.MagellanSource.TraceVerbose("Registering controller '{0}'", controllerName);
            
            if (controllerBuilders.ContainsKey(controllerName.ToUpper(CultureInfo.InvariantCulture)))
            {
                throw new ArgumentException(string.Format("A controller with the name '{0}' has already been added.", controllerName));
            }
            controllerBuilders.Add(controllerName.ToUpper(CultureInfo.InvariantCulture), controllerBuilder);
        }

        /// <summary>
        /// Resolves and provides an instance of the specified controller.
        /// </summary>
        /// <param name="request">The request being served.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public virtual ControllerFactoryResult CreateController(ResolvedNavigationRequest request, string controllerName)
        {
            Guard.ArgumentNotNull(request, "request");
            Guard.ArgumentNotNullOrEmpty(controllerName, "controllerName");
            
            TraceSources.MagellanSource.TraceVerbose("Resolving controller '{0}' for request '{1}'", controllerName, request);

            controllerName = controllerName.ToUpper(CultureInfo.InvariantCulture);
            if (!controllerBuilders.ContainsKey(controllerName))
            {
                TraceSources.MagellanSource.TraceError("Failed to resolve controller '{0}' for request '{1}'", controllerName, request);
                throw new ArgumentException(string.Format("A controller by the name of '{0}' could not be found. Please ensure the controller has been registered.", controllerName), "controllerName");
            }

            var controller = controllerBuilders[controllerName]();
            return new ControllerFactoryResult(controller, 
                () =>
                    {
                        var disposableController = controller as IDisposable;
                        if (disposableController != null)
                        {
                            TraceSources.MagellanSource.TraceVerbose("The controller '{0}' is disposable, so it is being disposed.", controllerName);
                            disposableController.Dispose();
                        }
                    }
                );
        }
    }
}
