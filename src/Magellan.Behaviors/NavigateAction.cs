using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using Magellan.Exceptions;
using Magellan.Routing;
using Magellan.Views;

namespace Magellan.Behaviors
{
    /// <summary>
    /// A Blend Trigger Action that makes it easy to navigate using Magellan routes.
    /// </summary>
    public class NavigateAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// Dependency property for the <see cref="Parameters"/> property.
        /// </summary>
        public static readonly DependencyProperty ParametersProperty = DependencyProperty.Register("Parameters", typeof(ParameterCollection), typeof(NavigateAction), new UIPropertyMetadata(null));

        /// <summary>
        /// Dependency property for the <see cref="NavigationService"/> property.
        /// </summary>
        public static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.Register("NavigationService", typeof(NavigationService), typeof(NavigateAction), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigateAction"/> class.
        /// </summary>
        public NavigateAction()
        {
            Parameters = new ParameterCollection();
        }

        /// <summary>
        /// Gets or sets the navigation service in which navigation will take place. By default this will 
        /// be inherited from the current page if available.
        /// </summary>
        /// <value>The navigation service.</value>
        [Category("Navigation")]
        [Description("The navigation service in which navigation will take place. By default this will be inherited from the current page if available.")]
        public NavigationService NavigationService
        {
            get { return (NavigationService)GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }

        /// <summary>
        /// Gets or sets an optional list of additional navigation parameters that will be passed to the controller.
        /// </summary>
        /// <value>The parameters.</value>
        [Category("Navigation")]
        [Description("An optional list of additional navigation parameters that will be passed to the controller.")]
        public ParameterCollection Parameters
        {
            get { return (ParameterCollection)GetValue(ParametersProperty); }
            set { SetValue(ParametersProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            Navigate();
        }

        private void Navigate()
        {
            var parameters = Parameters.ToDictionary(x => x.ParameterName, x => x.Value);

            var navigator = NavigationProperties.GetNavigator(AssociatedObject);
            if (navigator == null)
            {
                throw new ImpossibleNavigationRequestException("The Navigator for this UI element is not avaialable. Please ensure the current view has been loaded into a frame, or that the NavigationProperties.Navigator attached property has been set.");
            }

            var request = new RouteValueDictionary(parameters);
            PrepareRequest(request);
            navigator.ProcessRequest(new NavigationRequest(request));
        }

        /// <summary>
        /// Prepares the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected virtual void PrepareRequest(RouteValueDictionary request)
        {
        }
    }
}