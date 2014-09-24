namespace Magellan.Framework
{
    /// <summary>
    /// Represents the typical result of invoking an action on a controller.
    /// </summary>
    public abstract class ActionResult
    {
        /// <summary>
        /// Performs all actions necessary to realize the navigation result.
        /// </summary>
        /// <param name="controllerContext"></param>
        public void Execute(ControllerContext controllerContext)
        {
            ExecuteInternal(controllerContext);
        }

        /// <summary>
        /// When implemented in a derived class, performs the bulk of the action rendering.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        protected abstract void ExecuteInternal(ControllerContext controllerContext);
    }
}