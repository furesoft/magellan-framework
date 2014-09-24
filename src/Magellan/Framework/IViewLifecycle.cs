
using System.ComponentModel;

namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by <see cref="ViewModel">view models</see> and other <see cref="PresentationObject">
    /// presentation objects</see> that are interested in knowing when the view they are bound to is 
    /// connected.
    /// </summary>
    public interface IViewAware
    {
        /// <summary>
        /// Notifies the target that a view has been attached.
        /// </summary>
        /// <param name="view">The view.</param>
        void ViewAttached(object view);

        /// <summary>
        /// Notifies the target that the view is going to be activated.
        /// </summary>
        void Activated();

        /// <summary>
        /// Notifies the target that the view is being deactivated, giving the target a chance to 
        /// cancel deactivation.
        /// </summary>
        void Deactivating(CancelEventArgs e);

        /// <summary>
        /// Notifies the target that the view is no longer active.
        /// </summary>
        void Deactivated();
    }
}