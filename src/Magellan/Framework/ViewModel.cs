using System;
using System.ComponentModel;
using Magellan.Abstractions;

namespace Magellan.Framework
{
    /// <summary>
    /// A base class for view models in the MVVM pattern.
    /// </summary>
    public abstract class ViewModel : PresentationObject, IViewAware, INavigationAware, IFlasher
    {
        private object view;
        private FlashCollection flashes;
        private IScheduler scheduler = new Scheduler();
        private BackgroundOperationCollection operations;
        private IDispatcher dispatcher;

        /// <summary>
        /// Gets a collection of background thread operations, which also allows queuing of new operations.
        /// </summary>
        /// <value>The operations.</value>
        public BackgroundOperationCollection Operations
        {
            get
            {
                return operations = operations ?? new BackgroundOperationCollection(BusyState, Dispatcher);
            }
        }

        /// <summary>
        /// Gets a collection of user notification messages.
        /// </summary>
        /// <value>The flashes.</value>
        public FlashCollection Flashes
        {
            get { return flashes ?? (flashes = new FlashCollection(Scheduler)); }
        }

        /// <summary>
        /// Gets a scheduler for executing tasks after a short delay.
        /// </summary>
        /// <value>The scheduler.</value>
        public IScheduler Scheduler
        {
            get { return scheduler ?? (scheduler = new Scheduler()); }
            set { scheduler = value; }
        }

        /// <summary>
        /// Flashes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Flash(string message)
        {
            Flash(new Flash(message, null, true, null));
        }

        /// <summary>
        /// Flashes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="disappears">The disappears.</param>
        public void Flash(string message, TimeSpan disappears)
        {
            Flash(new Flash(message, null, true, disappears));
        }

        /// <summary>
        /// Flashes the specified flash.
        /// </summary>
        /// <param name="flash">The flash.</param>
        public void Flash(Flash flash)
        {
            Flashes.Add(flash);
        }

        /// <summary>
        /// Clears the flashes.
        /// </summary>
        public void ClearFlashes()
        {
            Flashes.Clear();
        }

        /// <summary>
        /// Gets the dispatcher that owns this presentation object.
        /// </summary>
        /// <value>The dispatcher.</value>
        public IDispatcher Dispatcher
        {
            get { return dispatcher = dispatcher ?? Navigator.Dispatcher; }
            set { dispatcher = value; }
        }

        /// <summary>
        /// Gets or sets the navigator that can be used for performing subsequent navigation actions.
        /// </summary>
        /// <value></value>
        public INavigator Navigator { get; set; }

        /// <summary>
        /// Gets the View that is attached to this ViewModel.
        /// </summary>
        /// <value>The view.</value>
        protected object View
        {
            get { return view; }
        }

        /// <summary>
        /// Called when a view has been attached (bound) to this ViewModel.
        /// </summary>
        /// <param name="view">The view.</param>
        protected virtual void ViewAttached(object view)
        {
        }

        /// <summary>
        /// Called when the view attached to this ViewModel raises the Loaded event. For a Window, this will be called 
        /// once. For a Page, it may be called multiple times, for example, if the user navigates to the page, then clicks
        /// 'Back', then clicks 'Forward', returning to the page.  
        /// </summary>
        protected virtual void Activated()
        {
        }

        /// <summary>
        /// Called when the view attached to this ViewModel is being closed, allowing for cancellation. For a Window, this 
        /// maps to the Closing event. For a Page, it is the Navigating event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void Deactivating(CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when the view attached to this ViewModel is closed. For a Window, this maps to the Closed event. For a 
        /// Page, it maps to the Navigated event, when the page is unloaded. Remember that in the case of pages, if the user 
        /// clicks 'Back' and then 'Forward', the view will be re-activated and thus re-deactivated.
        /// </summary>
        protected virtual void Deactivated()
        {
        }

        void IViewAware.ViewAttached(object view)
        {
            this.view = view;
            ViewAttached(view);
        }

        void IViewAware.Activated()
        {
            Activated();
        }

        void IViewAware.Deactivating(CancelEventArgs e)
        {
            Deactivating(e);
        }

        void IViewAware.Deactivated()
        {
            Deactivated();
        }
    }
}
