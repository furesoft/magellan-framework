using System.Collections.Generic;
using System.Windows.Navigation;
using Magellan.Routing;
using Transitionals;

namespace Magellan.Transitionals
{
    /// <summary>
    /// This object is applied to a TransitionElement. It monitors a NavigationService and uses the navigation events to track state 
    /// and select an appropriate transition.
    /// </summary>
    public class NavigationTransitionSelector : TransitionSelector
    {
        private readonly NavigationService navigationService;
        private readonly NavigationTransitionRegistry transitionRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationTransitionSelector"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="transitionRegistry">The transition registry.</param>
        public NavigationTransitionSelector(NavigationService navigationService, NavigationTransitionRegistry transitionRegistry)
        {
            BackStack = new Stack<NavigationTransition>();
            ForwardStack = new Stack<NavigationTransition>();
            this.transitionRegistry = transitionRegistry;
            this.navigationService = navigationService;
            this.navigationService.Navigating += HandleContentNavigating;
            this.navigationService.Navigated += HandleContentNavigated;
        }

        /// <summary>
        /// Gets or sets the back stack.
        /// </summary>
        /// <value>The back stack.</value>
        protected internal Stack<NavigationTransition> BackStack { get; private set; }

        /// <summary>
        /// Gets or sets the forward stack.
        /// </summary>
        /// <value>The forward stack.</value>
        protected internal Stack<NavigationTransition> ForwardStack { get; private set; }

        /// <summary>
        /// Gets or sets the current transition.
        /// </summary>
        /// <value>The current transition.</value>
        protected internal NavigationTransition CurrentTransition { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is navigating.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is navigating; otherwise, <c>false</c>.
        /// </value>
        protected bool IsNavigating { get; private set; }

        /// <summary>
        /// Selects the transition based on the last navigation action that occurred.
        /// </summary>
        /// <param name="oldContent">The old content.</param>
        /// <param name="newContent">The new content.</param>
        public override Transition SelectTransition(object oldContent, object newContent)
        {
            return CurrentTransition != null ? CurrentTransition.CreateTransition() : null;
        }

        private void HandleContentNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (IsNavigating)
                return;
            IsNavigating = true;

            // Choose the name of the transition based on the e.ExtraData that was passed to NavigationService.Navigate(). 
            // If the data is a string, we'll try to find a matching transition by that name. 
            if (e.ExtraData is ResolvedNavigationRequest)
            {
                var transitionName = ((ResolvedNavigationRequest)e.ExtraData).RouteValues.GetOrDefault<string>("Transition");
                CurrentTransition = transitionRegistry.Get(transitionName);
            }
            else if (e.ExtraData is string && transitionRegistry.Get(e.ExtraData.ToString()) != null)
            {
                var transitionName = e.ExtraData.ToString();
                CurrentTransition = transitionRegistry.Get(transitionName);
            }
            else
            {
                CurrentTransition = null;
            }

            // Keep a record of back/forward navigation and use this to select the reverse transitions.
            if (e.NavigationMode == NavigationMode.New)
            {
                BackStack.Push(CurrentTransition);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                var original = BackStack.Pop();
                ForwardStack.Push(original);
                var reverseTransition = original == null ? null : transitionRegistry.Get(original.Reverse);
                CurrentTransition = reverseTransition;
            }
            else if (e.NavigationMode == NavigationMode.Forward)
            {
                CurrentTransition = ForwardStack.Pop();
                BackStack.Push(CurrentTransition);
            }
        }

        private void HandleContentNavigated(object sender, NavigationEventArgs e)
        {
            IsNavigating = false;
            CurrentTransition = null;
        }
    }
}