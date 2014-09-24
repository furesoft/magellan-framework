using System;
using System.ComponentModel;
using System.Windows;

namespace Magellan.Abstractions
{
    /// <summary>
    /// A service that abstracts the WPF navigation service while providing the same public interface.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Occurs when the navigation service is navigating. The <see cref="Content"/> property will contain the previous
        /// (current) page. 
        /// </summary>
        event CancelEventHandler Navigating;

        /// <summary>
        /// Occurs when the navigation service has completed navigating. The <see cref="Content"/> property will 
        /// contain the new content.
        /// </summary>
        event EventHandler Navigated;

        /// <summary>
        /// Gets the value of a dependency property from the underlying wrapped navigation service.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        object GetValue(DependencyProperty property);

        /// <summary>
        /// Sets the value of a dependency property on the underlying navigation service.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        void SetValue(DependencyProperty property, object value);

        /// <summary>
        /// Gets a value indicating whether the back button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go back; otherwise, <c>false</c>.
        /// </value>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether the forward button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go forward; otherwise, <c>false</c>.
        /// </value>
        bool CanGoForward { get; }

        /// <summary>
        /// Navigates to the last page in the browser journal.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Navigates to the last page in the browser journal and removes the current page from the journal.
        /// </summary>
        /// <param name="removeFromJournal">if set to <c>true</c> the current page will be removed from 
        /// the journal.</param>
        void GoBack(bool removeFromJournal);

        /// <summary>
        /// Navigates to the next page in the browser journal.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Gets the current content.
        /// </summary>
        /// <value>The content.</value>
        object Content { get; }

        /// <summary>
        /// Navigates the specified content using a navigation state.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="navigationState">State of the navigation.</param>
        /// <returns></returns>
        bool NavigateDirectToContent(object root, object navigationState);

        /// <summary>
        /// Gets the dispatcher associated with this navigation service.
        /// </summary>
        /// <value>The dispatcher.</value>
        IDispatcher Dispatcher { get; }

        /// <summary>
        /// Resets the navigation history by removing all 'back' entries from the navigation journal.
        /// </summary>
        void ResetHistory();
    }
}