using System.Windows;
using System.Windows.Controls;
using Magellan.Controls.Conventions;

namespace Magellan.Controls
{
    /// <summary>
    /// A control that contains a collection of fields.
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(Form))]
    public class Form : ItemsControl
    {
        /// <summary>
        /// Dependency property for the FieldBuilder property.
        /// </summary>
        public static readonly DependencyProperty FieldBuilderProperty = DependencyProperty.RegisterAttached("FieldBuilder", typeof(IFieldConvention), typeof(Form), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Initializes the <see cref="Form"/> class.
        /// </summary>
        static Form()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));
        }

        /// <summary>
        /// Gets the field builder associated with the given element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IFieldConvention GetFieldBuilder(DependencyObject element)
        {
            return (IFieldConvention)element.GetValue(FieldBuilderProperty);
        }

        /// <summary>
        /// Sets the field builder associated with the given element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetFieldBuilder(DependencyObject element, IFieldConvention value)
        {
            element.SetValue(FieldBuilderProperty, value);
        }
    }
}
