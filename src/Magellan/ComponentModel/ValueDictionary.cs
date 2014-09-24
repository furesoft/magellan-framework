using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magellan.Utilities;

namespace Magellan.ComponentModel
{
    /// <summary>
    /// A base class for associative arrays and dictionaries used for parameter passing.
    /// </summary>
    public abstract class ValueDictionary : Dictionary<string, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueDictionary"/> class.
        /// </summary>
        protected ValueDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueDictionary"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        protected ValueDictionary(IDictionary items)
        {
            if (items == null) return;
            foreach (var key in items.Keys.Cast<object>().Where(key => key != null))
            {
                Add((key).ToString(), items[key]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueDictionary"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        protected ValueDictionary(object items)
        {
            if (items == null) return;
            if (items is string || items.GetType().IsValueType)
                throw new InvalidOperationException("A string or value type was passed to a method that expected a parameter value dictionary. These parameters are normally used for anonymous objects to represent key/value pairs as arguments. Ensure that the right method overload was called.");
            foreach (var pair in PropertyValueLister.GetProperties(items))
            {
                Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Gets a value by the specified key, or <c>default(TValue)</c> if not found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public TValue GetOrDefault<TValue>(string key, TValue defaultValue)
        {
            if (ContainsKey(key))
            {
                return (TValue)this[key];
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets a value by the specified key, or <c>default(TValue)</c> if not found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TValue GetOrDefault<TValue>(string key)
        {
            return GetOrDefault(key, default(TValue));
        }

        /// <summary>
        /// Adds all parameters in the given dictionary to the current set. Items that already exist will be
        /// ignored.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(ValueDictionary items)
        {
            AddRange(items, false);
        }

        /// <summary>
        /// Adds all parameters in the given dictionary to the current set.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="overwriteExisting">if set to <c>true</c> existing items will be overritten, or if false the new items will be ignored.</param>
        public void AddRange(ValueDictionary items, bool overwriteExisting)
        {
            if (items == null) return;
            if (items == this) throw new InvalidOperationException("An attempt was made to add the parameters from the current context to itself.");
            foreach (var key in items.Keys.OfType<string>())
            {
                if (ContainsKey(key))
                {
                    if (overwriteExisting)
                    {
                        this[key] = items[key];
                    }
                }
                else
                {
                    Add((key ?? string.Empty), items[key]);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            var pairs = this.Take(5);
            result.Append(string.Join("; ", pairs.Select(x => string.Format("{0}={1}", x.Key, x.Value)).ToArray()));
            return result.ToString();
        }
    }
}