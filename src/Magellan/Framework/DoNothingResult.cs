using Magellan.Diagnostics;

namespace Magellan.Framework
{
    /// <summary>
    /// An action result that does nothing.
    /// </summary>
    public class DoNothingResult : ActionResult
    {
        /// <summary>
        /// Executes the action result.
        /// </summary>
        /// <param name="controllerContext"></param>
        protected override void ExecuteInternal(ControllerContext controllerContext)
        {
            TraceSources.MagellanSource.TraceInformation("DoNothingResult is rendering for request '{0}'.", controllerContext.Request);
        }
    }
}
