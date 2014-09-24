using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects that can aid in the mapping of navigation parameters to action parameters.
    /// </summary>
    public interface IModelBinder
    {
        /// <summary>
        /// Maps a navigation parameters to target action parameter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The value that will be supplied to the action.
        /// </returns>
        object BindModel(ResolvedNavigationRequest request, ModelBindingContext bindingContext);
    }
}
