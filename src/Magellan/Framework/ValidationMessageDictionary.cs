using System;
using System.Collections.Generic;
using System.Linq;

namespace Magellan.Framework
{
    /// <summary>
    /// Maintains a list of validation message objects associated with the property name (key) that is associated with 
    /// each error.
    /// </summary>
    public class ValidationMessageDictionary
    {
        private readonly Dictionary<string, ValidationMessageCollection> items = new Dictionary<string, ValidationMessageCollection>();
        private readonly object sync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessageDictionary"/> class.
        /// </summary>
        public ValidationMessageDictionary()
        {
            
        }

        /// <summary>
        /// Occurs when the set of errors changes.
        /// </summary>
        public event EventHandler ErrorsChanged;

        /// <summary>
        /// Gets the validation messages (a collection) associated with the specified key, usually a property name.
        /// </summary>
        public ValidationMessageCollection this[string key]
        {
            get
            {
                lock (sync)
                {
                    if (!items.ContainsKey(key))
                    {
                        items[key] = new ValidationMessageCollection();
                        items[key].CollectionChanged += (x, y) => OnErrorsChanged(EventArgs.Empty);
                    }
                    return items[key];
                }
            }
        }

        /// <summary>
        /// Gets all error messages for every key.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> AllMessages()
        {
            return items.Values.SelectMany(x => x);
        }

        /// <summary>
        /// Gets all available keys.
        /// </summary>
        /// <value>The keys.</value>
        public string[] Keys
        {
            get { return items.Keys.ToArray(); }
        }

        /// <summary>
        /// Adds an error message for the specified key.
        /// </summary>
        /// <param name="key">The key, usually a property name.</param>
        /// <param name="message">The error message or object.</param>
        public void Add(string key, object message)
        {
            this[key].Add(message);
        }

        /// <summary>
        /// Removes all validation errors.
        /// </summary>
        public void Clear()
        {
            List<ValidationMessageCollection> list;
            lock (sync)
            {
                list = this.items.Values.ToList();
                this.items.Clear();
            }

            foreach (var item in list)
            {
                item.Clear();
            }
            OnErrorsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ErrorsChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnErrorsChanged(EventArgs e)
        {
            var handler = ErrorsChanged;
            if (handler != null) handler(this, e);
        }
    }
}