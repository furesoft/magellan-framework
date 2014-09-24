using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Magellan.Diagnostics;
using Magellan.Utilities;

namespace Magellan.Framework
{
    /// <summary>
    /// The standard implementation of <see cref="ActionInvoker"/> that uses reflection to invoke actions and locates filters based on attributes.
    /// </summary>
    public class DefaultActionInvoker : ActionInvoker
    {
        /// <summary>
        /// When implemented in a derived class, resolves an action by the given name on the controller.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>
        /// An implementation of <see cref="IActionDescriptor"/>, or null if no action could be found.
        /// </returns>
        protected override IActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
        {
            Guard.ArgumentNotNull(controllerContext, "controllerContext");
            Guard.ArgumentNotNullOrEmpty(actionName, "actionName");

            var method = SelectBestActionMethod(controllerContext, actionName);
            return method == null ? null : new DelegateActionDescriptor(method, controllerContext.Controller);
        }

        /// <summary>
        /// When implemented in a derived class, finds all pre and post <see cref="IActionFilter">action filters</see> that apply to the given action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        protected override IEnumerable<IActionFilter> FindActionFilters(IActionDescriptor action, ControllerContext controllerContext, string actionName)
        {
            Guard.ArgumentNotNull(action, "action");
            Guard.ArgumentNotNull(controllerContext, "controllerContext");
            Guard.ArgumentNotNullOrEmpty(actionName, "actionName");
            Guard.ArgumentIsOfType(action, typeof(DelegateActionDescriptor), "action");

            var delegateDescriptor = (DelegateActionDescriptor)action;
            var attributeSources = new ICustomAttributeProvider[] { delegateDescriptor.Method, controllerContext.Controller.GetType() };
            var filters = attributeSources.SelectMany(x => x.GetCustomAttributes(true)).Select(x => x as IActionFilter).Where(x => x != null).ToList();
            TraceSources.MagellanSource.TraceVerbose("DefaultActionInvoker found the following action filters for action '{0}': '{1}'.", actionName,
                string.Join(", ", filters.Select(x => x.GetType().Name).ToArray())
                ); 
            return filters;
        }

        /// <summary>
        /// When implemented in a derived class, finds all pre and post <see cref="IResultFilter">result filters</see> that apply to the given action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        protected override IEnumerable<IResultFilter> FindResultFilters(IActionDescriptor action, ControllerContext controllerContext, string actionName)
        {
            Guard.ArgumentNotNull(action, "action");
            Guard.ArgumentNotNull(controllerContext, "controllerContext");
            Guard.ArgumentNotNullOrEmpty(actionName, "actionName");
            Guard.ArgumentIsOfType(action, typeof(DelegateActionDescriptor), "action");

            var delegateDescriptor = (DelegateActionDescriptor)action;
            var attributeSources = new ICustomAttributeProvider[] { delegateDescriptor.Method, controllerContext.Controller.GetType() };
            var filters = attributeSources.SelectMany(x => x.GetCustomAttributes(true)).Select(x => x as IResultFilter).Where(x => x != null).ToList();
            TraceSources.MagellanSource.TraceVerbose("DefaultActionInvoker found the following result filters for action '{0}': '{1}'.", actionName, 
                string.Join(", ", filters.Select(x => x.GetType().Name).ToArray())
                );
            return filters;
        }

        private static bool HasMatchingSignature(MethodInfo method, string actionName)
        {
            return string.Equals(method.Name, actionName, StringComparison.InvariantCultureIgnoreCase)
                   && typeof(ActionResult).IsAssignableFrom(method.ReturnType);
        }

        private static MethodInfo SelectBestActionMethod(ControllerContext controllerContext, string actionName)
        {
            var type = controllerContext.Controller.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var candidateMethods = methods.Where(x => HasMatchingSignature(x, actionName)).ToList();
            if (candidateMethods.Count == 1)
            {
                var result = candidateMethods.First();
                TraceSources.MagellanSource.TraceVerbose("DefaultActionInvoker found the action '{0}' as method '{1}'", actionName, result);
                return result;
            }
            if (candidateMethods.Count > 1)
            {
                var result = candidateMethods
                    .OrderBy(x => x.GetParameters().Length)
                    .LastOrDefault(
                        candidate => candidate.GetParameters().Select(x => x.Name)
                            .HasSameItemsRegardlessOfSortOrder(controllerContext.Request.RouteValues.Select(x => x.Key))
                    );
                TraceSources.MagellanSource.TraceVerbose("DefaultActionInvoker found multiple methods for the action '{0}'. The method selected was '{1}'.", actionName, result);
                return result;
            }
            TraceSources.MagellanSource.TraceError("DefaultActionInvoker could not find a method for the action '{0}'.", actionName);
            return null;
        }
    }
}
