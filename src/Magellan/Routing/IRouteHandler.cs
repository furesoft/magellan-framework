namespace Magellan.Routing
{
    /// <summary>
    /// Implemented by objects which handle a specific navigation request.
    /// </summary>
    public interface IRouteHandler
    {
        /// <summary>
        /// Processes the navigation request.
        /// </summary>
        /// <param name="request">The navigation request information.</param>
        void ProcessRequest(ResolvedNavigationRequest request);
    }
}