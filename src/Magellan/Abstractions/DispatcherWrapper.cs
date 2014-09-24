using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using Magellan.Diagnostics;
using Magellan.Framework;

namespace Magellan.Abstractions
{
    /// <summary>
    /// This dispatcher is used at runtime by both Windows Forms and WPF. The WPF Dispatcher class works 
    /// within Windows Forms, so this appears to be safe.
    /// </summary>
    public class DispatcherWrapper : IDispatcher
    {
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherWrapper"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public DispatcherWrapper(Dispatcher dispatcher)
        {
            if (dispatcher == null)
            {
                if (Application.Current != null)
                {
                    dispatcher = Application.Current.Dispatcher;
                }
                else
                {
                    dispatcher = Dispatcher.CurrentDispatcher;
                }
            }
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        [DebuggerNonUserCode]
        public void Dispatch(Action actionToInvoke)
        {
            if (!DispatchRequired())
            {
                actionToInvoke();
            }
            else
            {
                dispatcher.Invoke(DispatcherPriority.Normal, actionToInvoke);   
            }
        }

        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="actionToInvoke">The action to invoke.</param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public TResult Dispatch<TResult>(Func<TResult> actionToInvoke)
        {
            if (dispatcher.CheckAccess())
            {
                return actionToInvoke();
            }
            return (TResult)dispatcher.Invoke(DispatcherPriority.Normal, actionToInvoke);
        }

        /// <summary>
        /// Checks whether the thread invoking the method.
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public bool DispatchRequired()
        {
            return !dispatcher.CheckAccess();
        }


        /// <summary>
        /// Executes a task on new background thread, but any exceptions will be marshalled back to the UI
        /// thread using this dispatcher.
        /// </summary>
        /// <param name="backgroundThreadCallback">A code block to execute on the background thread.</param>
        public void ExecuteOnNewBackgroundThreadWithExceptionsOnUIThread(Action backgroundThreadCallback)
        {
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    ExecuteOnCurrentThreadWithExceptionsOnUIThread(backgroundThreadCallback);
                });
        }

        /// <summary>
        /// Executes an operation on the current thread (assuming the current thread is a background thread),
        /// but marshalls any exceptions back to the UI thread for you.
        /// </summary>
        /// <param name="backgroundThreadCallback">A code block to execute on the background thread.</param>
        public void ExecuteOnCurrentThreadWithExceptionsOnUIThread(Action backgroundThreadCallback)
        {
            try
            {
                backgroundThreadCallback();
            }
            catch (Exception ex)
            {
                TraceSources.MagellanSource.TraceError("Exeption thrown on worker thread: " + ex);
                var rethrower = new Rethrower("An unexpected error occurred when executing a background operation.", ex);
                Dispatch(rethrower.Rethrow);
            }
        }

        /// <summary>
        /// Used to rethrow an exception on the UI thread that was caught on the background thread.
        /// </summary>
        [DebuggerNonUserCode] // Prevents breakpoints from stopping in this class
        private class Rethrower
        {
            private readonly string _message;
            private readonly Exception _ex;

            public Rethrower(string message, Exception ex)
            {
                _message = message;
                _ex = ex;
            }

            public void Rethrow()
            {
                throw new BackgroundOperationException(_message, _ex);
            }
        }
    }
}