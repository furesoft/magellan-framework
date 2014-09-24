using System.Threading;
using System;
using System.Diagnostics;
using Magellan.Exceptions;

namespace Magellan.Framework
{
    /// <summary>
    /// An asynchronous version of the <see cref="DefaultActionInvoker"/>. Actions are executed on a 
    /// background thread. Any execptions will be automatically dispatched back to the foreground thread.
    /// </summary>
    public class AsyncActionInvoker : DefaultActionInvoker
    {
        private void ExecuteBase(ControllerContext controllerContext, string actionName, ModelBinderDictionary modelBinders)
        {
            base.ExecuteAction(controllerContext, actionName, modelBinders);
        }

        /// <summary>
        /// Executes the action on the specified controller.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="modelBinders">The model binders.</param>
        public override void ExecuteAction(ControllerContext controllerContext, string actionName, ModelBinderDictionary modelBinders)
        {
            var dispatcher = controllerContext.Request.Navigator.Dispatcher;
            ThreadPool.QueueUserWorkItem(
                delegate
                    {
                        Thread.CurrentThread.Name = string.Format("Navigation request: {0}", controllerContext.Request);
                        try
                        {
                            ExecuteBase(controllerContext, actionName, modelBinders);
                        }
                        catch (Exception ex)
                        {
                            var rethrower = new Rethrower(
                                string.Format("An exception occurred when attempting to asynchronously execute the request '{0}'. {1}", controllerContext.Request, ex.Message), 
                                ex);
                            dispatcher.Dispatch(rethrower.RethrowOnDispatchThread);
                        }
                    });
        }

        /// <summary>
        /// Used to rethrow an exception on the UI thread that was caught on the background thread.
        /// </summary>
        [DebuggerNonUserCode] 
        private class Rethrower
        {
            private readonly string _message;
            private readonly Exception _ex;

            public Rethrower(string message, Exception ex)
            {
                _message = message;
                _ex = ex;
            }

            public void RethrowOnDispatchThread()
            {
                throw new AsyncControllerExecutionException(_message, _ex);
            }
        }
    }
}
