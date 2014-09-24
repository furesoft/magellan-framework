using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Magellan.ComponentModel;

namespace Magellan.Controls.Conventions.Editors
{
    /// <summary>
    /// An <see cref="IEditorStrategy">editor strategy</see> that creates a combo box for enum types.
    /// </summary>
    public class ComboBoxEditorStrategy : IEditorStrategy
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
            if (!context.PropertyDescriptor.PropertyType.IsEnum)
                return null;

            var enumType = context.PropertyDescriptor.PropertyType;
            var items = new List<EnumOption>();
            foreach (var value in Enum.GetValues(enumType))
            {
                var displayName = enumType.GetField(value.ToString()).GetCustomAttributes(typeof(EnumDisplayNameAttribute), true).OfType<EnumDisplayNameAttribute>().FirstOrDefault();
                var item = new EnumOption();
                item.DisplayName = displayName == null ? value.ToString() : displayName.DisplayName;
                item.Value = value;
                items.Add(item);
            }

            var comboBox = new ComboBox();
            comboBox.SelectedValuePath = "Value";
            comboBox.DisplayMemberPath = "DisplayName";
            comboBox.ItemsSource = items;
            BindingOperations.SetBinding(comboBox, Selector.SelectedValueProperty, context.Binding);
            return comboBox;
        }

        /// <summary>
        /// Wraps an enum value and display name.
        /// </summary>
        public class EnumOption
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public object Value { get; set; }
            
            /// <summary>
            /// Gets or sets the display name.
            /// </summary>
            /// <value>The display name.</value>
            public object DisplayName { get; set; }
        }
    }
}
