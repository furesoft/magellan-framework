using System.Collections;

namespace Magellan.Routing
{
    /// <summary>
    /// Represents the result of matching a segment to a path.
    /// </summary>
    public sealed class SegmentPathMatch
    {
        private readonly bool success;
        private readonly string failReason;
        private readonly RouteValueDictionary parameterValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentPathMatch"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="parameterValues">The parameter values.</param>
        /// <param name="failReason">The fail reason.</param>
        private SegmentPathMatch(bool success, IDictionary parameterValues, string failReason)
        {
            this.success = success;
            this.failReason = failReason;
            this.parameterValues = new RouteValueDictionary(parameterValues);
        }

        /// <summary>
        /// Produces a successful result.
        /// </summary>
        /// <returns></returns>
        public static SegmentPathMatch Successful()
        {
            return Successful(null);
        }

        /// <summary>
        /// Produces a successful result.
        /// </summary>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns></returns>
        public static SegmentPathMatch Successful(IDictionary parameterValues)
        {
            return new SegmentPathMatch(true, parameterValues, null);
        }

        /// <summary>
        /// Produces an unsuccessful result.
        /// </summary>
        /// <param name="failReason">The fail reason.</param>
        /// <returns></returns>
        public static SegmentPathMatch Failure(string failReason)
        {
            return new SegmentPathMatch(false, null, failReason);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SegmentPathMatch"/> is successful.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get { return success; }
        }

        /// <summary>
        /// Gets the route values extracted from the path.
        /// </summary>
        /// <value>The values.</value>
        public RouteValueDictionary Values
        {
            get { return parameterValues; }
        }

        /// <summary>
        /// Gets the reason for the segment path match to fail.
        /// </summary>
        /// <value>The fail reason.</value>
        public string FailReason
        {
            get { return failReason; }
        }
    }
}