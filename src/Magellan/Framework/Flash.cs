using System;
using System.Windows.Input;

namespace Magellan.Framework
{
    /// <summary>
    /// Represents an item that suddenly appears in the UI as a notification to users.
    /// </summary>
    public class Flash : PresentationObject
    {
        private bool closed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Flash"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="additionalData">The additional data.</param>
        /// <param name="closeable">if set to <c>true</c> [closeable].</param>
        /// <param name="expiry">The expiry.</param>
        public Flash(string message, object additionalData, bool closeable, TimeSpan? expiry)
        {
            Message = message;
            AdditionalData = additionalData;
            Closeable = closeable;
            Expiry = expiry;
            Close = new RelayCommand(CloseExecuted);
        }

        /// <summary>
        /// Gets a command to invoke to close the flash.
        /// </summary>
        /// <value>The close.</value>
        public ICommand Close { get; private set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets or sets the additional data.
        /// </summary>
        /// <value>The additional data.</value>
        public object AdditionalData { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Flash"/> is closeable.
        /// </summary>
        /// <value><c>true</c> if closeable; otherwise, <c>false</c>.</value>
        public bool Closeable { get; private set; }

        /// <summary>
        /// Gets or sets the expiry.
        /// </summary>
        /// <value>The expiry.</value>
        public TimeSpan? Expiry { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Flash"/> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        public bool Closed
        {
            get { return closed; }
            set { closed = value; NotifyChanged("Closed"); }
        }

        private void CloseExecuted()
        {
            Closed = true;
        }
    }
}