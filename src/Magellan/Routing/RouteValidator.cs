using System;
using System.Collections.Generic;
using System.Linq;

namespace Magellan.Routing
{
    /// <summary>
    /// The default implementation of <see cref="IRouteValidator"/>, which enforces the invariant 
    /// expectations that apply to route specifications. 
    /// </summary>
    public class RouteValidator : IRouteValidator
    {
        private readonly List<Func<Segment[], RouteValueDictionary, RouteValueDictionary, RouteValidationResult>> rules = new List<Func<Segment[], RouteValueDictionary, RouteValueDictionary, RouteValidationResult>>();
        private readonly HashSet<Type> supportedSegmentTypes = new HashSet<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteValidator"/> class.
        /// </summary>
        public RouteValidator()
        {
            Rules.Add(EnsureNoUnrecognizableSegments);
            Rules.Add(EnsureNoMoreThanOneCatchAllSegment);
            Rules.Add(EnsureCatchAllOnlyAppearAtEnd);
            Rules.Add(EnsureParameterNamesAreUnique);
            SupportedSegmentTypes.Add(typeof(ParameterSegment));
            SupportedSegmentTypes.Add(typeof(LiteralSegment));
            SupportedSegmentTypes.Add(typeof(CatchAllParameterSegment));
        }

        /// <summary>
        /// Gets a list of supported segment types. Segment types not in this list will trigger an exception.
        /// </summary>
        /// <value>The supported segment types.</value>
        protected HashSet<Type> SupportedSegmentTypes
        {
            get { return supportedSegmentTypes; }
        }

        /// <summary>
        /// Gets the rules that apply to this route.
        /// </summary>
        /// <value>The rules.</value>
        protected List<Func<Segment[], RouteValueDictionary, RouteValueDictionary, RouteValidationResult>> Rules
        {
            get { return rules; }
        }

        /// <summary>
        /// Validates the specified route, producing a <see cref="RouteValidationResult"/> indicating 
        /// what the error (if any) was.
        /// </summary>
        /// <param name="route">The route to validate.</param>
        /// <returns>An object indicating the success of the validation attempt, and details about any error 
        /// encountered.</returns>
        public virtual RouteValidationResult Validate(ParsedRoute route)
        {
            var segments = route.Segments;

            // Shortcut - no need to check any rules for an empty route
            if (segments.Length == 0)
            {
                return RouteValidationResult.Successful();
            }

            return Rules.Select(rule => rule(segments, route.Defaults, route.Constraints))
                .FirstOrDefault(x => !x.Success) 
                ?? RouteValidationResult.Successful();
        }

        private RouteValidationResult EnsureNoUnrecognizableSegments(Segment[] segments, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var unrecognized = segments.Where(x => SupportedSegmentTypes.Contains(x.GetType()) == false).ToList();
            return unrecognized.Count > 0 
                ? RouteValidationResult.Failure(string.Format("Unrecognized segment types were found. If using a custom segment type, please replace the IRouteValidator to enforce your own route validation rules. Unrecognized types: {0}", string.Join(", ", unrecognized.Select(x => "'" + x.GetType().Name + "'").ToArray()))) 
                : RouteValidationResult.Successful();
        }

        private static RouteValidationResult EnsureNoMoreThanOneCatchAllSegment(Segment[] segments, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var catchAll = segments.OfType<CatchAllParameterSegment>().ToList();
            return catchAll.Count > 1 
                ? RouteValidationResult.Failure(string.Format("A route cannot have more than one catch-all parameter. Catch all parameters: {0}", string.Join(", ", catchAll.Select(x => "'" + x.ParameterName + "'").ToArray()))) 
                : RouteValidationResult.Successful();
        }

        private static RouteValidationResult EnsureCatchAllOnlyAppearAtEnd(Segment[] segments, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var catchAll = segments.OfType<CatchAllParameterSegment>().ToList();
            return catchAll.Count == 1 && segments.Last() != catchAll.Single()
                ? RouteValidationResult.Failure(string.Format("Catch-all parameters may only appear at the end of a route. Catch all parameter: '{0}'", catchAll.Single().ParameterName))
                : RouteValidationResult.Successful();
        }

        private static RouteValidationResult EnsureParameterNamesAreUnique(Segment[] segments, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var parameterSegments = segments.OfType<ParameterSegment>().Select(x => x.ParameterName);
            var catchAllSegments = segments.OfType<CatchAllParameterSegment>().Select(x => x.ParameterName);
            var parameterNames = parameterSegments.Concat(catchAllSegments).Select(x => x.ToLowerInvariant());

            var duplicateNames = parameterNames.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key).ToList();
            
            return duplicateNames.Count > 0 
                ? RouteValidationResult.Failure(string.Format("The same parameter name cannot be used twice within a route. The following parameters appeared more than once: {0}", string.Join(", ", duplicateNames.Select(x => "'" + x + "'").ToArray()))) 
                : RouteValidationResult.Successful();
        }
    }
}