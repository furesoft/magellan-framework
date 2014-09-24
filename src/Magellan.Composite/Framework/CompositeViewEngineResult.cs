using System;
using System.Windows;
using Magellan.Diagnostics;
using Magellan.Framework;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Magellan.Composite.Framework
{
    /// <summary>
    /// The result of the <see cref="CompositeViewEngine"/>.
    /// </summary>
    public class CompositeViewEngineResult : FrameworkElementViewEngineResult
    {
        private readonly Type type;
        private readonly IViewActivator viewActivator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeViewEngineResult"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The options.</param>
        /// <param name="type">The type.</param>
        /// <param name="viewActivator">The view activator.</param>
        public CompositeViewEngineResult(ControllerContext controllerContext, ViewResultOptions options, Type type, IViewActivator viewActivator)
            : base(controllerContext, options)
        {
            this.type = type;
            this.viewActivator = viewActivator;
        }

        /// <summary>
        /// Renders this view.
        /// </summary>
        public override void Render()
        {
            var dispatcher = ControllerContext.Request.Navigator.Dispatcher;
            dispatcher.Dispatch(
                delegate
                {
                    TraceSources.MagellanSource.TraceInformation("CompositeViewResult is rendering the view '{0}'.", type.FullName);

                    // Create the view
                    var instance = (FrameworkElement)viewActivator.Instantiate(type);
                    ViewInitializer.Prepare(instance, Model, ControllerContext.Request);

                    // Figure out which region to use
                    var region = Options.GetRegion();
                    if (region == null)
                    {
                        var regionName = Options.GetRegionName();
                        if (regionName == null)
                        {
                            throw new RegionNotProvidedException("No region nor region name was specified. Please use the InRegion methods on the CompositeView return value to indicate the region the view should be added to.");
                        }

                        var regionManager = Options.GetRegionManager() ?? ServiceLocator.Current.GetInstance<IRegionManager>();
                        region = regionManager.Regions[regionName];
                    }

                    // Show the view
                    region.Add(instance);
                });
        }
    }
}