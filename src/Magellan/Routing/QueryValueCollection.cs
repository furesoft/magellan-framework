using System.Collections.Generic;
using System.Diagnostics;

namespace Magellan.Routing
{
    /// <summary>
    /// Represents a collection of query string values.
    /// </summary>
    [DebuggerTypeProxy(typeof(QueryValueCollectionDebuggerView))]
    [DebuggerDisplay("Count = {Count}")]
    public class QueryValueCollection : List<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryValueCollection"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public QueryValueCollection(string query)
        {
            if (query == null)
                return;

            var parts = query.Split('&');
            foreach (var part in parts)
            {
                var indexOfFirstEquals = part.IndexOf('=');
                if (indexOfFirstEquals > 0)
                {
                    var name = part.Substring(0, indexOfFirstEquals);
                    var value = part.Substring(indexOfFirstEquals + 1);
                    Add(name, value);
                }
            }
        }

        /// <summary>
        /// Adds a value with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, string value)
        {
            Add(new KeyValuePair<string, string>(key, value));
        }
    }
}
