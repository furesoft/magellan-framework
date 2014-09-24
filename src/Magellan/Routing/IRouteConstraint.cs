namespace Magellan.Routing
{
    /// <summary>
    /// A constraint that applies to the parameter segments of a route.
    /// </summary>
    public interface IRouteConstraint
    {
        /// <summary>
        /// Verifies the constraint against the specified route information. Returns <c>false</c> when the 
        /// constraint is violated.
        /// </summary>
        /// <param name="route">The route being matched.</param>
        /// <param name="value">The value of the route parameter to be matched.</param>
        /// <param name="parameterName">The name of the parameter being matched.</param>
        /// <returns>
        /// <c>true</c> if the value is valid according to this constraint, otherwise <c>false</c>.
        /// </returns>
        bool IsValid(IRoute route, string value, string parameterName);
    }
}
