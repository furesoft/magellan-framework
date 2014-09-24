using System.ComponentModel;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// An interface implemented by objects that know how to prepare a view and view model for a navigation 
    /// request.
    /// </summary>
    public interface IViewInitializer
    {
        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        /// <param name="request">The request.</param>
        void Prepare(object view, object model, ResolvedNavigationRequest request);

        /// <summary>
        /// Notifies the view that it has been activated.
        /// </summary>
        /// <param name="view">The view.</param>
        void NotifyActivated(object view);

        /// <summary>
        /// Notifies the view that it is being deactivated.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        void NotifyDeactivating(object view, CancelEventArgs e);

        /// <summary>
        /// Notifies the view that it has been deactivated.
        /// </summary>
        /// <param name="view">The view.</param>
        void NotifyDeactivated(object view);
    }
}