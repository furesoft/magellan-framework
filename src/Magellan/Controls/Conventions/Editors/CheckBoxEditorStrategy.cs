using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Magellan.Controls.Conventions.Editors
{
    /// <summary>
    /// An <see cref="IEditorStrategy">editor strategy</see> that creates CheckBoxes for boolean properties.
    /// </summary>
    public class CheckBoxEditorStrategy : IEditorStrategy
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
            if (context.PropertyDescriptor.PropertyType != typeof (bool))
            {
                return null;
            }
            
            // Hide the header:
            context.Field.Header = "";

            var displayName = context.PropertyDescriptor.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

            var checkBox = new CheckBox();
            checkBox.Content = displayName == null ? context.PropertyName : displayName.DisplayName;
            BindingOperations.SetBinding(checkBox, ToggleButton.IsCheckedProperty, context.Binding);
            return checkBox;
        }
    }
}