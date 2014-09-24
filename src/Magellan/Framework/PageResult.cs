namespace Magellan.Framework
{
    /// <summary>
    /// Represents a <see cref="ViewResult"/> for a WPF page object.
    /// </summary>
    public class PageResult : ViewResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model that will be bound to the view.</param>
        /// <param name="viewEngines">The view engines.</param>
        public PageResult(string viewName, object model, ViewEngineCollection viewEngines)
            : base(viewName, model, viewEngines)
        {
            Options.SetViewType("Page");
        }

        /// <summary>
        /// When the page is rendered, any previous journal entries will be evicted from the journal.
        /// </summary>
        /// <returns></returns>
        public PageResult ClearNavigationHistory()
        {
            Options.SetResetHistory(true);
            return this;
        }
    }
}