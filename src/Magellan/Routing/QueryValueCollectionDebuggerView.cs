using System.Collections.Generic;
using System.Diagnostics;

namespace Magellan.Routing
{
    internal sealed class QueryValueCollectionDebuggerView
    {
        private readonly QueryValueCollection collection;

        public QueryValueCollectionDebuggerView(QueryValueCollection collection)
        {
            this.collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<string, string>[] Items
        {
            get
            {
                var array = new KeyValuePair<string, string>[collection.Count];
                collection.CopyTo(array, 0);
                return array;
            }
        }
    }
}