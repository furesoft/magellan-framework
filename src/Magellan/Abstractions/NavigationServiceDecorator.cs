using System;
using System.ComponentModel;
using System.Windows;

namespace Magellan.Abstractions
{
    /// <summary>
    /// A base class that can be used when implementing <see cref="INavigationService"/> and forwarding 
    /// calls to an inner <see cref="INavigationService"/>.
    /// </summary>
    public class NavigationServiceDecorator : INavigationService
    {
        private readonly Func<INavigationService> innerAccessor;
        private INavigationService inner;
        private readonly object sync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationServiceDecorator"/> class.
        /// </summary>
        /// <param name="inner">The inner navigation service.</param>
        public NavigationServiceDecorator(Func<INavigationService> inner)
        {
            innerAccessor = inner;
        }

        private INavigationService Inner
        {
            get
            {
                lock (sync)
                {
                    return inner ?? (inner = innerAccessor());
                }
            }
        }

        /// <summary>
        /// Occurs when the navigation service is navigating. The <see cref="Content"/> property will contain the previous
        /// (current) page.
        /// </summary>
        public event CancelEventHandler Navigating
        {
            add { Inner.Navigating += value; }
            remove { Inner.Navigating -= value; }
        }

        /// <summary>
        /// Occurs when the navigation service has completed navigating. The <see cref="Content"/> property will
        /// contain the new content.
        /// </summary>
        public event EventHandler Navigated
        {
            add { Inner.Navigated += value; }   
            remove { Inner.Navigated -= value; }   
        }

        /// <summary>
        /// Gets the value of a dependency property from the underlying wrapped navigation service.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public object GetValue(DependencyProperty property)
        {
            return Inner.GetValue(property);
        }

        /// <summary>
        /// Sets the value of a dependency property on the underlying navigation service.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        public void SetValue(DependencyProperty property, object value)
        {
            Inner.SetValue(property, value);
        }

        /// <summary>
        /// Gets a value indicating whether the back button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go back; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanGoBack
        {
            get { return Inner.CanGoBack; }
        }

        /// <summary>
        /// Gets a value indicating whether the forward button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go forward; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanGoForward
        {
            get { return Inner.CanGoForward; }
        }

        /// <summary>
        /// Navigates to the last page in the browser journal.
        /// </summary>
        public virtual void GoBack()
        {
            Inner.GoBack();
        }

        /// <summary>
        /// Navigates to the last page in the browser journal and removes the current page from the journal.
        /// </summary>
        /// <param name="removeFromJournal">if set to <c>true</c> the current page will be removed from
        /// the journal.</param>
        public virtual void GoBack(bool removeFromJournal)
        {
            Inner.GoBack(removeFromJournal);
        }

        /// <summary>
        /// Navigates to the next page in the browser journal.
        /// </summary>
        public virtual void GoForward()
        {
            Inner.GoForward();
        }

        /// <summary>
        /// Gets the current content.
        /// </summary>
        /// <value>The content.</value>
        public object Content
        {
            get { return Inner.Content; }
        }

        /// <summary>
        /// Navigates the specified content using a navigation state.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="navigationState">State of the navigation.</param>
        /// <returns></returns>
        public virtual bool NavigateDirectToContent(object root, object navigationState)
        {
            return Inner.NavigateDirectToContent(root, navigationState);
        }

        /// <summary>
        /// Gets the dispatcher associated with this navigation service.
        /// </summary>
        /// <value>The dispatcher.</value>
        public virtual IDispatcher Dispatcher
        {
            get { return Inner.Dispatcher; }
        }

        /// <summary>
        /// Resets the navigation history by removing all 'back' entries from the navigation journal.
        /// </summary>
        public virtual void ResetHistory()
        {
            Inner.ResetHistory();
        }
    }
}