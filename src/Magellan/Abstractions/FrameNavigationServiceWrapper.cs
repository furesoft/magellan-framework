using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Magellan.Abstractions
{
    /// <summary>
    /// Wraps the WPF <see cref="NavigationService"/> and allows it to implement the 
    /// <see cref="INavigationService"/> interface.
    /// </summary>
    public class FrameNavigationServiceWrapper : INavigationService
    {
        private readonly Dispatcher dispatcher;
        private readonly Frame frame;
        private readonly NavigationService navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNavigationServiceWrapper"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="frame">The frame.</param>
        public FrameNavigationServiceWrapper(Dispatcher dispatcher, Frame frame)
        {
            this.dispatcher = dispatcher;
            this.frame = frame;
            navigationService = this.frame.NavigationService;
            navigationService.Navigating += NavigationServiceNavigating;
            navigationService.Navigated += NavigationServiceNavigated;
        }

        private void NavigationServiceNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var handler = Navigating;
            if (handler != null) handler(this, e);
        }

        private void NavigationServiceNavigated(object sender, NavigationEventArgs e)
        {
            var handler = Navigated;
            if (handler != null) handler(this, e);
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
            get { return navigationService.CanGoBack; }
        }

        /// <summary>
        /// Gets a value indicating whether the forward button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go forward; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoForward
        {
            get { return navigationService.CanGoForward; }
        }

        /// <summary>
        /// Navigates to the last page in the browser journal.
        /// </summary>
        public void GoBack()
        {
            navigationService.GoBack();
        }

        /// <summary>
        /// Navigates to the last page in the browser journal and removes the current page from the journal.
        /// </summary>
        /// <param name="removeFromJournal">if set to <c>true</c> the current page will be removed from
        /// the journal.</param>
        public void GoBack(bool removeFromJournal)
        {
            navigationService.GoBack();
            if (removeFromJournal)
            {
                navigationService.RemoveBackEntry();
            }
        }

        /// <summary>
        /// Navigates to the next page in the browser journal.
        /// </summary>
        public void GoForward()
        {
            navigationService.GoForward();
        }

        /// <summary>
        /// Gets the current content.
        /// </summary>
        /// <value>The content.</value>
        public object Content
        {
            get { return navigationService.Content; }
        }

        /// <summary>
        /// Navigates to the specified content.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public bool NavigateDirectToContent(object root)
        {
            var result =navigationService.Navigate(root);
            CommandManager.InvalidateRequerySuggested();
            return result;
        }

        /// <summary>
        /// Navigates the specified content using a navigation state.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="navigationState">State of the navigation.</param>
        /// <returns></returns>
        public bool NavigateDirectToContent(object root, object navigationState)
        {
            var result = navigationService.Navigate(root, navigationState);
            CommandManager.InvalidateRequerySuggested();
            return result;
        }

        /// <summary>
        /// Gets the dispatcher associated with this navigation service.
        /// </summary>
        /// <value>The dispatcher.</value>
        public IDispatcher Dispatcher
        {
            get
            {
                return new DispatcherWrapper(dispatcher);
            }
        }

        private void DoEvents()
        {
            var frame = new DispatcherFrame(true);
            var dispatcherTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(100),
                DispatcherPriority.Normal,
                (x, y) => { frame.Continue = false; ((DispatcherTimer)x).Stop(); },
                dispatcher
                );
            dispatcherTimer.Start();
            System.Windows.Threading.Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// Resets the navigation history by removing all 'back' entries from the navigation journal.
        /// </summary>
        public void ResetHistory()
        {
            DoEvents();
            while (navigationService.CanGoBack)
            {
                navigationService.RemoveBackEntry();
            }
        }
    }
}