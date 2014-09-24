using Magellan.Routing;

namespace Magellan.Events
{
    internal interface INavigationEvent
    {
        ResolvedNavigationRequest Request { get; set; }
    }
}