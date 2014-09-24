using System.Text.RegularExpressions;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// A <see cref="IRouteConstraint"/> that matches a regular expression.
    /// </summary>
    public sealed class RegexConstraint : IRouteConstraint
    {
        private readonly Regex regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexConstraint"/> class.
        /// </summary>
        /// <param name="regex">The regex.</param>
        public RegexConstraint(string regex)
        {
            Guard.ArgumentNotNullOrEmpty(regex, "regex");
            this.regex = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexConstraint"/> class.
        /// </summary>
        /// <param name="regex">The regex.</param>
        public RegexConstraint(Regex regex)
        {
            Guard.ArgumentNotNull(regex, "regex");
            this.regex = regex;
        }

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
        public bool IsValid(IRoute route, string value, string parameterName)
        {
            return regex.Match(value).Success;
        }
    }
}
