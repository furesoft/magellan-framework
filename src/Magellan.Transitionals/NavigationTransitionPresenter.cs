using System.Windows;
using System.Windows.Navigation;
using Magellan.Utilities;
using Transitionals.Controls;

namespace Magellan.Transitionals
{
    /// <summary>
    /// This control is added to the ContentTemplates of Frames as an alternative to the content presenter. It applies 
    /// transitions based on navigation requests. 
    /// </summary>
    public class NavigationTransitionPresenter : TransitionElement
    {
        /// <summary>
        /// Dependency property for the <see cref="TransitionRegistry"/> property.
        /// </summary>
        public static readonly DependencyProperty TransitionRegistryProperty = DependencyProperty.Register("TransitionRegistry", typeof(NavigationTransitionRegistry), typeof(NavigationTransitionPresenter), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationTransitionPresenter"/> class.
        /// </summary>
        public NavigationTransitionPresenter()
        {
            Loaded += ControlLoaded;
            TransitionRegistry = NavigationTransitions.Table;
        }

        /// <summary>
        /// Gets or sets the registry of transition animations.
        /// </summary>
        /// <value>The transition registry.</value>
        public NavigationTransitionRegistry TransitionRegistry
        {
            get { return (NavigationTransitionRegistry)GetValue(TransitionRegistryProperty); }
            set { SetValue(TransitionRegistryProperty, value); }
        }

        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            var service = NavigationService.GetNavigationService(this);

            if (TransitionSelector == null)
            {
                TransitionSelector = new NavigationTransitionSelector(service, TransitionRegistry);
            }
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement"/> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IsTransitioningProperty)
            {
                this.ToggleSuppressInput(IsTransitioning);
            }
            
            base.OnPropertyChanged(e);
        }
    }
}