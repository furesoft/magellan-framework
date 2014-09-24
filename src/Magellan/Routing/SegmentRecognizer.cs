namespace Magellan.Routing
{
    /// <summary>
    /// Implemented by strategies that can recognize a part of a route specification, and produce a 
    /// <see cref="Segment"/> that can later be used when evaluating paths to this route.
    /// </summary>
    public abstract class SegmentRecognizer
    {
        /// <summary>
        /// Tries to produce an appropriate segment for the given route specification part. Returning null 
        /// indicates that the value is not valid for this kind of segment.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <returns>The <see cref="Segment"/> produced by this <see cref="SegmentRecognizer"/>, or null if 
        /// the value is not recognizable.</returns>
        public abstract Segment Recognise(string value, RouteValueDictionary defaults, RouteValueDictionary constraints);
    }
}