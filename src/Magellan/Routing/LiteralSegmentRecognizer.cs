using System.Text.RegularExpressions;

namespace Magellan.Routing
{
    /// <summary>
    /// A <see cref="SegmentRecognizer"/> that matches literal values such as "foo" from a route 
    /// specification, producing a <see cref="LiteralSegment"/>.
    /// </summary>
    public class LiteralSegmentRecognizer : RegexBasedSegmentRecognizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralSegmentRecognizer"/> class.
        /// </summary>
        public LiteralSegmentRecognizer()
            : base(new Regex("^[A-Z0-9\\-]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline))
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
            return new LiteralSegment(match.Value);
        }
    }
}