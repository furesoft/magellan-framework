
namespace Magellan.Events
{
    /// <summary>
    /// This event indicates that the action has been invoked, and post-action filters are 
    /// about to to executed. This event does not always occur (pre-action filters can cancel the 
    /// navigation request, for example).
    /// </summary>
    public class PostActionFiltersNavigationEvent : NavigationEvent
    {
    }
}