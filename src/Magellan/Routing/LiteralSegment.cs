using System;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// A <see cref="Segment"/> that represents a literal value, such as "foo", in a route specification.
    /// </summary>
    internal class LiteralSegment : Segment
    {
        private readonly string literal;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralSegment"/> class.
        /// </summary>
        /// <param name="literal">The literal.</param>
        public LiteralSegment(string literal)
        {
            Guard.ArgumentNotNullOrEmpty(literal, "literal");
            this.literal = literal;
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
            var next = path.Next();
            return String.Equals(next, literal, StringComparison.InvariantCultureIgnoreCase)
                ? SegmentPathMatch.Successful()
                : SegmentPathMatch.Failure(string.Format("Expected segment '{0}'; got '{1}'", literal, next));
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
            return SegmentValueMatch.Successful(literal);
        }
    }
}
