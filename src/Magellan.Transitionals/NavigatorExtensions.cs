using Magellan.Routing;
using Magellan.Utilities;

namespace Magellan.Transitionals
{
    /// <summary>
    /// Extension methods for the <see cref="INavigator"/> interface that make it easy to use transitions.
    /// </summary>
    public static class NavigatorExtensions
    {
        /// <summary>
        /// Navigates to the specified controller and action using a transition.
        /// </summary>
        /// <param name="navigator">The navigator that will perform the navigation.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="transition">The transition.</param>
        public static void NavigateWithTransition(this INavigator navigator, string controller, string action, string transition)
        {
            Guard.ArgumentNotNull(navigator, "navigator");
            var request = new RouteValueDictionary();
            request["controller"] = controller;
            request["action"] = action;
            request["Transition"] = transition;
            navigator.Navigate(request);
        }

        /// <summary>
        /// Navigates to the specified controller and action, passing the parameters dictionary to the action, using a transiton.
        /// </summary>
        /// <param name="navigator">The navigator that will perform the navigation.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="transition">The transition.</param>
        /// <param name="parameters">The parameters.</param>
        public static void NavigateWithTransition(this INavigator navigator, string controller, string action, string transition, object parameters)
        {
            Guard.ArgumentNotNull(navigator, "navigator");
            var request = new RouteValueDictionary(parameters);
            request["controller"] = controller;
            request["action"] = action;
            request["Transition"] = transition;
            navigator.Navigate(request);
        }
    }
}
