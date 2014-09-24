using System.Linq;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// A route validator for Magellan's MVVM routin support.
    /// </summary>
    internal class ViewModelRouteValidator : RouteValidator
    {
        public ViewModelRouteValidator()
        {
            Rules.Add(MustHaveViewModelSegment);
        }

        private static RouteValidationResult MustHaveViewModelSegment(Segment[] segments, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var hasViewModel = segments.OfType<ParameterSegment>().Any(x => x.ParameterName == "viewModel")
                || defaults.GetOrDefault<object>("viewModel") != null;

            return hasViewModel 
                ? RouteValidationResult.Successful() 
                : RouteValidationResult.Failure("The route does not contain a '{viewModel}' segment, and no default ViewModel type was provided.");
        }
    }
}