using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Magellan.Abstractions;

namespace Magellan.ComponentModel
{
    /// <summary>
    /// An ObservableCollection that raises all events on the dispatcher (UI) thread.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>Borrowed from Caliburn :)</remarks>
    public class DispatchedObservableCollection<T> : ObservableCollection<T>
    {
        private bool raiseCollectionChanged = true;
        private IDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchedObservableCollection&lt;T&gt;"/> class.
        /// </summary>
        public DispatchedObservableCollection() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchedObservableCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="collection"/> parameter cannot be null.
        /// </exception>
        public DispatchedObservableCollection(IEnumerable<T> collection)
        {
            AddRange(collection);
        }

        /// <summary>
        /// Gets or sets the dispatcher that collection changed events will be sent to.
        /// </summary>
        /// <value>The dispatcher.</value>
        public IDispatcher Dispatcher
        {
            get { return dispatcher ?? (dispatcher = GetDefaultDispatcher()); }
            set { dispatcher = value; }
        }

        private static IDispatcher GetDefaultDispatcher()
        {
            var dispatcher = Application.Current == null
                                 ? System.Windows.Threading.Dispatcher.CurrentDispatcher
                                 : Application.Current.Dispatcher;

            return new DispatcherWrapper(dispatcher);
        }

        /// <summary>
        /// Inserts the item to the specified position.
        /// </summary>
        /// <param name="index">The index to insert at.</param>
        /// <param name="item">The item to be inserted.</param>
        protected override void InsertItem(int index, T item)
        {
            Dispatcher.Dispatch(() => InsertItemBase(index, item));
        }

        /// <summary>
        /// Exposes the base implementation fo the <see cref="InsertItem"/> function.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <remarks>Used to avoid compiler warning regarding unverificable code.</remarks>
        private void InsertItemBase(int index, T item)
        {
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Moves the item within the collection.
        /// </summary>
        /// <param name="oldIndex">The old position of the item.</param>
        /// <param name="newIndex">The new position of the item.</param>
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            Dispatcher.Dispatch(() => MoveItemBase(oldIndex, newIndex));
        }

        /// <summary>
        /// Exposes the base implementation fo the <see cref="MoveItem"/> function.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <remarks>Used to avoid compiler warning regarding unverificable code.</remarks>
        private void MoveItemBase(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        /// <summary>
        /// Sets the item at the specified position.
        /// </summary>
        /// <param name="index">The index to set the item at.</param>
        /// <param name="item">The item to set.</param>
        protected override void SetItem(int index, T item)
        {
            Dispatcher.Dispatch(() => SetItemBase(index, item));
        }

        /// <summary>
        /// Exposes the base implementation fo the <see cref="SetItem"/> function.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <remarks>Used to avoid compiler warning regarding unverificable code.</remarks>
        private void SetItemBase(int index, T item)
        {
            base.SetItem(index, item);
        }

        /// <summary>
        /// Removes the item at the specified position.
        /// </summary>
        /// <param name="index">The position used to identify the item to remove.</param>
        protected override void RemoveItem(int index)
        {
            Dispatcher.Dispatch(() => RemoveItemBase(index));
        }

        /// <summary>
        /// Exposes the base implementation fo the <see cref="RemoveItem"/> function.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <remarks>Used to avoid compiler warning regarding unverificable code.</remarks>
        private void RemoveItemBase(int index)
        {
            base.RemoveItem(index);
        }

        /// <summary>
        /// Clears the items contained by the collection.
        /// </summary>
        protected override void ClearItems()
        {
            Dispatcher.Dispatch(ClearItemsBase);
        }

        /// <summary>
        /// Exposes the base implementation of the <see cref="ClearItems"/> function.
        /// </summary>
        /// <remarks>Used to avoid compiler warning regarding unverificable code.</remarks>
        private void ClearItemsBase()
        {
            base.ClearItems();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (raiseCollectionChanged)
                base.OnCollectionChanged(e);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<T> items)
        {
            raiseCollectionChanged = false;
            foreach (var item in items)
                Add(item);
            raiseCollectionChanged = true;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public void RemoveRange(IEnumerable<T> items)
        {
            raiseCollectionChanged = false;
            foreach (var item in items)
                Remove(item);
            raiseCollectionChanged = true;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}