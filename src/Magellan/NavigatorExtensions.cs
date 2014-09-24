using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Magellan.Exceptions;
using Magellan.Framework;
using Magellan.Routing;

namespace Magellan
{
    /// <summary>
    /// Extension methods for the <see cref="INavigator"/> interface that help with common navigation 
    /// scenarios.
    /// </summary>
    public static class NavigatorExtensions
    {
        /// <summary>
        /// Navigates to the specified URI, resolving the first matching route.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="requestUri">The request URI.</param>
        public static void Navigate(this INavigator navigator, string requestUri)
        {
            navigator.Navigate(
                requestUri.IndexOf(':') <= 0
                ? new Uri(navigator.Scheme + "://" + requestUri)
                : new Uri(requestUri));
        }

        /// <summary>
        /// Navigates to the specified URI, resolving the first matching route.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="requestUri">The request URI.</param>
        public static void Navigate(this INavigator navigator, Uri requestUri)
        {
            navigator.Navigate(requestUri, null);
        }

        /// <summary>
        /// Navigates to the specified URI, resolving the first matching route.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="additionalData">Additional data (like post data) that is not in the URI but is used
        /// for navigation.</param>
        public static void Navigate(this INavigator navigator, Uri requestUri, object additionalData)
        {
            navigator.Navigate(requestUri, new RouteValueDictionary(additionalData));
        }

        /// <summary>
        /// Navigates to the specified URI, resolving the first matching route.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="additionalData">Additional data (like post data) that is not in the URI but is used
        /// for navigation.</param>
        public static void Navigate(this INavigator navigator, Uri requestUri, RouteValueDictionary additionalData)
        {
            navigator.ProcessRequest(new NavigationRequest(requestUri, additionalData));
        }

        /// <summary>
        /// Resolves and navigates to the first route that matches the given set of route values.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="routeValues">The route values. For example, <code>new { controller = "MyController", action = "MyAction" }</code></param>
        public static void Navigate(this INavigator navigator, object routeValues)
        {
            navigator.Navigate(new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// Resolves and navigates to the first route that matches the given set of route values.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="request">The request.</param>
        public static void Navigate(this INavigator navigator, RouteValueDictionary request)
        {
            navigator.ProcessRequest(new NavigationRequest(request));
        }

        /// <summary>
        /// Navigates to the specified controller, following the action and parameters passed below. Note 
        /// that the controller must be registered in the controller factory with the type name minus the 
        /// "Controller" suffix - for example, a controller of type "PatientSearchController" should be 
        /// registered with the name "PatientSearch" for this call to work.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="navigator">The navigator.</param>
        /// <param name="actionSelector">The action selector.</param>
        public static void Navigate<TController>(this INavigator navigator, Expression<Func<TController, ActionResult>> actionSelector)
            where TController : IController
        {
            var routeValues = new RouteValueDictionary();
            var controller = typeof(TController);
            var controllerName = GetControllerNameFromType(controller);
            routeValues.Add("controller", controllerName);

            var body = actionSelector.Body as MethodCallExpression;
            if (body == null)
            {
                throw new ImpossibleNavigationRequestException("The lambda expression used for navigation could not be parsed. The lambda should be a MethodCallExpression, for example: 'x => x.Search(text)'.");
            }

            var method = body.Method;
            var actionName = method.Name;
            routeValues.Add("action", actionName);

            var parameters = method.GetParameters();
            var arguments = body.Arguments;
            for (var i = 0; i < parameters.Length && i < arguments.Count; i++)
            {
                var parameter = parameters[i].Name;
                var argument = arguments[i];
                var lambda = Expression.Lambda<Func<TController, object>>(Expression.Convert(argument, typeof(object)), actionSelector.Parameters.ToList());
                var compiled = lambda.Compile();
                var value = compiled(default(TController));

                routeValues.Add(parameter, value);
            }

			var request = new NavigationRequest(routeValues);
			navigator.ProcessRequest(request);
        }

    	/// <summary>
        /// Resolves and navigates to the first route that matches the route values for the given view model.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="navigator">The navigator.</param>
        public static void Navigate<TViewModel>(this INavigator navigator)
        {
            Navigate<TViewModel>(navigator, (object)null);
        }

        /// <summary>
        /// Resolves and navigates to the first route that matches the route values for the given view model.
        /// </summary>
        /// <typeparam name="TViewModelOrController">The type of the view model.</typeparam>
        /// <param name="navigator">The navigator.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Navigate<TViewModelOrController>(this INavigator navigator, object parameters)
        {
			var routeValues = new RouteValueDictionary(parameters);

			if (typeof(IController).IsAssignableFrom(typeof(TViewModelOrController)))
			{
				routeValues["controller"] = GetControllerNameFromType(typeof(TViewModelOrController));
			}
			else
			{
				routeValues["viewModel"] = typeof(TViewModelOrController).Name.Replace("ViewModel", "");
			}

			var request = new NavigationRequest(routeValues);
			navigator.ProcessRequest(request);
        }

		private static string GetControllerNameFromType(Type controller)
		{
			return controller.Name.Replace("Controller", "");
		}
    }
}
