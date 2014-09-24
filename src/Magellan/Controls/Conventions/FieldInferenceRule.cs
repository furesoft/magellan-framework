using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace Magellan.Controls.Conventions
{
    /// <summary>
    /// This validation rule is applied to the <see cref="Field.For"/> binding to automatically infer field 
    /// settings. A validation rule is used because it will be triggered any time the property path changes. 
    /// </summary>
    internal class FieldInferenceRule : ValidationRule
    {
        private readonly Field field;
        private readonly IFieldConvention fieldBuilder;
        private readonly Binding editorBinding;
        private Type previousSourceItemType;
        
        public FieldInferenceRule(Field field, IFieldConvention fieldBuilder, Binding editorBinding)
            : base(ValidationStep.UpdatedValue, true)
        {
            this.field = field;
            this.fieldBuilder = fieldBuilder;
            this.editorBinding = editorBinding;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var expression = value as BindingExpression;
            if (expression == null)
            {
                // TODO: Can't handle this
                return ValidationResult.ValidResult;
            }

            var sourceItem = typeof(BindingExpression).GetProperty("SourceItem", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(expression, null);
            if (sourceItem == null || sourceItem.GetType().Name == "NullDataItem" || sourceItem.GetType() == previousSourceItemType)
            {
                return ValidationResult.ValidResult;
            }
            previousSourceItemType = sourceItem.GetType();

            var propertyName = (string)typeof(BindingExpression).GetProperty("SourcePropertyName", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(expression, null);
            if (propertyName == null)
            {
                return ValidationResult.ValidResult;
            }

            var properties = TypeDescriptor.GetProperties(sourceItem);
            var property = properties.Find(propertyName, true);
            if (property == null)
            {
                // TODO: Can't handle this
                return ValidationResult.ValidResult;
            }

            var context = new FieldContext(field, editorBinding, sourceItem, propertyName, property);
            fieldBuilder.Configure(context);

            return ValidationResult.ValidResult;
        }
    }
}