using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using Magellan.Abstractions;

namespace Magellan.Testability
{
    /// <summary>
    /// Represents an <see cref="IDispatcher"/> used for tests. Since test frameworks
    /// do not have message pumps normally used by dispatchers, the event is simply 
    /// invoked on a new thread.
    /// </summary>
    [DebuggerNonUserCode]
    public class SingleThreadDispatcher : IDispatcher
    {
        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        public void Dispatch(Action actionToInvoke)
        {
            actionToInvoke();
        }

        /// <summary>
        /// Dispatches the specified action to invoke.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="actionToInvoke">The action to invoke.</param>
        /// <returns></returns>
        public TResult Dispatch<TResult>(Func<TResult> actionToInvoke)
        {
            return actionToInvoke();
        }

        /// <summary>
        /// Checks whether the thread invoking the method is the dispatcher thread.
        /// </summary>
        /// <returns></returns>
        public bool DispatchRequired()
        {
            return true;
        }

        public void ExecuteOnNewBackgroundThreadWithExceptionsOnUIThread(Action backgroundThreadCallback)
        {
            backgroundThreadCallback();
        }

        public void ExecuteOnCurrentThreadWithExceptionsOnUIThread(Action backgroundThreadCallback)
        {
            backgroundThreadCallback();
        }
    }
}