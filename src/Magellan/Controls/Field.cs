using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Magellan.Controls.Conventions;

namespace Magellan.Controls
{
    /// <summary>
    /// Represents a data entry field control, with a caption (Header), description and editor control. It 
    /// has the ability to automatically infer control settings using a single binding.
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TextBox))]
    public class Field : ContentControl
    {
        /// <summary>
        /// A static default value for fields that haven't been set (as opposed to explicitly set to null).
        /// </summary>
        public static readonly object UnsetField = new object();
        /// <summary>
        /// Dependency property for the <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(Field), new PropertyMetadata(string.Empty));
        /// <summary>
        /// Dependency property for the <see cref="Description"/> property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(object), typeof(Field), new PropertyMetadata(string.Empty));
        /// <summary>
        /// Dependency property for the <see cref="IsRequired"/> property.
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(Field), new PropertyMetadata(false));
        /// <summary>
        /// Dependency property for the <see cref="For"/> property.
        /// </summary>
        public static readonly DependencyProperty ForProperty = DependencyProperty.Register("For", typeof(object), typeof(Field), new UIPropertyMetadata(UnsetField, ForPropertySet));
        /// <summary>
        /// Dependency property for the <see cref="InferredHeader"/> property.
        /// </summary>
        public static readonly DependencyProperty InferredHeaderProperty = DependencyProperty.Register("InferredHeader", typeof(object), typeof(Field), new UIPropertyMetadata(null));
        /// <summary>
        /// Dependency property for the <see cref="InferredIsRequired"/> property.
        /// </summary>
        public static readonly DependencyProperty InferredIsRequiredProperty = DependencyProperty.Register("InferredIsRequired", typeof(bool), typeof(Field), new UIPropertyMetadata(null));
        /// <summary>
        /// Dependency property for the <see cref="InferredDescription"/> property.
        /// </summary>
        public static readonly DependencyProperty InferredDescriptionProperty = DependencyProperty.Register("InferredDescription", typeof(object), typeof(Field), new UIPropertyMetadata(null));
        private readonly FieldInferrer fieldInferrer = new FieldInferrer();

        /// <summary>
        /// Initializes the <see cref="Field"/> class.
        /// </summary>
        static Field()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Field), new FrameworkPropertyMetadata(typeof(Field)));
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Category("Appearance")]
        public object Description
        {
            get { return GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header, which serves as the caption for the field.
        /// </summary>
        /// <value>The header.</value>
        [Category("Appearance")]
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should display a required hint.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        [Category("Appearance")]
        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        /// <summary>
        /// Gets or sets a binding that will be used to automatically infer control settings.
        /// </summary>
        /// <value>For.</value>
        [Category("Appearance")]
        public object For
        {
            get { return GetValue(ForProperty); }
            set { SetValue(ForProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inferred header.
        /// </summary>
        /// <value>The inferred header.</value>
        public object InferredHeader
        {
            get { return GetValue(InferredHeaderProperty); }
            set { SetValue(InferredHeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inferred description.
        /// </summary>
        /// <value>The inferred description.</value>
        public object InferredDescription
        {
            get { return GetValue(InferredDescriptionProperty); }
            set { SetValue(InferredDescriptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inferred value for the <see cref="IsRequired"/> field.
        /// </summary>
        public bool InferredIsRequired
        {
            get { return (bool)GetValue(InferredIsRequiredProperty); }
            set { SetValue(InferredIsRequiredProperty, value); }
        }

        private static void ForPropertySet(DependencyObject @this, DependencyPropertyChangedEventArgs e)
        {
            var field = (Field)@this;
            field.Infer();
        }

        private void Infer()
        {
            var binding = BindingOperations.GetBinding(this, ForProperty);
            if (binding != null)
            {
                fieldInferrer.Infer(this, binding);
            }
        }
    }
}
