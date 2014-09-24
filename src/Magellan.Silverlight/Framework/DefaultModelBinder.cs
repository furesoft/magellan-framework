using System;
using System.Linq;
using Magellan.Diagnostics;
using System.Globalization;

namespace Magellan.Framework
{
    /// <summary>
    /// The default implementation of <see cref="IModelBinder"/> that simply maps navigation parameters to 
    /// method parameters by name.
    /// </summary>
    public class DefaultModelBinder : IModelBinder
    {
        /// <summary>
        /// Maps a navigation parameters to target action parameter.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The value that will be supplied to the action.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var requestParameter = bindingContext.RouteValues.FirstOrDefault(x => string.Equals(x.Key, bindingContext.TargetParameterName, StringComparison.InvariantCultureIgnoreCase));
            if (requestParameter.Key == null)
            {
                TraceSources.MagellanSource.TraceError("DefaultModelBinder could not find a parameter '{0}' for the action method '{1}' in the list of navigation parameters.", controllerContext.ActionName, bindingContext.TargetParameterName);
                throw new ArgumentException(string.Format("The action '{0}' on controller '{1}' requires a parameter named '{2}', which was not supplied.",
                    controllerContext.ActionName,
                    controllerContext.Controller.GetType().FullName,
                    bindingContext.TargetParameterName));
            }

            var source = requestParameter.Value;
            if (source == null)
            {
                TraceSources.MagellanSource.TraceWarning("The parameter '{0}' which was supplied to action '{1}' was null, but the parameter is declared as type '{2}', which is a value type. DefaultModelBinder is creating a new default instance of the type instead.",
                    bindingContext.TargetParameterName,
                    controllerContext.ActionName,
                    bindingContext.TargetParameterType
                    );

                return bindingContext.TargetParameterType.IsValueType
                    ? Activator.CreateInstance(bindingContext.TargetParameterType)
                    : null;
            }

            if (bindingContext.TargetParameterType.IsAssignableFrom(source.GetType()))
            {
                return source;
            }
            return Convert.ChangeType(source, bindingContext.TargetParameterType, CultureInfo.InvariantCulture);
        }
    }
}