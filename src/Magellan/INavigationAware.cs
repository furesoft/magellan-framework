namespace Magellan
{
    /// <summary>
    /// Implemented by views and models that wish to be aware of the current navigation context. 
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Gets or sets the navigator that can be used for performing subsequent navigation actions.
        /// </summary>
        INavigator Navigator { get; set; }
    }
}
