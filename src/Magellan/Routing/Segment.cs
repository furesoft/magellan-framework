namespace Magellan.Routing
{
    /// <summary>
    /// A segment represents part of a route specification that will be chained together to match a path to 
    /// a route.
    /// </summary>
    public abstract class Segment
    {
        /// <summary>
        /// Given a path, attempts to match the next part of it to the current segment.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="path">The path.</param>
        /// <returns>An object that indicates whether the path was successfully matched.</returns>
        public abstract SegmentPathMatch MatchPath(IRoute route, PathIterator path);
        
        /// <summary>
        /// Given a set of route values, extracts any necessary values that would be used by this segment.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="values">The values.</param>
        /// <returns>An object that indicates whether the values were successfully matched.</returns>
        public abstract SegmentValueMatch MatchValues(IRoute route, RouteValueBag values);
    }
}