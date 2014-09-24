using System.ComponentModel;
using Magellan.Behaviors;
using Magellan.Routing;

namespace Magellan.Transitionals.Behaviors
{
    /// <summary>
    /// A Blend Trigger Action that makes it easy to navigate to actions on controllers using a transition.
    /// </summary>
    public class NavigateWithTransitionAction : NavigateControllerAction
    {
        /// <summary>
        /// Gets or sets the name of the transition to play when navigating between pages.
        /// </summary>
        /// <value>The transition.</value>
        [Category("Navigation")]
        [Description("The name of the transition to play when navigating between pages.")]
        public string Transition { get; set; }

        /// <summary>
        /// Prepares the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void PrepareRequest(RouteValueDictionary request)
        {
            request.Add("Transition", Transition);
            base.PrepareRequest(request);
        }
    }
}