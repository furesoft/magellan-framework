using System.Text.RegularExpressions;

namespace Magellan.Routing
{
    /// <summary>
    /// Recognizes a segment such as "{*foo}", and produces a <see cref="CatchAllParameterSegment"/>.
    /// </summary>
    public class CatchAllParameterSegmentRecognizer : RegexBasedSegmentRecognizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatchAllParameterSegmentRecognizer"/> class.
        /// </summary>
        public CatchAllParameterSegmentRecognizer()
            : base(new Regex("^\\{\\*([A-Z0-9]+)\\}$", RegexOptions.IgnoreCase | RegexOptions.Singleline))
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
            return new CatchAllParameterSegment(
                placeholder,
                defaults.GetOrDefault<object>(placeholder, UrlParameter.NotSpecified),
                constraints.GetOrDefault<object>(placeholder, UrlParameter.NotSpecified)
                );
        }
    }
}