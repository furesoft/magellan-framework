using System;
using Transitionals;

namespace Magellan.Transitionals
{
    /// <summary>
    /// Represents an item registered in the <see cref="NavigationTransitionRegistry" />.
    /// </summary>
    public class NavigationTransition
    {
        private readonly string name;
        private readonly string reverse;
        private readonly Func<Transition> transitionBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationTransition"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="reverse">The reverse.</param>
        /// <param name="transitionBuilder">The transition builder.</param>
        public NavigationTransition(string name, string reverse, Func<Transition> transitionBuilder)
        {
            this.name = name;
            this.reverse = reverse;
            this.transitionBuilder = transitionBuilder;
        }

        /// <summary>
        /// Gets the name of this transition.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the name of the reverse transition.
        /// </summary>
        public string Reverse
        {
            get { return reverse; }
        }

        /// <summary>
        /// Creates an instance of the transition.
        /// </summary>
        public virtual Transition CreateTransition()
        {
            return transitionBuilder();
        }
    }
}