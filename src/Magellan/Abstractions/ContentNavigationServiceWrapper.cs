using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Magellan.Abstractions
{
    /// <summary>
    /// An implementation of <see cref="INavigationService"/> that wraps a content control.
    /// </summary>
    public class ContentNavigationServiceWrapper : INavigationService
    {
        private readonly ContentControl frame;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentNavigationServiceWrapper"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public ContentNavigationServiceWrapper(ContentControl content)
        {
            frame = content;
        }

        /// <summary>
        /// Occurs when the navigation service is navigating. The <see cref="Content"/> property will contain the previous
        /// (current) page.
        /// </summary>
        public event CancelEventHandler Navigating;

        /// <summary>
        /// Occurs when the navigation service has completed navigating. The <see cref="Content"/> property will
        /// contain the new content.
        /// </summary>
        public event EventHandler Navigated;

        /// <summary>
        /// Gets the value of a dependency property from the underlying wrapped navigation service.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public object GetValue(DependencyProperty property)
        {
            return frame.GetValue(property);
        }

        /// <summary>
        /// Sets the value of a dependency property on the underlying navigation service.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        public void SetValue(DependencyProperty property, object value)
        {
            frame.SetValue(property, value);
        }

        /// <summary>
        /// Gets a value indicating whether the back button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go back; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoBack
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the forward button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go forward; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoForward
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the current content.
        /// </summary>
        /// <value>The content.</value>
        public object Content
        {
            get { return frame.Content; }
        }

        /// <summary>
        /// Navigates to the last page in the browser journal.
        /// </summary>
        public void GoBack()
        {
        }

        /// <summary>
        /// Navigates to the last page in the browser journal and removes the current page from the journal.
        /// </summary>
        /// <param name="removeFromJournal">
        /// if set to <c>true</c> the current page will be removed from the journal.
        /// </param>
        public void GoBack(bool removeFromJournal)
        {
        }

        /// <summary>
        /// Navigates to the next page in the browser journal.
        /// </summary>
        public void GoForward()
        {
        }

        /// <summary>
        /// Navigates the specified content using a navigation state.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="navigationState">State of the navigation.</param>
        /// <returns></returns>
        public bool NavigateDirectToContent(object root, object navigationState)
        {
            frame.Content = root;
            return true;
        }

        /// <summary>
        /// Gets the dispatcher associated with this navigation service.
        /// </summary>
        /// <value>The dispatcher.</value>
        public IDispatcher Dispatcher
        {
            get { return new DispatcherWrapper(frame.Dispatcher); }
        }

        /// <summary>
        /// Resets the navigation history by removing all 'back' entries from the navigation journal.
        /// </summary>
        public void ResetHistory()
        {
        }

        /// <summary>
        /// Raises the <see cref="Navigating"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnNavigating(CancelEventArgs e)
        {
            var handler = Navigating;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Navigated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnNavigated(EventArgs e)
        {
            var handler = Navigated;
            if (handler != null) handler(this, e);
        }
    }
}