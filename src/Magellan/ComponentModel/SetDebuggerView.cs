using System.Diagnostics;
using System.Linq;

namespace Magellan.ComponentModel
{
    /// <summary>
    /// A debugger visualizer for <see cref="Set{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of item in the set.</typeparam>
    internal sealed class SetDebuggerView<T> where T : class
    {
        private readonly Set<T> collection;

        public SetDebuggerView(Set<T> collection)
        {
            this.collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                return collection.ToArray();
            }
        }
    }
}