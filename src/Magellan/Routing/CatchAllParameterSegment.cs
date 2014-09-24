using System.Text.RegularExpressions;
using Magellan.Exceptions;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// Represents a route specification segment such as "{*path}".
    /// </summary>
    internal class CatchAllParameterSegment : Segment
    {
        private readonly string parameterName;
        private readonly object defaultValue;
        private readonly IRouteConstraint constraint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatchAllParameterSegment"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="constraint">The constraint.</param>
        public CatchAllParameterSegment(string parameterName, object defaultValue, object constraint)
        {
            Guard.ArgumentNotNullOrEmpty(parameterName, "parameterName");
            this.parameterName = parameterName;
            this.defaultValue = defaultValue;
            
            if (constraint is string)
            {
                this.constraint = new RegexConstraint((string)constraint);
            }
            else if (constraint is IRouteConstraint)
            {
                this.constraint = (IRouteConstraint)constraint;
            }
            else if (constraint is Regex)
            {
                this.constraint = new RegexConstraint((Regex)constraint);
            }
            else if (constraint == UrlParameter.NotSpecified)
            {
                this.constraint = null;
            }
            else if (constraint != null)
            {
                throw new UnsupportedConstraintException("The parameter '{0}' was given an invalid constraints. Constraints must be strings, Regex's or objects that implement IRouteConstraint");
            }
        }

        /// <summary>
        /// Gets the name of the parameter (e.g., if created with a spec of "{*foo}", the value will be 
        /// "foo").
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName
        {
            get { return parameterName; }
        }

        /// <summary>
        /// Matches the path.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override SegmentPathMatch MatchPath(IRoute route, PathIterator reader)
        {
            var values = new RouteValueDictionary();
            var path = reader.ReadAll();

            values[parameterName] = path;
            if (path.Length == 0)
            {
                if (defaultValue != UrlParameter.NotSpecified)
                {
                    values[parameterName] = defaultValue;
                }
            }
            else if (constraint != null && !constraint.IsValid(route, path, parameterName))
            {
                return SegmentPathMatch.Failure(string.Format("Segment '{0}' did not match constraint for parameter '{1}'", path, parameterName));
            }
            return SegmentPathMatch.Successful(values);
        }

        /// <summary>
        /// Given a set of route values, extracts any necessary values that would be used by this segment.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// An object that indicates whether the values were successfully matched.
        /// </returns>
        public override SegmentValueMatch MatchValues(IRoute route, RouteValueBag values)
        {
            var value = values.Take(parameterName);
            if (value == UrlParameter.NotSpecified)
            {
                if (defaultValue != UrlParameter.NotSpecified)
                {
                    return SegmentValueMatch.Successful(defaultValue);
                }
                return SegmentValueMatch.Successful();
            }

            return SegmentValueMatch.Successful(value);
        }
    }
}
