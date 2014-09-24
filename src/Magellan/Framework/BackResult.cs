using Magellan.Diagnostics;

namespace Magellan.Framework
{
    /// <summary>
    /// Represents a request to go to the previous page, optionally removing the current page from the WPF 
    /// navigation journal.
    /// </summary>
    public class BackResult : ActionResult
    {
        private readonly bool removeFromJournal;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackResult"/> class.
        /// </summary>
        /// <param name="removeFromJournal">if set to <c>true</c> [remove from journal].</param>
        public BackResult(bool removeFromJournal)
        {
            this.removeFromJournal = removeFromJournal;
        }

        /// <summary>
        /// When implemented in a derived class, performs the bulk of the action rendering.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        protected override void ExecuteInternal(ControllerContext controllerContext)
        {
            TraceSources.MagellanSource.TraceInformation("BackResult is rendering for request '{0}'. RemoveFromJournal is {1}", controllerContext.Request, removeFromJournal);

            var dispatcher = controllerContext.Request.Navigator.Dispatcher;
            dispatcher.Dispatch(
                delegate
                {
                    var navigationService = controllerContext.Request.Navigator;
                    navigationService.GoBack(removeFromJournal);
                });
        }
    }
}