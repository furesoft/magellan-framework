namespace Magellan.Framework
{
    /// <summary>
    /// A base class for <see cref="Controller">controllers</see> that are executed asynchronously. An 
    /// alternative to using this is to manually set the controller's <see cref="ActionInvoker"/> property
    /// to an instance of an <see cref="AsyncActionInvoker"/>, or use the 
    /// <see cref="AsyncControllerFactory"/>.
    /// </summary>
    public class AsyncController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncController"/> class.
        /// </summary>
        public AsyncController()
        {
            ActionInvoker = new AsyncActionInvoker();
        }
    }
}
