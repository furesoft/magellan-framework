namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by timers returned by scheduled tasks.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Cancels this instance.
        /// </summary>
        void Cancel();
    }
}