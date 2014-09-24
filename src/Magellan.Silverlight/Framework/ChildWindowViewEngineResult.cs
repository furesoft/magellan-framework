using System;
using Magellan.Abstractions;
using Magellan.Diagnostics;
using System.Windows.Controls;

namespace Magellan.Framework
{
    /// <summary>
    /// The result from a <see cref="ChildWindowViewEngine"/>.
    /// </summary>
    public class ChildWindowViewEngineResult : FrameworkElementViewEngineResult
    {
        private readonly IViewActivator _viewActivator;
        private readonly Type _type;
        private readonly ViewResultOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildWindowViewEngineResult"/> class.
        /// </summary>
        /// <param name="viewActivator">The view activator.</param>
        /// <param name="type">The type.</param>
        /// <param name="options">The options.</param>
        /// <param name="controllerContext">The controller context.</param>
        public ChildWindowViewEngineResult(IViewActivator viewActivator, Type type, ViewResultOptions options, ControllerContext controllerContext)
            : base(controllerContext, options)
        {
            _viewActivator = viewActivator;
            _type = type;
            _options = options;
        }

        /// <summary>
        /// Gets the ChildWindow that was rendered.
        /// </summary>
        /// <value>The rendered instance.</value>
        public ChildWindow RenderedInstance { get; protected set; }

        /// <summary>
        /// Renders this view.
        /// </summary>
        public override void Render()
        {
            var dispatcher = ControllerContext.Request.Navigator.Dispatcher;

            dispatcher.Dispatch(
                delegate
                {
                    TraceSources.MagellanSource.TraceInformation("The WindowViewEngine is instantiating the window '{0}'.", _type);

                    // Prepare the window
                    RenderedInstance = (ChildWindow)_viewActivator.Instantiate(_type);
                    WireModelToView(RenderedInstance);

                    TraceSources.MagellanSource.TraceVerbose("The ChildWindowViewEngine is rendering the window '{0}' as a dialog.", _type);
                    RenderedInstance.Show();
                });
        }
    }
}