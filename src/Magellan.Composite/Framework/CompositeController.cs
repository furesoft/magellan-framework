using Magellan.Framework;

namespace Magellan.Composite.Framework
{
    /// <summary>
    /// A base class for controllers that make use of Composite WPF (Prism).
    /// </summary>
    public class CompositeController : Controller
    {
        /// <summary>
        /// Returns a <see cref="ViewResult"/> that specifies to display a composite WPF view with the same
        /// name as this action.
        /// </summary>
        /// <returns>A <see cref="CompositeViewResult"/>.</returns>
        protected CompositeViewResult CompositeView()
        {
            return CompositeView(null);
        }

        /// <summary>
        /// Returns a <see cref="ViewResult"/> that specifies to display a composite WPF view with the same
        /// name as this action.
        /// </summary>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="CompositeViewResult"/>.</returns>
        protected CompositeViewResult CompositeView(object model)
        {
            return CompositeView(null, model);
        }

        /// <summary>
        /// Returns a <see cref="ViewResult"/> that specifies to display a composite WPF view.
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for
        /// example, a <paramref name="viewName"/> of "Add" will match "AddView".</param>
        /// <returns>A <see cref="CompositeViewResult"/>.</returns>
        protected CompositeViewResult CompositeView(string viewName)
        {
            return CompositeView(viewName, null);
        }

        /// <summary>
        /// Returns a <see cref="ViewResult"/> that specifies to display a composite WPF view.
        /// </summary>
        /// <param name="viewName">The name of the view. Conventions will automatically add suffixes - for
        /// example, a <paramref name="viewName"/> of "Add" will match "AddView".</param>
        /// <param name="model">The view model that will be set as the DataContext for the view.</param>
        /// <returns>A <see cref="CompositeViewResult"/>.</returns>
        protected CompositeViewResult CompositeView(string viewName, object model)
        {
            return new CompositeViewResult(viewName, model, ViewEngines);
        }
    }
}
