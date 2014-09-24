using System;
using System.ComponentModel;

namespace Magellan.Framework
{
    /// <summary>
    /// Keeps track of whether an object is "busy" with active background operations.
    /// </summary>
    public class BusyState : INotifyPropertyChanged
    {
        private int entrances;
        private readonly object sync = new object();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether the object associated with this <see cref="BusyState"/> is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get { return entrances > 0; }
        }

        /// <summary>
        /// Enters the busy state, setting <see cref="IsBusy"/> to <c>true</c>.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> that allows you to use a <c>using</c> block.</returns>
        public IDisposable Enter()
        {
            var wasBusy = IsBusy;
            lock (sync)
            {
                entrances++;
            }
            if (wasBusy != IsBusy) OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            return new BusyStateEntrance(Exit);
        }

        /// <summary>
        /// Exits the busy state. When Exit has been called for all corresponding <see cref="Enter"/> calls,
        /// <see cref="IsBusy"/> will be <c>false</c>.
        /// </summary>
        public void Exit()
        {
            var wasBusy = IsBusy;
            lock (sync)
            {
                entrances--;
                if (entrances < 0) entrances = 0;
            }
            if (wasBusy != IsBusy) OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        private class BusyStateEntrance : IDisposable
        {
            private readonly Action _disposeCallback;

            public BusyStateEntrance(Action disposeCallback)
            {
                _disposeCallback = disposeCallback;
            }

            public void Dispose()
            {
                _disposeCallback();
            }
        }
    }
}
