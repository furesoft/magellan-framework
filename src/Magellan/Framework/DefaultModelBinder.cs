using System;
using System.Linq;
using Magellan.Diagnostics;
using Magellan.Exceptions;
using Magellan.Routing;
using Magellan.Utilities;

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
        /// <param name="request">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The value that will be supplied to the action.
        /// </returns>
        public object BindModel(ResolvedNavigationRequest request, ModelBindingContext bindingContext)
        {
            var requestParameter = bindingContext.RouteValues.FirstOrDefault(x => string.Equals(x.Key, bindingContext.TargetParameterName, StringComparison.InvariantCultureIgnoreCase));
            if (requestParameter.Key == null)
            {
                TraceSources.MagellanSource.TraceError("DefaultModelBinder could not find a parameter '{0}' for the method '{1}' in the list of navigation parameters.", request.RouteValues, bindingContext.TargetParameterName);
                throw new ModelBindingException(bindingContext, request);
            }


            var source = requestParameter.Value;
            var targetType = bindingContext.TargetParameterType;

            return AmazingConverter.Convert(source, targetType);
        }
    }
}