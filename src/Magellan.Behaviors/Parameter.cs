using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace Magellan.Behaviors
{
    /// <summary>
    /// Represents a <see cref="Parameter"/> to a <see cref="NavigateAction"/>.
    /// </summary>
    public class Parameter : Animatable
    {
        /// <summary>
        /// Dependency property for the <see cref="Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(Parameter), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        [Category("Navigation")]
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        /// <value>The value.</value>
        [Category("Navigation")]
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(GetType());
        }
    }
}