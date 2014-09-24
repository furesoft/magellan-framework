using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Data;

namespace Magellan.Controls.Conventions.Editors
{
    /// <summary>
    /// The default fallback implementation of an <see cref="IEditorStrategy"/>. If no other strategy can 
    /// figure out how to edit a field, this editor makes a good fallback.
    /// </summary>
    public class TextBoxEditorStrategy : IEditorStrategy
    {
        /// <summary>
        /// Creates the appropriate editor from the context. If the return value is null, another
        /// <see cref="IEditorStrategy"/> will be asked. The returned value should be fully configured
        /// and bound.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A control that will be used for editing the field, or null if this strategy cannot be
        /// used for this field.
        /// </returns>
        public object CreateEditor(FieldContext context)
        {
            var textBox = new TextBox { Width = 200 };
            context.WhenHasAttribute<StringLengthAttribute>(sl => textBox.MaxLength = sl.MaximumLength);

            BindingOperations.SetBinding(textBox, TextBox.TextProperty, context.Binding);
            return textBox;
        }
    }
}