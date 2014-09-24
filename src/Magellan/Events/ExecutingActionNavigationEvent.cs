
namespace Magellan.Events
{
    /// <summary>
    /// This event indicates that pre-action filters have been invoked, and the action 
    /// is about to to executed. This event does not always occur (pre-action filters can cancel the 
    /// navigation request, for example).
    /// </summary>
    public class ExecutingActionNavigationEvent : NavigationEvent
    {
    }
}