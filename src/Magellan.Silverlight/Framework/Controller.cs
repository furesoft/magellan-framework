using System;
using Magellan.Routing;
using Magellan.Utilities;

namespace Magellan.Framework
{
    /// <summary>
    /// This is the most common base class for Magellan Silverlight controllers.
    /// </summary>
    public abstract class Controller : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        protected Controller()
        {
        }

        /// <summary>
        /// This method can be used for cancelling the current request; in effect, it is a 'no-op' result.
        /// </summary>
        /// <returns>A <see cref="DoNothingResult"/>.</returns>
        public virtual DoNothingResult Cancel()
        {
            return new DoNothingResult();
        }

        /// <summary>
        /// This method can be used for cancelling the current request; in effect, it is a 'no-op' result.
        /// </summary>
        /// <returns>A <see cref="DoNothingResult"/>.</returns>
        public virtual DoNothingResult DoNothing()
        {
            return new DoNothingResult();
        }

        /// <summary>
        /// Returns to the previous page in the navigation journal.
        /// </summary>
        /// <returns>A <see cref="BackResult"/>.</returns>
        public virtual BackResult Back()
        {
            return new BackResult();
        }

        /// <summary>
        /// Navigates using the current navigation service to a page with the same name as the current 
        /// action. For example, if the current action is "ShowCustomer", it will try to resolve and navigate
        /// to a WPF page named ShowCustomer, ShowCustomerPage, ShowCustomerView, and other combinations 
        /// (see the <see cref="PageViewEngine"/> for details).
        /// </summary>
        /// <returns>A <see cref="PageResult"/>.</returns>
        public PageResult Page()
        {
            return Page(null, null);
        }

        /// <summary>
        /// Navigates using the current navigation service to a page with the specified name. For example,
        /// if the page name is "CustomerDetails", it will try to resolve and navigate to a WPF page named
        /// CustomerDetails, CustomerDetailsPage, CustomerDetailsView, and other combinations (see the
        /// <see cref="PageViewEngine"/> for details).
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns>A <see cref="PageResult"/>.</returns>
        public virtual PageResult Page(string viewName, object model)
        {
            var result = new PageResult(viewName, model, ViewEngines);
            return result;
        }

        /// <summary>
        /// Shows a child window with the same name as the current action.
        /// </summary>
        /// <returns>A <see cref="ChildWindowResult"/> representing the command to show the child window.
        /// </returns>
        public ChildWindowResult ChildWindow()
        {
            return ChildWindow(null);
        }

        /// <summary>
        /// Shows a child window.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>A <see cref="ChildWindowResult"/> representing the command to show the child window.
        /// </returns>
        public virtual ChildWindowResult ChildWindow(string viewName)
        {
            var result = new ChildWindowResult(viewName, ViewEngines);
            PrepareViewResult(result);
            return result;
        }

        /// <summary>
        /// Redirects to an alternative action.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>A <see cref="RedirectResult"/>.</returns>
        public RedirectResult Redirect(string actionName)
        {
            return Redirect(Request.Controller, actionName);
        }

        /// <summary>
        /// Redirects to an alternative action.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionParameters">The action parameters.</param>
        /// <returns>A <see cref="RedirectResult"/>.</returns>
        public RedirectResult Redirect(string actionName, params Func<string, object>[] actionParameters)
        {
            return Redirect(Request.Controller, actionName, actionParameters);
        }

        /// <summary>
        /// Redirects to an alternative action on another controller.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>A <see cref="RedirectResult"/>.</returns>
        public RedirectResult Redirect(string controllerName, string actionName)
        {
            return Redirect(controllerName, actionName, null);
        }

        /// <summary>
        /// Redirects to an alternative action on another controller.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionParameters">The action parameters.</param>
        /// <returns>A <see cref="RedirectResult"/>.</returns>
        public RedirectResult Redirect(string controllerName, string actionName, params Func<string, object>[] actionParameters)
        {
            var request = new NavigationRequest(controllerName, actionName, new RouteValueDictionary(actionParameters));
            return Redirect(request);
        }

        /// <summary>
        /// Redirects to a different navigation request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public virtual RedirectResult Redirect(NavigationRequest request)
        {
            Guard.ArgumentNotNull(request, "request");
            request.ContextParameters.AddRange(Request.ContextParameters);
            return new RedirectResult(request);
        }
    }
}
