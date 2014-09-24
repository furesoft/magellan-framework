using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Magellan.Abstractions;
using Magellan.Diagnostics;

namespace Magellan.Framework
{
    /// <summary>
    /// A <see cref="ViewEngineResult"/> that contains a WPF <see cref="Page"/> that will be navigated to using the <see cref="NavigationService"/> of the incoming request.
    /// </summary>
    public class PageViewEngineResult : FrameworkElementViewEngineResult
    {
        private readonly Type viewType;
        private readonly IViewActivator viewActivator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewEngineResult"/> class.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="options">The options.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewActivator">The view activator.</param>
        public PageViewEngineResult(Type viewType, ViewResultOptions options, ControllerContext controllerContext, IViewActivator viewActivator)
            : base(controllerContext, options)
        {
            this.viewType = viewType;
            this.viewActivator = viewActivator;
        }

        /// <summary>
        /// Gets the type of the view.
        /// </summary>
        /// <value>The type of the view.</value>
        public Type ViewType
        {
            get { return viewType; }
        }

        /// <summary>
        /// Gets or sets the rendered instance of the Page or ContentControl.
        /// </summary>
        /// <value>The rendered instance.</value>
        public FrameworkElement RenderedInstance { get; private set; }

        /// <summary>
        /// Renders this view.
        /// </summary>
        public override void Render()
        {
            var dispatcher = ControllerContext.Request.Navigator.Dispatcher;

            dispatcher.Dispatch(
                delegate
                {
                    TraceSources.MagellanSource.TraceInformation("The PageViewEngine is rendering the page '{0}'.", viewType);

                    // Prepare the page
                    RenderedInstance = (FrameworkElement)viewActivator.Instantiate(viewType);

                    ViewInitializer.Prepare(RenderedInstance, Model, ControllerContext.Request);

                    // Navigate to the page
                    var navigationService = ControllerContext.Request.Navigator;
                    NavigationMonitor.EnsureBound(navigationService, ViewInitializer);
                    navigationService.NavigateDirectToContent(RenderedInstance, ControllerContext.Request);

                    TraceSources.MagellanSource.TraceVerbose("The PageViewEngine has navigated to the page '{0}'.", viewType);

                    // Remove all back entries from the navigation journal if necessary
                    var clearHistory = Options.GetResetHistory();
                    if (clearHistory)
                    {
                        TraceSources.MagellanSource.TraceVerbose("The PageViewEngine is clearing the navigation history.", viewType);
                        navigationService.ResetHistory();
                    }
                });
        }

        internal class NavigationMonitor
        {
            private static readonly DependencyProperty _monitorProperty = DependencyProperty.RegisterAttached("Monitor", typeof(NavigationMonitor), typeof(NavigationMonitor), new PropertyMetadata(null));
            private readonly INavigationService _navigationService;
            private readonly IViewInitializer _viewInitializer;

            public NavigationMonitor(INavigationService navigationService, IViewInitializer viewInitializer)
            {
                _navigationService = navigationService;
                _viewInitializer = viewInitializer;
            }

            public static void EnsureBound(INavigationService navigationService, IViewInitializer viewInitializer)
            {
                var existing = (NavigationMonitor)navigationService.GetValue(_monitorProperty);
                if (existing == null)
                {
                    existing = new NavigationMonitor(navigationService, viewInitializer);
                    navigationService.Navigating += existing.Navigating;
                    navigationService.Navigated += existing.Navigated;
                    navigationService.SetValue(_monitorProperty, existing);
                }
            }

            private void Navigating(object sender, CancelEventArgs e)
            {
                var view = _navigationService.Content;
                if (view == null)
                    return;

                _viewInitializer.NotifyDeactivating(view, e);
                if (!e.Cancel)
                {
                    _viewInitializer.NotifyDeactivated(view);
                }
            }

            private void Navigated(object sender, EventArgs e)
            {
                var view = _navigationService.Content;
                if (view == null)
                    return;

                _viewInitializer.NotifyActivated(view);
            }
        }

        private class NavigationClosure
        {
            private readonly INavigationService _navigationService;
            private readonly object _view;
            private readonly object _viewModel;
            private readonly IViewInitializer _viewInitializer;

            private NavigationClosure(INavigationService navigationService, object view, object viewModel, IViewInitializer viewInitializer)
            {
                _navigationService = navigationService;
                _view = view;
                _viewModel = viewModel;
                _viewInitializer = viewInitializer;
            }

            public static NavigationClosure Bind(INavigationService navigationService, object view, object viewModel, IViewInitializer initializer)
            {
                var closure = new NavigationClosure(navigationService, view, viewModel, initializer);
                navigationService.Navigated += closure.Navigated;
                return closure;
            }

            private void Navigated(object sender, EventArgs e)
            {
                _navigationService.Navigated -= Navigated;
                _navigationService.Navigating -= Navigating;
                _navigationService.Navigating += Navigating;

                _viewInitializer.NotifyActivated(_view);
            }

            private void Navigating(object sender, CancelEventArgs e)
            {
                _viewInitializer.NotifyDeactivating(_view, e);
                if (!e.Cancel)
                {
                    _viewInitializer.NotifyDeactivated(_view);
                    _navigationService.Navigating -= Navigating;
                }
            }
        }
    }
}