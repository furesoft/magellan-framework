
namespace Magellan.Events
{
    /// <summary>
    /// This event indicates that the pre-result filters have been executed, and the result
    /// is about to be executed (this is typically when views are rendered). This event does not always 
    /// occur (action filters or pre-action filters can cancel the navigation request, for example). 
    /// </summary>
    public class ExecutingResultNavigationEvent : NavigationEvent
    {
    }
}