using System.Linq;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// A validator for Magellan's MVC routes (<see cref="ControllerRouteHandler"/>).
    /// </summary>
    public class ControllerRouteValidator : RouteValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRouteValidator"/> class.
        /// </summary>
        public ControllerRouteValidator()
        {
            Rules.Add(MustHaveControllerSegment);
            Rules.Add(MustHaveActionSegment);
        }

        private static RouteValidationResult MustHaveControllerSegment(Segment[] segments, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var hasController = segments.OfType<ParameterSegment>().Any(x => x.ParameterName == "controller")
                                || defaults.GetOrDefault<object>("controller") != null;
         
            return hasController 
                ? RouteValidationResult.Successful()
                : RouteValidationResult.Failure("The route does not contain a '{controller}' segment, and no default controller was provided.");
        }

        private static RouteValidationResult MustHaveActionSegment(Segment[] segments, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var hasController = segments.OfType<ParameterSegment>().Any(x => x.ParameterName == "action")
                                || defaults.GetOrDefault<object>("action") != null;
         
            return hasController 
                ? RouteValidationResult.Successful()
                : RouteValidationResult.Failure("The route does not contain an '{action}' segment, and no default action was provided.");
        }
    }
}