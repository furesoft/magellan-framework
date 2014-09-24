using System.Text.RegularExpressions;

namespace Magellan.Routing
{
    /// <summary>
    /// A useful <see cref="SegmentRecognizer"/> that makes it easier to work with regular expressions.
    /// </summary>
    public abstract class RegexBasedSegmentRecognizer : SegmentRecognizer
    {
        private readonly Regex formatRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexBasedSegmentRecognizer"/> class.
        /// </summary>
        /// <param name="formatRegex">The format regex.</param>
        protected RegexBasedSegmentRecognizer(Regex formatRegex)
        {
            this.formatRegex = formatRegex;
        }

        /// <summary>
        /// Tries to produce an appropriate segment for the given route specification part. Returning null
        /// indicates that the value is not valid for this kind of segment.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <returns>
        /// The <see cref="Segment"/> produced by this <see cref="SegmentRecognizer"/>, or null if
        /// the value is not recognizable.
        /// </returns>
        public override Segment Recognise(string value, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            var match = formatRegex.Match(value);
            if (!match.Success)
                return null;

            return Build(match, defaults, constraints);
        }

        /// <summary>
        /// Builds a segment from the regular expression match. This method is only called when the base 
        /// class matches a the value to this regex.
        /// </summary>
        /// <param name="match">The regular expression match.</param>
        /// <param name="defaults">The default values used in the route.</param>
        /// <param name="constraints">The constraints used in the route.</param>
        /// <returns>The route segment.</returns>
        protected abstract Segment Build(Match match, RouteValueDictionary defaults, RouteValueDictionary constraints);
    }
}