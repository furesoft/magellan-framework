using System.Collections.Generic;

namespace Magellan.Routing
{
    /// <summary>
    /// Represents the result of an attempt to validate a route specification.
    /// </summary>
    public class RouteValidationResult
    {
        private readonly bool success;
        private readonly string[] errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteValidationResult"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="errors">The errors.</param>
        protected RouteValidationResult(bool success, params string[] errors)
        {
            this.success = success;
            this.errors = errors;
        }

        /// <summary>
        /// Produces a successful result.
        /// </summary>
        /// <returns></returns>
        public static RouteValidationResult Successful()
        {
            return new RouteValidationResult(true);
        }

        /// <summary>
        /// Produces an unsuccessful result.
        /// </summary>
        /// <param name="errors">The validation problems encountered with the route.</param>
        /// <returns></returns>
        public static RouteValidationResult Failure(params string[] errors)
        {
            return new RouteValidationResult(false, errors);
        }

        /// <summary>
        /// Gets a value indicating whether the route was valid.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get { return success; }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public IList<string> Errors
        {
            get { return errors; }
        }
    }
}
