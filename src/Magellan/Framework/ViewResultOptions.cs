using Magellan.ComponentModel;

namespace Magellan.Framework
{
    /// <summary>
    /// A dictionary of settings for view results.
    /// </summary>
    public class ViewResultOptions : ValueDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewResultOptions"/> class.
        /// </summary>
        public ViewResultOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewResultOptions"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public ViewResultOptions(object items) : base(items)
        {
        }
    }
}