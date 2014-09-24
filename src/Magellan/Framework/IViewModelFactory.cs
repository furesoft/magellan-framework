using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects that can be used for resolving a Model-View-ViewModel combination for a 
    /// navigation request.
    /// </summary>
    public interface IViewModelFactory
    {
        /// <summary>
        /// Creates a view and view model to handle the given navigation request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="viewModelName">Name of the view model.</param>
        /// <returns>An object containing the View/ViewModel pair.</returns>
        ViewModelFactoryResult CreateViewModel(ResolvedNavigationRequest request, string viewModelName);
    }
}