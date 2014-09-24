namespace Magellan.Views
{
    /// <summary>
    /// Implemented by views that wish to make use of a custom model property instead of DataContext.
    /// </summary>
    public interface IModelBound
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        object Model { get; set; }
    }
}
