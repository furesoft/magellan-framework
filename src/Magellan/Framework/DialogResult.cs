namespace Magellan.Framework
{
    /// <summary>
    /// A <see cref="ViewResult"/> specifically for showing modal windows.
    /// </summary>
    public class DialogResult : ViewResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model that will be bound to the view.</param>
        /// <param name="viewEngines">The view engines.</param>
        public DialogResult(string viewName, object model, ViewEngineCollection viewEngines)
            : base(viewName, model, viewEngines)
        {
            Options.SetViewType("Dialog");
        }
    }
}