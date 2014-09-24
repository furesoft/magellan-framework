using System.Collections.Generic;
using Magellan.ComponentModel;
using Magellan.Diagnostics;
using Magellan.Exceptions;

namespace Magellan.Framework
{
    /// <summary>
    /// Tracks a list of <see cref="IViewEngine">view engines</see> and makes it easier to search them for a view.
    /// </summary>
    public class ViewEngineCollection : Set<IViewEngine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewEngineCollection"/> class.
        /// </summary>
        /// <param name="viewEngines">The view engines.</param>
        public ViewEngineCollection(params IViewEngine[] viewEngines)
        {
            AddRange(viewEngines);
        }

        /// <summary>
        /// Finds the view.
        /// </summary>
        /// <param name="navigationRequest">The navigation request.</param>
        /// <param name="options">The options.</param>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        public ViewEngineResult FindView(ControllerContext navigationRequest, ViewResultOptions options, string view)
        {
            if (Count == 0)
            {
                throw new NavigationConfigurationException("No view engines have been registered with the ViewEngines collection.");
            }

            var searchLocations = new List<string>();
            foreach (var viewEngine in this)
            {
                TraceSources.MagellanSource.TraceVerbose("The ViewEngineCollection is consulting the view engine '{0}' for the view '{1}'.", viewEngine.GetType().FullName, view);
                var result = viewEngine.FindView(navigationRequest, options, view);
                if (result == null)
                {
                    continue;
                }
                if (result.Success)
                {
                    return result;
                }

                searchLocations.AddRange(result.SearchLocations);
            }

            return new ViewEngineResult(false, searchLocations);
        }
    }
}