using System.Collections.Generic;
using System.ComponentModel;
using Magellan.ComponentModel;

namespace Magellan.Framework
{
    /// <summary>
    /// Maintains a collection of flashes. Removes flashes from the collection when they are closed.
    /// </summary>
    public class FlashCollection : DispatchedObservableCollection<Flash>
    {
        private readonly IScheduler scheduler;
        private readonly Dictionary<Flash, ITimer> timers = new Dictionary<Flash, ITimer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashCollection"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        public FlashCollection(IScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void InsertItem(int index, Flash item)
        {
            item.PropertyChanged += ItemPropertyChanged;
            if (item.Expiry != null)
            {
                var timer = scheduler.ScheduleOnce(item.Expiry.Value, x => Remove(item));
                timers[item] = timer;
            }
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void RemoveItem(int index)
        {
            if (index >= 0 && index < Count)
            {
                var item = this[index];
                if (timers.ContainsKey(item))
                {
                    var timer = timers[item];
                    timer.Cancel();
                }
            }
            base.RemoveItem(index);
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Closed")
            {
                var item = (Flash)sender;
                Remove(item);
            }
        }
    }
}
