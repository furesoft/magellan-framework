using System.Collections.Generic;
using System.Linq;

namespace Magellan.Routing
{
    /// <summary>
    /// Represents a route specification that has been parsed using an <see cref="IRouteParser"/>.
    /// </summary>
    public class ParsedRoute
    {
        private readonly RouteValueDictionary defaults;
        private readonly RouteValueDictionary constraints;
        private readonly Segment[] segments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedRoute"/> class.
        /// </summary>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <param name="segments">The segments.</param>
        public ParsedRoute(RouteValueDictionary defaults, RouteValueDictionary constraints, params Segment[] segments)
        {
            this.defaults = defaults;
            this.constraints = constraints;
            this.segments = segments;
        }

        /// <summary>
        /// Gets the segments that make up this route.
        /// </summary>
        /// <value>The segments.</value>
        public Segment[] Segments
        {
            get { return segments; }
        }

        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <value>The defaults.</value>
        public RouteValueDictionary Defaults
        {
            get { return defaults; }
        }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        /// <value>The constraints.</value>
        public RouteValueDictionary Constraints
        {
            get { return constraints; }
        }

        /// <summary>
        /// Attempts to matche a path to this parsed route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="request">The request.</param>
        /// <returns>An object indicating the success or failure of the attempt to match the path.</returns>
        public RouteMatch MatchPathToRoute(IRoute route, string request)
        {
            var values = new RouteValueDictionary();
            var iterator = new PathIterator(request);

            foreach (var segment in Segments)
            {
                var match = segment.MatchPath(route, iterator);
                if (match.Success)
                {
                    values.AddRange(match.Values);
                }
                else
                {
                    return RouteMatch.Failure(route, match.FailReason);
                }
            }

            return iterator.IsAtEnd
                ? RouteMatch.Successful(route, values)
                : RouteMatch.Failure(route, "Route was initially matched, but the request contains additional unexpected segments");
        }

        /// <summary>
        /// Attempts to matche given route values to this parsed route, building up a probable path that
        /// could be used to navigate to this route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// An object indicating the success or failure of the attempt to match the path.
        /// </returns>
        public PathMatch MatchRouteToPath(IRoute route, RouteValueDictionary values)
        {
            var valueBag = new RouteValueBag(values);
            var segmentValues = new List<object>();

            foreach (var segment in Segments)
            {
                var match = segment.MatchValues(route, valueBag);
                if (match.Success)
                {
                    segmentValues.Add(match.SegmentValue);
                }
                else
                {
                    return PathMatch.Failure(route, match.FailReason);
                }
            }

            var allValues = new RouteValueDictionary(Defaults);
            allValues.AddRange(values, true);
            allValues.AddRange(values, true);

            var leftOver = valueBag.GetRemaining();
            
            foreach (var leftOverItem in leftOver)
            {
                var key = leftOverItem.Key;
                if (Defaults.ContainsKey(key))
                {
                    var leftOverValue = leftOverItem.Value;
                    var defaultValue = Defaults[leftOverItem.Key];
                    
                    if ((leftOverValue == null || defaultValue == null) && (leftOverValue != defaultValue)
                        || (leftOverValue != null && !leftOverValue.Equals(defaultValue)))
                    {
                        return PathMatch.Failure(route, string.Format("The route was a close match, but the value of the '{0}' parameter was expected to be '{1}', but '{2}' was provided instead.", key, defaultValue, leftOverValue));
                    }
                }
            }
            
            if (leftOver.Count > 0)
            {
                foreach (var defaultItem in Defaults.Where(item => leftOver.ContainsKey(item.Key)))
                {
                    leftOver.Remove(defaultItem.Key);
                }
            }

            return PathMatch.Successful(route, allValues, leftOver, segmentValues);
        }
    }
}