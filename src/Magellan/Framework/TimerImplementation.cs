using System;
using System.Diagnostics;
using System.Threading;

namespace Magellan.Framework
{
    /// <summary>
    /// Returned by the <see cref="Scheduler"/> when a task is scheduled.
    /// </summary>
    public class TimerImplementation : ITimer, IDisposable
    {
        private readonly TimeSpan when;
        private readonly Action<ITimer> callback;
        private readonly bool recurring;
        private readonly Timer realTimer;
        private bool manuallyCancelled;
        private bool isDisposed;
        private readonly object sync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerImplementation"/> class.
        /// </summary>
        /// <param name="when">The when.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="recurring">if set to <c>true</c> [recurring].</param>
        public TimerImplementation(TimeSpan when, Action<ITimer> callback, bool recurring)
        {
            this.when = when;
            this.callback = callback;
            this.recurring = recurring;
            realTimer = new Timer(Ticked, null, TimeSpan.FromMilliseconds(-1), when);
        }

        private void Ticked(object state)
        {
            Pause();

            callback(this);

            lock (sync)
            {
                if (recurring)
                {
                    Resume();
                }
                else
                {
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Starts the task.
        /// </summary>
        public void Start()
        {
            Resume();
        }

        private void Pause()
        {
            lock (sync)
            {
                if (isDisposed)
                    return;

                realTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));
            }
        }

        private void Resume()
        {
            lock (sync)
            {
                if (isDisposed || manuallyCancelled)
                    return;

                realTimer.Change(when, when);
            }
        }

        /// <summary>
        /// Cancels this task.
        /// </summary>
        public void Cancel()
        {
            lock (sync)
            {
                manuallyCancelled = true;
                Dispose();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (sync)
            {
                if (isDisposed)
                    return;

                Pause();
                realTimer.Dispose();
                isDisposed = true;
                Trace.WriteLine("Disposed: " + GetType().Name);
            }
        }
    }
}