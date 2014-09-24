using System.Diagnostics;
using Magellan.Routing;
using Magellan.Utilities;

namespace Magellan.Framework
{
    /// <summary>
    /// This is the most common base class for Magellan controllers.
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
        protected virtual DoNothingResult Cancel()
        {
            return new DoNothingResult();
        }

        /// <summary>
        /// This method can be used for cancelling the current request; in effect, it is a 'no-op' result.
        /// </summary>
        /// <returns>A <see cref="DoNothingResult"/>.</returns>
        protected virtual DoNothingResult DoNothing()
        {
            return new DoNothingResult();
        }

        /// <summary>
        /// Returns to the previous page in the navigation journal.
        /// </summary>
        /// <returns>A <see cref="BackResult"/>.</returns>
        protected BackResult Back()
        {
            return Back(false);
        }

        /// <summary>
        /// Returns to the previous page in the navigation journal.
        /// </summary>
        /// <param name="removeFromJournal">if set to <c>true</c> the page will also be removed from the navigation journal (so 'forward' won't be enabled).</param>
        /// <returns>A <see cref="BackResult"/>.</returns>
        protected virtual BackResult Back(bool removeFromJournal)
        {
            return new BackResult(removeFromJournal);
        }

        /// <summary>
        /// Navigates using the current navigation service to a page with the same name as the current 
        /// action. For example, if the current action is "ShowCustomer", it will try to resolve and navigate
        /// to a WPF page named ShowCustomer, ShowCustomerPage, ShowCustomerView, and other combinations 
        /// (see the <see cref="PageViewEngine"/> for details).
        /// </summary>
        /// <returns>A <see cref="PageResult"/>.</returns>
        protected PageResult Page()
        {
            return Page(null, null);
        }

        /// <summary>
        /// Navigates using the current navigation service to a page with the specified name. For example,
        /// if the page name is "CustomerDetails", it will try to resolve and navigate to a WPF page named
        /// CustomerDetails, CustomerDetailsPage, CustomerDetailsView, and other combinations (see the
        /// <see cref="PageViewEngine"/> for details).
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for 
        /// example, a <paramref name="viewName"/> of "Add" will match "AddPage", "AddView", and so on.
        /// </param>
        /// <returns>A <see cref="PageResult"/>.</returns>
        protected PageResult Page(string viewName)
        {
            return Page(viewName, null);
        }

        /// <summary>
        /// Navigates using the current navigation service to a page with the same name as the current
        /// action. For example, if the current action is "ShowCustomer", it will try to resolve and navigate
        /// to a WPF page named ShowCustomer, ShowCustomerPage, ShowCustomerView, and other combinations
        /// (see the <see cref="PageViewEngine"/> for details).
        /// </summary>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="PageResult"/>.</returns>
        protected PageResult Page(object model)
        {
            return Page(null, model);
        }

        /// <summary>
        /// Navigates using the current navigation service to a page with the specified name. For example,
        /// if the page name is "CustomerDetails", it will try to resolve and navigate to a WPF page named
        /// CustomerDetails, CustomerDetailsPage, CustomerDetailsView, and other combinations (see the
        /// <see cref="PageViewEngine"/> for details).
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for 
        /// example, a <paramref name="viewName"/> of "Add" will match "AddPage", "AddView", and so on.
        /// </param>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="PageResult"/>.</returns>
        protected virtual PageResult Page(string viewName, object model)
        {
            return new PageResult(viewName, model, ViewEngines);
        }

        /// <summary>
        /// Resolves and shows a window with a name matching the current action. For example, if the current
        /// action is "ShowCustomer", it will try to resolve a WPF Window object named ShowCustomer,
        /// ShowCustomerWindow, ShowCustomerView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <returns>A <see cref="WindowResult"/>.</returns>
        protected WindowResult Window()
        {
            return Window(null, null);
        }

        /// <summary>
        /// Resolves and shows a window with a name matching the specified view name. For example, if the
        /// current action is "CustomerDetails", it will try to resolve a WPF Window object named
        /// CustomerDetails, CustomerDetailsWindow, CustomerDetailsView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for 
        /// example, a <paramref name="viewName"/> of "Add" will match "AddWindow", "AddView", and so on.
        /// </param>
        /// <returns>A <see cref="WindowResult"/>.</returns>
        protected WindowResult Window(string viewName)
        {
            return Window(viewName, null);
        }

        /// <summary>
        /// Resolves and shows a window with a name matching the specified view name. For example, if the
        /// current action is "CustomerDetails", it will try to resolve a WPF Window object named
        /// CustomerDetails, CustomerDetailsWindow, CustomerDetailsView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="WindowResult"/>.</returns>
        protected WindowResult Window(object model)
        {
            return Window(null, model);
        }

        /// <summary>
        /// Resolves and shows a window with a name matching the specified view name. For example, if the
        /// current action is "CustomerDetails", it will try to resolve a WPF Window object named
        /// CustomerDetails, CustomerDetailsWindow, CustomerDetailsView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for 
        /// example, a <paramref name="viewName"/> of "Add" will match "AddWindow", "AddView", and so on.
        /// </param>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="WindowResult"/>.</returns>
        protected virtual WindowResult Window(string viewName, object model)
        {
            return new WindowResult(viewName, model, ViewEngines);
        }

        /// <summary>
        /// Resolves and shows a dialog with a name matching the specified view name. For example, if the
        /// current action is "CustomerDetails", it will try to resolve a WPF Window object named
        /// CustomerDetails, CustomerDetailsDialog, CustomerDetailsView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <returns>A <see cref="DialogResult"/>.</returns>
        protected DialogResult Dialog()
        {
            return Dialog(null, null);
        }

        /// <summary>
        /// Resolves and shows a dialog with a name matching the specified view name. For example, if the
        /// current action is "CustomerDetails", it will try to resolve a WPF Window object named
        /// CustomerDetails, CustomerDetailsDialog, CustomerDetailsView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for 
        /// example, a <paramref name="viewName"/> of "Add" will match "AddDialog", "AddView", and so on.
        /// </param>
        /// <returns>A <see cref="DialogResult"/>.</returns>
        protected DialogResult Dialog(string viewName)
        {
            return Dialog(viewName, null);
        }

        /// <summary>
        /// Resolves and shows a dialog with a name matching the specified view name. For example, if the
        /// current action is "CustomerDetails", it will try to resolve a WPF Window object named
        /// CustomerDetails, CustomerDetailsDialog, CustomerDetailsView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="DialogResult"/>.</returns>
        protected DialogResult Dialog(object model)
        {
            return Dialog(null, model);
        }

        /// <summary>
        /// Resolves and shows a dialog with a name matching the specified view name. For example, if the
        /// current action is "CustomerDetails", it will try to resolve a WPF Window object named
        /// CustomerDetails, CustomerDetailsDialog, CustomerDetailsView, and other combinations (see the
        /// <see cref="WindowViewEngine"/> for details).
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for 
        /// example, a <paramref name="viewName"/> of "Add" will match "AddDialog", "AddView", and so on.
        /// </param>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="DialogResult"/>.</returns>
        protected virtual DialogResult Dialog(string viewName, object model)
        {
            return new DialogResult(viewName, model, ViewEngines);
        }

        /// <summary>
        /// Redirects to an alternative action.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>A <see cref="RedirectResult"/>.</returns>
        protected RedirectResult Redirect(string actionName)
        {
            var controllerName = ControllerContext == null ? "" : ControllerContext.ControllerName;
            return Redirect(controllerName, actionName);
        }

        /// <summary>
        /// Redirects to an alternative action.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionParameters">The action parameters.</param>
        /// <returns>A <see cref="RedirectResult"/>.</returns>
        protected RedirectResult Redirect(string actionName, object actionParameters)
        {
            var controllerName = ControllerContext == null ? "" : ControllerContext.ControllerName;
            return Redirect(controllerName, actionName, actionParameters);
        }

        /// <summary>
        /// Redirects to an alternative action on another controller.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>A <see cref="RedirectResult"/>.</returns>
        protected RedirectResult Redirect(string controllerName, string actionName)
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
        protected RedirectResult Redirect(string controllerName, string actionName, object actionParameters)
        {
            var request = new RouteValueDictionary(actionParameters);
            request["controller"] = controllerName;
            request["action"] = actionName;
            return Redirect(request);
        }

        /// <summary>
        /// Redirects to a different navigation request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected virtual RedirectResult Redirect(RouteValueDictionary request)
        {
            Guard.ArgumentNotNull(request, "request");
            return new RedirectResult(request);
        }

        /// <summary>
        /// Starts a new process without waiting for it to end.
        /// </summary>
        /// <param name="processName">Full file path or name of the process.</param>
        /// <returns></returns>
        protected StartProcessResult StartProcess(string processName)
        {
            return StartProcess(new ProcessStartInfo(processName), false);
        }

        /// <summary>
        /// Starts a new process without waiting for it to end.
        /// </summary>
        /// <param name="processName">Full file path or name of the process.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        protected StartProcessResult StartProcess(string processName, string arguments)
        {
            return StartProcess(new ProcessStartInfo(processName, arguments), false);
        }

        /// <summary>
        /// Starts a new process without waiting for it to end.
        /// </summary>
        /// <param name="processName">Full file path or name of the process.</param>
        /// <param name="waitForExit">if set to <c>true</c> the result will wait for the process to exit.</param>
        /// <returns></returns>
        protected StartProcessResult StartProcess(string processName, bool waitForExit)
        {
            return StartProcess(new ProcessStartInfo(processName), waitForExit);
        }

        /// <summary>
        /// Starts a new process without waiting for it to end.
        /// </summary>
        /// <param name="processName">Full file path or name of the process.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="waitForExit">if set to <c>true</c> the result will wait for the process to exit.</param>
        /// <returns></returns>
        protected StartProcessResult StartProcess(string processName, string arguments, bool waitForExit)
        {
            return StartProcess(new ProcessStartInfo(processName, arguments), waitForExit);
        }

        /// <summary>
        /// Starts a new process without waiting for it to end.
        /// </summary>
        /// <param name="startInfo">The process start information.</param>
        /// <returns></returns>
        protected StartProcessResult StartProcess(ProcessStartInfo startInfo)
        {
            return StartProcess(startInfo, false);
        }

        /// <summary>
        /// Starts a new process without waiting for it to end.
        /// </summary>
        /// <param name="startInfo">The process start information.</param>
        /// <param name="waitForExit">if set to <c>true</c> the result will wait for the process to exit.</param>
        /// <returns></returns>
        protected virtual StartProcessResult StartProcess(ProcessStartInfo startInfo, bool waitForExit)
        {
            Guard.ArgumentNotNull(startInfo, "startInfo");
            return new StartProcessResult(startInfo, waitForExit);
        }
    }
}