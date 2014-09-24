namespace Magellan.Routing
{
    /// <summary>
    /// Implemented by objects that can validate a route specification.
    /// </summary>
    public interface IRouteValidator
    {
        /// <summary>
        /// Validates the specified route, producing a <see cref="RouteValidationResult"/> indicating 
        /// what the error (if any) was.
        /// </summary>
        /// <param name="route">The route to validate.</param>
        /// <returns>An object indicating the success of the validation attempt, and details about any error 
        /// encountered.</returns>
        RouteValidationResult Validate(ParsedRoute route);
    }
}