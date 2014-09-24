namespace Magellan.Framework
{
    /// <summary>
    /// A <see cref="ViewResult"/> that represents a child window that will be opened.
    /// </summary>
    public class ChildWindowResult : ViewResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildWindowResult"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model that will be bound to the view.</param>
        /// <param name="viewEngines">The set of view engines that can help to activate this view.</param>
        public ChildWindowResult(string viewName, object model, ViewEngineCollection viewEngines) 
            : base(viewName, model, viewEngines)
        {
            Options.SetViewType("ChildWindow");
        }
    }
}