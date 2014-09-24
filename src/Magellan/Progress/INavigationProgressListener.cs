using Magellan.Events;

namespace Magellan.Progress
{
    /// <summary>
    /// Implemented by objects that wish to be notified of navigation progress.
    /// </summary>
    public interface INavigationProgressListener
    {
        /// <summary>
        /// Informs the navigation progress listener of the current 
        /// </summary>
        void UpdateProgress(NavigationEvent navigationEvent);
    }
}