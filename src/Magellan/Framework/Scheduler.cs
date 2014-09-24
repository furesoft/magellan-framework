using System;
using System.Collections.Generic;

namespace Magellan.Framework
{
    /// <summary>
    /// Allows scheduling of tasks to execute after a delay.
    /// </summary>
    public class Scheduler : IScheduler
    {
        private readonly List<WeakReference> activeTimers = new List<WeakReference>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler"/> class.
        /// </summary>
        public Scheduler()
        {
        }

        /// <summary>
        /// Schedules a delegate to execute once when the specified timespan elapses.
        /// </summary>
        /// <param name="when">The delay before execution.</param>
        /// <param name="callback">The callback to invoke.</param>
        /// <returns></returns>
        public ITimer ScheduleOnce(TimeSpan when, Action<ITimer> callback)
        {
            return Schedule(when, callback, false);
        }

        /// <summary>
        /// Schedules a delegate to execute repeatedly when the specified timespan elapses.
        /// </summary>
        /// <param name="when">The delay before execution.</param>
        /// <param name="callback">The callback to invoke.</param>
        /// <returns></returns>
        public ITimer ScheduleRecurring(TimeSpan when, Action<ITimer> callback)
        {
            return Schedule(when, callback, true);
        }

        private ITimer Schedule(TimeSpan when, Action<ITimer> callback, bool recurring)
        {
            var timer = new TimerImplementation(when, callback, recurring);
            timer.Start();
            activeTimers.Add(new WeakReference(timer));
            return timer;
        }

        /// <summary>
        /// Cancels and disposes all scheduled tasks.
        /// </summary>
        public void CancelAll()
        {
            foreach (var timer in activeTimers)
            {
                var instance = timer.Target as TimerImplementation;
                if (instance == null)
                    continue;

                instance.Cancel();
            }
        }
    }
}