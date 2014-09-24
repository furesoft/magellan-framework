using System;
using System.Collections.Generic;
using System.Reflection;
using Magellan.Diagnostics;
using Magellan.Utilities;

namespace Magellan.Framework
{
    /// <summary>
    /// The default implementation of <see cref="IActionDescriptor"/> that executes the action by using reflection to invoke the method.
    /// </summary>
    public class DelegateActionDescriptor : IActionDescriptor
    {
        private readonly MethodInfo method;
        private readonly IController controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateActionDescriptor"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="controller">The controller.</param>
        public DelegateActionDescriptor(MethodInfo method, IController controller)
        {
            this.method = method;
            this.controller = controller;
        }

        /// <summary>
        /// Gets the method that will be invoked.
        /// </summary>
        /// <value>The method.</value>
        public MethodInfo Method
        {
            get { return method; }
        }

        /// <summary>
        /// Executes the action on the controller using the parameters and model binders in the current request.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="modelBinders">The model binders.</param>
        /// <returns>
        /// The <see cref="ActionResult"/> returned by the controller action.
        /// </returns>
        public ActionResult Execute(ControllerContext controllerContext, ModelBinderDictionary modelBinders)
        {
            var arguments = new List<object>();
            foreach (var parameterInfo in method.GetParameters())
            {
                var bindingContext = new ModelBindingContext(parameterInfo.Name, Method, parameterInfo.ParameterType, controllerContext.Request.RouteValues);
                var binder = modelBinders.GetBinder(parameterInfo.ParameterType);
                var argument = binder.BindModel(controllerContext.Request, bindingContext);
                arguments.Add(argument);
            }

            try
            {
                var wrapper = DelegateInvoker.CreateInvoker(controller, method);
                return (ActionResult) wrapper.Call(arguments.ToArray());
            }
            catch (Exception ex)
            {
                TraceSources.MagellanSource.TraceError(ex, "The action '{0}' on controller '{1} threw an exception.", controllerContext.ActionName, controllerContext.ActionName);
                throw;
            }
        }
    }
}