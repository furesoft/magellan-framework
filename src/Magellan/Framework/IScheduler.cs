using System;

namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects that allow the scheduling of tasks.
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Schedules a delegate to execute once when the specified timespan elapses.
        /// </summary>
        /// <param name="when">The delay before execution.</param>
        /// <param name="callback">The callback to invoke.</param>
        /// <returns></returns>
        ITimer ScheduleOnce(TimeSpan when, Action<ITimer> callback);

        /// <summary>
        /// Schedules a delegate to execute repeatedly when the specified timespan elapses.
        /// </summary>
        /// <param name="when">The delay before execution.</param>
        /// <param name="callback">The callback to invoke.</param>
        /// <returns></returns>
        ITimer ScheduleRecurring(TimeSpan when, Action<ITimer> callback);
        
        /// <summary>
        /// Cancels and disposes all scheduled tasks.
        /// </summary>
        void CancelAll();
    }
}