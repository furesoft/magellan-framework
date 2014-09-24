using System;
using System.Windows.Controls;
using Magellan.Diagnostics;

namespace Magellan.Framework
{
    /// <summary>
    /// The result from the <see cref="PageViewEngine"/>.
    /// </summary>
    public class PageViewEngineResult : FrameworkElementViewEngineResult
    {
        private readonly IViewActivator _viewActivator;
        private readonly Type _type;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewEngineResult"/> class.
        /// </summary>
        /// <param name="viewActivator">The view activator.</param>
        /// <param name="type">The type.</param>
        /// <param name="options">The options.</param>
        /// <param name="controllerContext">The controller context.</param>
        public PageViewEngineResult(IViewActivator viewActivator, Type type, ViewResultOptions options, ControllerContext controllerContext) 
            : base(controllerContext, options)
        {
            _viewActivator = viewActivator;
            _type = type;
        }

        public UserControl RenderedInstance { get; private set; }

        /// <summary>
        /// Renders this view.
        /// </summary>
        public override void Render()
        {
            var dispatcher = ControllerContext.Request.Navigator.Dispatcher;

            dispatcher.Dispatch(
                delegate
                {
                    TraceSources.MagellanSource.TraceInformation("The PageViewEngineResult is instantiating the window '{0}'.", _type);

                    // Prepare the window
                    RenderedInstance = (UserControl)_viewActivator.Instantiate(_type);
                    WireModelToView(RenderedInstance);

                    TraceSources.MagellanSource.TraceVerbose("The PageViewEngineResult is rendering the window '{0}' as a dialog.", _type);
                    ControllerContext.Request.Navigator.NavigateDirectToContent(RenderedInstance);
                });
        }
    }
}