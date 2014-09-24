namespace Magellan.Framework
{
    /// <summary>
    /// Implemented by objects that store validation messages assocated to keys (properties).
    /// </summary>
    public interface IStoreValidationMessages
    {
        /// <summary>
        /// Gets the validation messages associated with this object.
        /// </summary>
        /// <value>The validation messages.</value>
        ValidationMessageDictionary ValidationMessages { get; }
    }
}