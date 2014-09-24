using System;

namespace Magellan.Abstractions
{
    /// <summary>
    /// Provides a wrapper around the process of dispatching actions onto different threads, primarily for 
    /// unit testing.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        void Dispatch(Action actionToInvoke);

        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        TResult Dispatch<TResult>(Func<TResult> actionToInvoke);

        /// <summary>
        /// Checks whether the thread invoking the method .
        /// </summary>
        /// <returns></returns>
        bool DispatchRequired();

        /// <summary>
        /// Executes a task on new background thread, but any exceptions will be marshalled back to the UI 
        /// thread using this dispatcher.
        /// </summary>
        /// <param name="backgroundThreadCallback">A code block to execute on the background thread.</param>
        void ExecuteOnNewBackgroundThreadWithExceptionsOnUIThread(Action backgroundThreadCallback);

        /// <summary>
        /// Executes an operation on the current thread (assuming the current thread is a background thread),
        /// but marshalls any exceptions back to the UI thread for you.
        /// </summary>
        /// <param name="backgroundThreadCallback">A code block to execute on the background thread.</param>
        void ExecuteOnCurrentThreadWithExceptionsOnUIThread(Action backgroundThreadCallback);
    }
}