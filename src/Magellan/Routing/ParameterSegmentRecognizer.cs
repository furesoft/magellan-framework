using System.Text.RegularExpressions;

namespace Magellan.Routing
{
    /// <summary>
    /// A <see cref="SegmentRecognizer"/> that recognizes parameter values, such as "{action}", and creates a 
    /// segment that can match that parameter value.
    /// </summary>
    public class ParameterSegmentRecognizer : RegexBasedSegmentRecognizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSegmentRecognizer"/> class.
        /// </summary>
        public ParameterSegmentRecognizer()
            : base(new Regex("^\\{([A-Z0-9]+)\\}$", RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
        }

        /// <summary>
        /// Builds a segment from the regular expression match. This method is only called when the base
        /// class matches a the value to this regex.
        /// </summary>
        /// <param name="match">The regular expression match.</param>
        /// <param name="defaults">The default values used in the route.</param>
        /// <param name="constraints">The constraints used in the route.</param>
        /// <returns>The route segment.</returns>
        protected override Segment Build(Match match, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var placeholder = match.Groups[1].Value;
            return new ParameterSegment(
                placeholder,
                defaults.GetOrDefault<object>(placeholder, UrlParameter.NotSpecified),
                constraints.GetOrDefault<object>(placeholder, UrlParameter.NotSpecified)
                );
        }
    }
}