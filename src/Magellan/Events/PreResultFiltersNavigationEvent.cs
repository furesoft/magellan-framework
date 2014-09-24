using Magellan.Framework;

namespace Magellan.Events
{
    /// <summary>
    /// This event indicates that the action has been executed and all 
    /// <see cref="IActionFilter">action filters</see> have been invoked. The result is now about to be 
    /// evaluated. This event does not always occur (action filters can cancel the navigation request, 
    /// for example).
    /// </summary>
    public class PreResultFiltersNavigationEvent : NavigationEvent
    {
    }
}