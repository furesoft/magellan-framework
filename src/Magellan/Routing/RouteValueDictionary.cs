using System.Collections;
using Magellan.ComponentModel;

namespace Magellan.Routing
{
    /// <summary>
    /// Stores parameters and their values for passing along with the current navigation request.
    /// </summary>
    public class RouteValueDictionary : ValueDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteValueDictionary"/> class.
        /// </summary>
        public RouteValueDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteValueDictionary"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public RouteValueDictionary(IDictionary items) : base(items)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteValueDictionary"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public RouteValueDictionary(object items) : base(items)
        {
        }
    }
}