using System.Diagnostics;
using Magellan.Diagnostics;

namespace Magellan.Framework
{
    /// <summary>
    /// An <see cref="ActionResult"/> that creates a new process.
    /// </summary>
    public class StartProcessResult : ActionResult
    {
        private readonly ProcessStartInfo startInfo;
        private readonly bool waitForExit;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartProcessResult"/> class.
        /// </summary>
        /// <param name="startInfo">The start info.</param>
        /// <param name="waitForExit">if set to <c>true</c> this result should wait for the started process 
        /// to exit.</param>
        public StartProcessResult(ProcessStartInfo startInfo, bool waitForExit)
        {
            this.startInfo = startInfo;
            this.waitForExit = waitForExit;
        }

        /// <summary>
        /// Gets the startup arguments for the process.
        /// </summary>
        /// <value>The start info.</value>
        public ProcessStartInfo StartInfo
        {
            get { return startInfo; }
        }

        /// <summary>
        /// Gets a value indicating whether this result should wait for the started process to exit.
        /// </summary>
        /// <value><c>true</c> if this result should wait for the started process to exit; 
        /// otherwise, <c>false</c>.</value>
        public bool WaitForExit
        {
            get { return waitForExit; }
        }

        /// <summary>
        /// Gets the started process (available after render).
        /// </summary>
        /// <value>The process.</value>
        public Process StartedProcess { get; private set; }

        /// <summary>
        /// When implemented in a derived class, performs the bulk of the action rendering.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        protected override void ExecuteInternal(ControllerContext controllerContext)
        {
            TraceSources.MagellanSource.TraceInformation("The StartProcessResult is launching the process '{0}'.", startInfo.FileName);
            StartedProcess = Process.Start(startInfo);
            if (waitForExit)
            {
                TraceSources.MagellanSource.TraceInformation("The StartProcessResult is waiting for the process '{0}' to exit.", startInfo.FileName);
                StartedProcess.WaitForExit();
            }
        }
    }
}
