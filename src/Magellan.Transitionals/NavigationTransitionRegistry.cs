using System;
using System.Collections.Generic;
using System.Linq;
using Transitionals;

namespace Magellan.Transitionals
{
    /// <summary>
    /// Stores a list of transitions and the reverse of each transition.
    /// </summary>
    public class NavigationTransitionRegistry
    {
        private readonly List<NavigationTransition> transitions = new List<NavigationTransition>();

        /// <summary>
        /// Gets a transition by the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public NavigationTransition Get(string name)
        {
            return transitions.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Adds a transition with a name and the name of the transition to play when reversing.
        /// </summary>
        /// <param name="name">The name of this transition.</param>
        /// <param name="reverseName">The name of the transition to play when reversing.</param>
        /// <param name="transitionBuilder">A callback that will instantiate the transition.</param>
        public void Add(string name, string reverseName, Func<Transition> transitionBuilder)
        {
            transitions.Add(new NavigationTransition(name, reverseName, transitionBuilder));
        }
    }
}