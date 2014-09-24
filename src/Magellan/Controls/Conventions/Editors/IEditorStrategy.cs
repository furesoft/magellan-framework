namespace Magellan.Controls.Conventions.Editors
{
    /// <summary>
    /// Implemented by objects which create editors for a field.
    /// </summary>
    public interface IEditorStrategy
    {
        /// <summary>
        /// Creates the appropriate editor from the context. If the return value is null, another 
        /// <see cref="IEditorStrategy"/> will be asked. The returned value should be fully configured 
        /// and bound.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A control that will be used for editing the field, or null if this strategy cannot be 
        /// used for this field.</returns>
        object CreateEditor(FieldContext context);
    }
}
