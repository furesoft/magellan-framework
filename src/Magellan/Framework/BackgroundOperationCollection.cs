using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using Magellan.Abstractions;

namespace Magellan.Framework
{
    /// <summary>
    /// Keeps a list of running background tasks, and allows new tasks to be queued.
    /// </summary>
    public class BackgroundOperationCollection : IEnumerable<IOperation>, INotifyCollectionChanged
    {
        private readonly BusyState busyState;
        private readonly IDispatcher dispatcher;
        private readonly ObservableCollection<IOperation> activeOperations = new ObservableCollection<IOperation>();
        private readonly object sync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundOperationCollection"/> class.
        /// </summary>
        /// <param name="busyState">State of the busy.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public BackgroundOperationCollection(BusyState busyState, IDispatcher dispatcher)
        {
            this.busyState = busyState;
            this.dispatcher = dispatcher;
            activeOperations.CollectionChanged += (x, y) => OnCollectionChanged(y);
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Queues the specified background task, creating a new background thread on which to execute it.
        /// </summary>
        /// <param name="executedOnBackgroundThread">A code block that will be executed on the thread.</param>
        /// <returns></returns>
        public IOperation Queue(Action<IOperation> executedOnBackgroundThread)
        {
            return Queue(executedOnBackgroundThread, dispatcher.ExecuteOnNewBackgroundThreadWithExceptionsOnUIThread);
        }

        /// <summary>
        /// Queues the specified background task on the current thread, assuming the current thread is a 
        /// background thread.
        /// </summary>
        /// <param name="executedOnBackgroundThread">A code block that will be executed on the thread.</param>
        /// <returns></returns>
        public IOperation QueueFromBackground(Action<IOperation> executedOnBackgroundThread)
        {
            return Queue(executedOnBackgroundThread, dispatcher.ExecuteOnCurrentThreadWithExceptionsOnUIThread);
        }

        private IOperation Queue(Action<IOperation> executedOnBackgroundThread, Action<Action> queue)
        {
            var handle = new ManualResetEvent(false);
            var operation = new Operation(dispatcher, handle);
            lock (sync)
            {
                activeOperations.Add(operation);
            }

            var busyState = this.busyState;
            var state = busyState.Enter();
            var executionCallback = new Action(
                delegate
                {
                    try
                    {
                        executedOnBackgroundThread(operation);
                    }
                    finally
                    {
                        handle.Set();

                        dispatcher.Dispatch(() =>
                        {
                            state.Dispose();
                            lock (sync)
                            {
                                activeOperations.Remove(operation);
                            }
                        });
                    }
                });

            queue(executionCallback);

            return operation;
        }

        /// <summary>
        /// Notifies all background operations that they should cancel their work. It is up to indidividual 
        /// operations to check the <see cref="IOperation.Cancelled"/> property while executing for this to
        /// have any effect. Follow this call with a call to <see cref="WaitForCompletion"/> if you want to 
        /// wait for all running background operations to come to an end.
        /// </summary>
        public void CancelAll()
        {
            lock (sync)
            {
                foreach (var op in activeOperations)
                {
                    op.Cancel();
                }
            }
        }

        /// <summary>
        /// Waits for all background operations to complete. This is a blocking call, and could take some 
        /// time. Does not cancel running operations. 
        /// </summary>
        public void WaitForCompletion()
        {
            lock (sync)
            {
                foreach (var op in activeOperations)
                {
                    op.WaitForCompletion();
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IOperation> GetEnumerator()
        {
            lock (activeOperations)
            {
                return activeOperations.ToList().GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Represents a handle to a running background operation.
        /// </summary>
        private class Operation : IOperation
        {
            private readonly IDispatcher dispatcher;
            private readonly WaitHandle handle;
            private bool cancelled;

            /// <summary>
            /// Initializes a new instance of the <see cref="Operation"/> class.
            /// </summary>
            /// <param name="dispatcher">The dispatcher.</param>
            /// <param name="handle">The handle.</param>
            public Operation(IDispatcher dispatcher, WaitHandle handle)
            {
                this.dispatcher = dispatcher;
                this.handle = handle;
            }

            /// <summary>
            /// A wait handle that can be used to wait until this operation has completed.
            /// </summary>
            /// <value>The handle.</value>
            public WaitHandle Handle
            {
                get { return handle; }
            }

            /// <summary>
            /// Executed the specified code block on the UI thread.
            /// </summary>
            /// <param name="action">The action.</param>
            public void Dispatch(Action action)
            {
                dispatcher.Dispatch(action);
            }

            /// <summary>
            /// Cancels this operation. This sets <see cref="Cancelled"/> to true, with the assumption that your
            /// background task will poll this property.
            /// </summary>
            public void Cancel()
            {
                cancelled = true;
            }

            /// <summary>
            /// Gets a value indicating whether this <see cref="Operation"/> has been cancelled.
            /// </summary>
            /// <value><c>true</c> if cancelled; otherwise, <c>false</c>.</value>
            public bool Cancelled
            {
                get { return cancelled; }
            }

            /// <summary>
            /// Waits for this operation to complete.
            /// </summary>
            public void WaitForCompletion()
            {
                handle.WaitOne();
            }
        }
    }
}
