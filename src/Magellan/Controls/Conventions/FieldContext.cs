using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Magellan.Controls.Conventions.Editors;

namespace Magellan.Controls.Conventions
{
    /// <summary>
    /// Contains information used by <see cref="IFieldConvention"/> and <see cref="IEditorStrategy"/> 
    /// implementations when they need to interrogate a field.
    /// </summary>
    public class FieldContext
    {
        private readonly Field field;
        private readonly Binding binding;
        private readonly object sourceItem;
        private readonly string propertyName;
        private readonly PropertyDescriptor propertyDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldContext"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="sourceItem">The source item.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        public FieldContext(Field field, Binding binding, object sourceItem, string propertyName, PropertyDescriptor propertyDescriptor)
        {
            this.field = field;
            this.binding = binding;
            this.sourceItem = sourceItem;
            this.propertyName = propertyName;
            this.propertyDescriptor = propertyDescriptor;
        }

        /// <summary>
        /// Gets the UI field control.
        /// </summary>
        /// <value>The field.</value>
        public Field Field
        {
            get { return field; }
        }

        /// <summary>
        /// Gets the binding that should be used for editing the UI field.
        /// </summary>
        /// <value>The binding.</value>
        public Binding Binding
        {
            get { return binding; }
        }

        /// <summary>
        /// Gets the source item, the object which contains the property. For deep bindings, such as 
        /// <code>{Binding Path=Orders[0].LineItems[0].Address.Street1}</code>, the <see cref="SourceItem"/> 
        /// would be <code>Address</code>, and the <see cref="PropertyName"/> would be <code>Street1</code>.
        /// </summary>
        /// <value>The source item.</value>
        public object SourceItem
        {
            get { return sourceItem; }
        }

        /// <summary>
        /// Gets the name of the property this field is bound to. For deep bindings, such as 
        /// <code>{Binding Path=Orders[0].LineItems[0].Address.Street1}</code>, the <see cref="SourceItem"/> 
        /// would be <code>Address</code>, and the <see cref="PropertyName"/> would be <code>Street1</code>.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Gets the property descriptor for the property represented by <see cref="PropertyName"/>. 
        /// </summary>
        /// <value>The property descriptor.</value>
        public PropertyDescriptor PropertyDescriptor
        {
            get { return propertyDescriptor; }
        }

        /// <summary>
        /// A helper that executes a delegate when the property has a given attribute type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="hasAttributeCallback">The callback executed if the attribute existed. The argument is the attribute instance.</param>
        public void WhenHasAttribute<TAttribute>(Action<TAttribute> hasAttributeCallback) where TAttribute : Attribute
        {
            var attributes = propertyDescriptor.Attributes.OfType<TAttribute>().ToList();
            foreach (var att in attributes)
            {
                hasAttributeCallback(att);
            }
        }
    }
}
