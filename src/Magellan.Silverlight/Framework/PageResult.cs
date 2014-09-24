namespace Magellan.Framework
{
    /// <summary>
    /// Represents a <see cref="ViewResult"/> for a Silverlight page object.
    /// </summary>
    public class PageResult : ViewResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageResult"/> class.
        /// </summary>
        /// <param name="publicUri">The public URI, such as /Home, which will be given to the Silverlight navigation framework.</param>
        /// <param name="model">The model.</param>
        /// <param name="viewEngines">The view engines.</param>
        public PageResult(string publicUri, object model, ViewEngineCollection viewEngines)
            : base(publicUri, model, viewEngines)
        {
            Options.SetViewType("Page");
        }
    }
}