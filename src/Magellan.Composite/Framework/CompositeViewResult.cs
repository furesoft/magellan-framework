using Magellan.Framework;
using Microsoft.Practices.Prism.Regions;

namespace Magellan.Composite.Framework
{
    /// <summary>
    /// Represents a view result for a Composite WPF (Prism) view. The view can be any UserControl.
    /// </summary>
    public class CompositeViewResult : ViewResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeViewResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model that will be bound to the view.</param>
        /// <param name="viewEngines">The view engines.</param>
        public CompositeViewResult(string viewName, object model, ViewEngineCollection viewEngines)
            : base(viewName, model, viewEngines)
        {
            Options.SetViewType("CompositeView");
        }

        /// <summary>
        /// Sets the region that the view will be shown in.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>The current view result.</returns>
        public CompositeViewResult InRegion(string regionName)
        {
            Options.SetRegionName(regionName);
            return this;
        }

        /// <summary>
        /// Sets the region that the view will be shown in.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="regionManager">The region manager.</param>
        /// <returns>The current view result.</returns>
        public CompositeViewResult InRegion(string regionName, IRegionManager regionManager)
        {
            Options.SetRegionName(regionName);
            Options.SetRegionManager(regionManager);
            return this;
        }

        /// <summary>
        /// Sets the region that the view will be shown in.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns>The current view result.</returns>
        public CompositeViewResult InRegion(IRegion region)
        {
            Options.SetRegion(region);
            return this;
        }
    }
}