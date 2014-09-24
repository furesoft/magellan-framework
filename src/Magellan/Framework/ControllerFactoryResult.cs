using System;

namespace Magellan.Framework
{
    /// <summary>
    /// Returned by controller factories to provide the controller and a custom cleanup action that will be 
    /// invoked after the controller action has been executed.
    /// </summary>
    public class ControllerFactoryResult : IDisposable
    {
        private readonly Action releaseCallback;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerFactoryResult"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="releaseCallback">The cleanup callback. Can be null to do nothing.</param>
        public ControllerFactoryResult(IController controller, Action releaseCallback)
        {
            this.releaseCallback = releaseCallback;
            Controller = controller;
        }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public virtual IController Controller { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (releaseCallback != null) releaseCallback();
        }
    }
}