namespace Magellan.Events
{
    /// <summary>
    /// The final navigation event. This indicates that the navigation request has been completed 
    /// (whether successfully or failed), any views have been rendered and any resources from the 
    /// navigation request have been cleared up.
    /// </summary>
    public class CompleteNavigationEvent : NavigationEvent
    {
    }
}