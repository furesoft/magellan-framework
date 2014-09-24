using System.Windows;
using Magellan.Routing;

namespace Magellan.Views
{
    /// <summary>
    /// Contains dependency properties useful for navigation.
    /// </summary>
    public static class NavigationProperties
    {
#if SILVERLIGHT
        /// <summary>
        /// A dependency property for storing the <see cref="INavigator"/> associated with a UI element. This 
        /// property is marked for inheritance.
        /// </summary>
        public static readonly DependencyProperty NavigatorProperty = DependencyProperty.RegisterAttached("Navigator", typeof(INavigator), typeof(NavigationProperties), new PropertyMetadata(null));
#else
        /// <summary>
        /// A dependency property for storing the <see cref="INavigator"/> associated with a UI element. This 
        /// property is marked for inheritance.
        /// </summary>
        public static readonly DependencyProperty NavigatorProperty = DependencyProperty.RegisterAttached("Navigator", typeof(INavigator), typeof(NavigationProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// A dependency property for storing the current navigation request.
        /// </summary>
        public static readonly DependencyProperty CurrentRequestProperty = DependencyProperty.RegisterAttached("CurrentRequest", typeof(ResolvedNavigationRequest), typeof(NavigationProperties), new UIPropertyMetadata(null));

#endif

        /// <summary>
        /// Gets the navigator.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static INavigator GetNavigator(DependencyObject obj)
        {
            return (INavigator)obj.GetValue(NavigatorProperty);
        }

        /// <summary>
        /// Sets the navigator.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetNavigator(DependencyObject obj, INavigator value)
        {
            obj.SetValue(NavigatorProperty, value);
        }

        /// <summary>
        /// Gets the current request.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static ResolvedNavigationRequest GetCurrentRequest(DependencyObject obj)
        {
            return (ResolvedNavigationRequest)obj.GetValue(CurrentRequestProperty);
        }

        /// <summary>
        /// Sets the current request.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetCurrentRequest(DependencyObject obj, ResolvedNavigationRequest value)
        {
            obj.SetValue(CurrentRequestProperty, value);
        }
    }
}
