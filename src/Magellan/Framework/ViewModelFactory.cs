using System;
using System.Collections.Generic;
using System.Globalization;
using Magellan.Diagnostics;
using Magellan.Routing;
using Magellan.Utilities;

namespace Magellan.Framework
{
    /// <summary>
    /// A default implementation of <see cref="IViewModelFactory"/> that uses delegates for resolving views and 
    /// view models.
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly Dictionary<string, Func<object>> modelBuilders = new Dictionary<string, Func<object>>();
        private readonly Dictionary<string, Func<object>> viewBuilders = new Dictionary<string, Func<object>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactory"/> class.
        /// </summary>
        public ViewModelFactory() 
        {
        }

        /// <summary>
        /// Registers a view and view model pair by the specified name.
        /// </summary>
        /// <param name="name">The name that will later be used to resolve the view/view model pair.</param>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="viewModelType">Type of the view model.</param>
        public void Register(string name, Func<object> viewType, Func<object> viewModelType)
        {
            Guard.ArgumentNotNull(name, "controllerName");
            Guard.ArgumentNotNull(viewType, "viewBuilder");

            TraceSources.MagellanSource.TraceVerbose("Registering view/view model pair '{0}'", name);

            if (modelBuilders.ContainsKey(name.ToUpper(CultureInfo.InvariantCulture)))
            {
                throw new ArgumentException(string.Format("A view model with the name '{0}' has already been added.", name));
            }
            modelBuilders.Add(name.ToUpper(CultureInfo.InvariantCulture), viewModelType);

            if (viewBuilders.ContainsKey(name.ToUpper(CultureInfo.InvariantCulture)))
            {
                throw new ArgumentException(string.Format("A view with the name '{0}' has already been added.", name));
            }
            viewBuilders.Add(name.ToUpper(CultureInfo.InvariantCulture), viewType);
        }

        /// <summary>
        /// Creates a view and view model to handle the given navigation request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="viewModelName">Name of the view model.</param>
        /// <returns>
        /// An object containing the View/ViewModel pair.
        /// </returns>
        public ViewModelFactoryResult CreateViewModel(ResolvedNavigationRequest request, string viewModelName)
        {
            var name = viewModelName.ToUpper(CultureInfo.InvariantCulture);
            if (!modelBuilders.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("A view model by the name {0} is not registered in this ViewModelFactory.", name));
            }
            
            var model = modelBuilders[name]();
            var view = viewBuilders[name]();

            return new ViewModelFactoryResult(view, model);
        }
    }
}
