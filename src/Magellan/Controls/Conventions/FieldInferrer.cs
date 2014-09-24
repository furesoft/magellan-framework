using System.Windows;
using System.Windows.Data;
using Magellan.Controls.Conventions.Editors;
using Magellan.Utilities;

namespace Magellan.Controls.Conventions
{
    /// <summary>
    /// Manages inference of field settings when the For property binding is used.
    /// </summary>
    internal class FieldInferrer
    {
        private static readonly DependencyProperty previousFieldBindingProperty = DependencyProperty.RegisterAttached("PreviousFieldBinding", typeof(Binding), typeof(FieldInferrer), new UIPropertyMetadata(null));
        
        public void Infer(Field field, Binding binding)
        {
            var previousBinding = GetPreviousFieldBinding(field);
            if (previousBinding == binding)
            {
                return;
            }

            SetPreviousFieldBinding(field, binding);

            var editorBinding = binding.Clone();

            var fieldBuilder = Form.GetFieldBuilder(field)
                               ?? new FieldConvention(EditorStrategies.Strategies);
            
            binding.ValidationRules.Add(new FieldInferenceRule(field, fieldBuilder, editorBinding));
        }

        private static Binding GetPreviousFieldBinding(DependencyObject obj)
        {
            return (Binding)obj.GetValue(previousFieldBindingProperty);
        }

        private static void SetPreviousFieldBinding(DependencyObject obj, Binding value)
        {
            obj.SetValue(previousFieldBindingProperty, value);
        }
    }
}