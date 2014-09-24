namespace Magellan.Transitionals
{
    /// <summary>
    /// Provides a well-known registry of default navigation transitions.
    /// </summary>
    public static class NavigationTransitions
    {
        private static readonly NavigationTransitionRegistry table = new NavigationTransitionRegistry();

        /// <summary>
        /// Initializes the <see cref="NavigationTransitions"/> class.
        /// </summary>
        static NavigationTransitions()
        {
        }

        /// <summary>
        /// Gets the table of transitions that are currently registered.
        /// </summary>
        public static NavigationTransitionRegistry Table
        {
            get { return table; }
        }
    }
}