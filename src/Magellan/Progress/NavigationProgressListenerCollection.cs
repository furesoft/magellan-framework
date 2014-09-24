using Magellan.ComponentModel;

namespace Magellan.Progress
{
    /// <summary>
    /// Represents a collection of <see cref="INavigationProgressListener">navigation progress 
    /// listeners</see>.
    /// </summary>
    public class NavigationProgressListenerCollection : Set<INavigationProgressListener>
    {
    }   
}