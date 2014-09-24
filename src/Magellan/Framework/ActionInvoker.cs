using System;
using Magellan.Events;
using Magellan.Exceptions;
using Magellan.Utilities;
using System.Collections.Generic;
using Magellan.Diagnostics;

namespace Magellan.Framework
{
    /// <summary>
    /// A base class for <see cref="IActionInvoker">action invokers</see>. This class uses the template method pattern to co-ordinate processing a navigation request, 
    /// calling the action filters, and executing the action results. Other classes should inherit from this class to supply details around resolving actions and filters.
    /// </summary>
    public abstract class ActionInvoker : IActionInvoker
    {
        /// <summary>
        /// When implemented in a derived class, resolves an action by the given name on the controller.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>An implementation of <see cref="IActionDescriptor"/>, or null if no action could be found.</returns>
        protected abstract IActionDescriptor FindAction(ControllerContext controllerContext, string actionName);

        /// <summary>
        /// When implemented in a derived class, finds all pre and post <see cref="IActionFilter">action filters</see> that apply to the given action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        protected abstract IEnumerable<IActionFilter> FindActionFilters(IActionDescriptor action, ControllerContext controllerContext, string actionName);

        /// <summary>
        /// When implemented in a derived class, finds all pre and post <see cref="IResultFilter">result filters</see> that apply to the given action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        protected abstract IEnumerable<IResultFilter> FindResultFilters(IActionDescriptor action, ControllerContext controllerContext, string actionName);

        /// <summary>
        /// Executes the action on the specified controller.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="modelBinders">The model binders.</param>
        public virtual void ExecuteAction(ControllerContext controllerContext, string actionName, ModelBinderDictionary modelBinders)
        {
            try
            {
                Guard.ArgumentNotNull(controllerContext, "controllerContext");
                Guard.ArgumentNotNull(modelBinders, "modelBinders");
                Guard.ArgumentNotNullOrEmpty(actionName, "actionName");

                controllerContext.Request.ReportProgress(new ResolvingActionNavigationEvent());
                var actionDescriptor = FindAction(controllerContext, actionName);
                if (actionDescriptor == null)
                {
                    throw new ActionNotFoundException(string.Format("An action by the name '{0}' could not be found on the controller '{1}'. In general, actions should be public instance methods, and should return ActionResult or a derived type. Please ensure the action exists, has a valid signature and the name is spelled correctly.", actionName, controllerContext.Controller.GetType()));
                }

                var actionResult = ExecuteAction(controllerContext, modelBinders, actionName, actionDescriptor);
                ExecuteResult(actionDescriptor, controllerContext, actionName, actionResult);
            }
            finally
            {
                controllerContext.Dispose();
                TraceSources.MagellanSource.TraceInformation("Request completed: '{0}'.", controllerContext.Request);
            }
        }

        private ActionResult ExecuteAction(ControllerContext controllerContext, ModelBinderDictionary modelBinders, string actionName, IActionDescriptor actionDescriptor)
        {
            var filters = FindActionFilters(actionDescriptor, controllerContext, actionName);
            var result = ExecutePreActionFilters(controllerContext, modelBinders, filters);

            // If one of the filters gave us a result, we can short-circuit calling the action and skip ahead
            if (result != null)
                return result;

            controllerContext.Request.ReportProgress(new ExecutingActionNavigationEvent());

            try
            {
                // Invoke the action on the controller
                result = actionDescriptor.Execute(controllerContext, modelBinders);
            }
            catch (Exception ex)
            {
                // Give post-filters a chance to suppress or handle the exception
                var errorFilterResult = ExecutePostActionFilters(controllerContext, result, ex, filters);
                
                if (errorFilterResult.ExceptionHandled || errorFilterResult.Exception == null)
                    return errorFilterResult.Result;

                if (ex == errorFilterResult.Exception)
                    throw;

                throw errorFilterResult.Exception;
            }

            // Give post-filters a chance rewrite the result
            var filterResult = ExecutePostActionFilters(controllerContext, result, null, filters);
            if (filterResult.ExceptionHandled || filterResult.Exception == null)
                return filterResult.Result;

            throw filterResult.Exception;
        }

        private void ExecuteResult(IActionDescriptor actionDescriptor, ControllerContext controllerContext, string actionName, ActionResult actionResult)
        {
            if (actionResult == null)
                return;

            var resultFilters = FindResultFilters(actionDescriptor, controllerContext, actionName);
            var preResultContext = ExecutePreResultFilters(controllerContext, actionResult, resultFilters);
            if (preResultContext.Cancel)
                return;

            actionResult = preResultContext.Result;

            controllerContext.Request.ReportProgress(new ExecutingResultNavigationEvent());

            try
            {
                actionResult.Execute(controllerContext);
            }
            catch (Exception ex)
            {
                var errorResult = ExecutePostResultFilters(controllerContext, actionResult, ex, resultFilters);
                if (errorResult.ExceptionHandled || errorResult.Exception == null)
                    return;

                if (errorResult.Exception == ex)
                    throw;

                throw errorResult.Exception;
            }

            var successResult = ExecutePostResultFilters(controllerContext, actionResult, null, resultFilters);
            if (successResult.ExceptionHandled || successResult.Exception == null)
                return;

            throw successResult.Exception;
        }

        /// <summary>
        /// Invokes all post-result filters, after the result has been invoked.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionResult">The action result.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="resultFilters">The result filters.</param>
        /// <returns></returns>
        protected virtual ResultExecutedContext ExecutePostResultFilters(ControllerContext controllerContext, ActionResult actionResult, Exception exception, IEnumerable<IResultFilter> resultFilters)
        {
            controllerContext.Request.ReportProgress(new PostResultFiltersNavigationEvent());

            var context = new ResultExecutedContext(controllerContext, actionResult, exception);
            foreach (var filter in resultFilters)
            {
                filter.OnResultExecuted(context);
            }
            return context;
        }

        /// <summary>
        /// Invokes all pre-result filters, before the result has been invoked.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="result">The result.</param>
        /// <param name="resultFilters">The result filters.</param>
        /// <returns></returns>
        protected virtual ResultExecutingContext ExecutePreResultFilters(ControllerContext controllerContext, ActionResult result, IEnumerable<IResultFilter> resultFilters)
        {
            controllerContext.Request.ReportProgress(new PreResultFiltersNavigationEvent());

            var context = new ResultExecutingContext(controllerContext, result);
            foreach (var filter in resultFilters)
            {
                filter.OnResultExecuting(context);
                if (context.Cancel)
                {
                    return context;
                }
            }
            return context;
        }

        /// <summary>
        /// Invokes all pre-request filters, before the controller has been invoked.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="modelBinders">The model binders.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        protected virtual ActionResult ExecutePreActionFilters(ControllerContext controllerContext, ModelBinderDictionary modelBinders, IEnumerable<IActionFilter> filters)
        {
            controllerContext.Request.ReportProgress(new PreActionFiltersNavigationEvent());

            var context = new ActionExecutingContext(controllerContext, modelBinders);
            foreach (var filter in filters)
            {
                filter.OnActionExecuting(context);
                if (context.OverrideResult != null)
                {
                    return context.OverrideResult;
                }
            }
            return null;
        }

        /// <summary>
        /// Invokes all post-request filters after the controller has been invoked.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="result">The result.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        protected virtual ActionExecutedContext ExecutePostActionFilters(ControllerContext controllerContext, ActionResult result, Exception ex, IEnumerable<IActionFilter> filters)
        {
            controllerContext.Request.ReportProgress(new PostActionFiltersNavigationEvent());

            var executedContext = new ActionExecutedContext(controllerContext, result, ex);
            foreach (var filter in filters)
            {
                filter.OnActionExecuted(executedContext);
            }

            return executedContext;
        }
    }
}