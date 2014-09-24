namespace Magellan.Framework
{
    /// <summary>
    /// Represents a <see cref="ViewResult"/> that specifies a non-modal window to show.
    /// </summary>
    public class WindowResult : ViewResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model that will be bound to the view.</param>
        /// <param name="viewEngines">The view engines.</param>
        public WindowResult(string viewName, object model, ViewEngineCollection viewEngines)
            : base(viewName, model, viewEngines)
        {
            Options.SetViewType("Window");
        }
    }
}