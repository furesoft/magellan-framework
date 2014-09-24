namespace Magellan.Framework
{
    public class BackResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackResult"/> class.
        /// </summary>
        public BackResult()
        {
        }

        /// <summary>
        /// When implemented in a derived class, performs the bulk of the action rendering.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        protected override void ExecuteInternal(ControllerContext controllerContext)
        {
            var navigator = controllerContext.Request.Navigator;
            navigator.GoBack();
        }
    }
}