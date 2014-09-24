using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Magellan.ComponentModel
{
    /// <summary>
    /// A simple thread-safe ordered collection where the same items can only appear once. It is thread safe
    /// in the sense that one thread can iterate over the collection, blocking another thread from modifying 
    /// the collection during the process. When combining individual actions (such as multiple calls to 
    /// <see cref="Add"/>) the collection is still not thread safe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerTypeProxy(typeof(SetDebuggerView<>)), DebuggerDisplay("Count = {Count}")]
    public class Set<T> : IEnumerable<T> where T : class
    {
        private readonly List<T> innerList = new List<T>();
        private readonly object sync = new object();

        /// <summary>
        /// Executes a batch of operations on the underlying item list, whilst holding a lock.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        protected void Edit(Action<List<T>> configurator)
        {
            lock (sync)
            {
                configurator(innerList);
            }
        }

        /// <summary>
        /// Adds the specified item if it does not already exist.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            if (item == null) return;
            lock (sync)
            {
                if (!innerList.Contains(item))
                {
                    innerList.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds the range of items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<T> items)
        {
            var allItems = items.ToList();
            lock (sync)
            {
                foreach (var item in allItems)
                {
                    Add(item);
                }
            }
        }

        /// <summary>
        /// Clears all items in the collection.
        /// </summary>
        public void Clear()
        {
            lock (sync) innerList.Clear();
        }

        /// <summary>
        /// Determines whether the item exists in the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        public bool Contains(T item)
        {
            if (item == null) return false;
            lock (sync) return innerList.Contains(item);
        }

        /// <summary>
        /// Removes the specified item if it exists.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(T item)
        {
            if (item == null) return;
            lock (sync)
            {
                if (innerList.Contains(item))
                {
                    innerList.Remove(item);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (sync)
            {
                var enumerator = innerList.GetEnumerator();
                return new LockedEnumerator(enumerator, sync);
            }
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { lock (sync) return innerList.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// An enumerator that holds a lock until it is disposed.
        /// </summary>
        private class LockedEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> _innerEnumerator;
            private readonly object _syncRoot;

            public LockedEnumerator(IEnumerator<T> innerEnumerator, object syncRoot)
            {
                _innerEnumerator = innerEnumerator;
                _syncRoot = syncRoot;
                Monitor.Enter(_syncRoot);
            }

            public void Dispose()
            {
                _innerEnumerator.Dispose();
                Monitor.Exit(_syncRoot);
            }

            public bool MoveNext()
            {
                return _innerEnumerator.MoveNext();
            }

            public void Reset()
            {
                _innerEnumerator.Reset();
            }

            public T Current
            {
                get { return _innerEnumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
