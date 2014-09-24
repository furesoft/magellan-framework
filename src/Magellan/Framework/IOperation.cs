using System;
using System.Threading;

namespace Magellan.Framework
{
    /// <summary>
    /// Represents a hande to a running background operation.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// A wait handle that can be used to wait until this operation has completed.
        /// </summary>
        /// <value>The handle.</value>
        WaitHandle Handle { get; }

        /// <summary>
        /// Executed the specified code block on the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        void Dispatch(Action action);

        /// <summary>
        /// Cancels this operation. This sets <see cref="Cancelled"/> to true, with the assumption that your
        /// background task will poll this property.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Gets a value indicating whether this <see cref="IOperation"/> has been cancelled.
        /// </summary>
        /// <value><c>true</c> if cancelled; otherwise, <c>false</c>.</value>
        bool Cancelled { get; }

        /// <summary>
        /// Waits for this operation to complete.
        /// </summary>
        void WaitForCompletion();
    }
}