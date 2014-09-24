using System.Text.RegularExpressions;
using Magellan.Exceptions;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// A <see cref="Segment"/> that represents a parameter value, such as "{action}", in a route specification.
    /// </summary>
    public class ParameterSegment : Segment
    {
        private readonly string parameterName;
        private readonly object defaultValue;
        private readonly IRouteConstraint constraint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSegment"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="constraint">The constraint.</param>
        public ParameterSegment(string parameterName, object defaultValue, object constraint)
        {
            Guard.ArgumentNotNullOrEmpty(parameterName, "parameterName");
            this.parameterName = parameterName;
            this.defaultValue = ReferenceEquals(defaultValue, UrlParameter.Optional) ? null : defaultValue;

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
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName
        {
            get { return parameterName; }
        }

        /// <summary>
        /// Given a path, attempts to match the next part of it to the current segment.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// An object that indicates whether the path was successfully matched.
        /// </returns>
        public override SegmentPathMatch MatchPath(IRoute route, PathIterator path)
        {
            var values = new RouteValueDictionary();
            var next = path.Next();
            
            if (next.Length == 0)
            {
                if (defaultValue != UrlParameter.NotSpecified)
                {
                    values[parameterName] = defaultValue;
                }
                else
                {
                    return SegmentPathMatch.Failure(string.Format("The path does not contain a segment for parameter '{0}'", parameterName));   
                }
            }
            else
            {
                values[parameterName] = next;

                if (constraint != null && !constraint.IsValid(route, next, parameterName))
                {
                    return SegmentPathMatch.Failure(string.Format("Segment '{0}' did not match the constraint on parameter '{1}'", next, parameterName));
                }
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
                return SegmentValueMatch.Failure(string.Format("A value for the parameter '{0}' was not provided", parameterName));
            }

            return SegmentValueMatch.Successful(value);
        }
    }
}
