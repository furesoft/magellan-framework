using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Magellan.Controls.Conventions.Editors;

namespace Magellan.Controls.Conventions
{
    /// <summary>
    /// A field convention that makes use of a set of <see cref="IEditorStrategy">editor strategies</see> to 
    /// choose an appropriate editor.
    /// </summary>
    public class FieldConvention : IFieldConvention
    {
        private readonly EditorStrategyCollection editorStrategies;
        private readonly List<KeyValuePair<Type, Action<Field, Attribute>>> attributeMatchers = new List<KeyValuePair<Type, Action<Field, Attribute>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldConvention"/> class.
        /// </summary>
        /// <param name="editorStrategies">The editor strategies.</param>
        public FieldConvention(EditorStrategyCollection editorStrategies)
        {
            this.editorStrategies = editorStrategies;

            WhenPropertyHas<DisplayNameAttribute>((fld, att) => fld.InferredHeader = att.DisplayName);
            WhenPropertyHas<DescriptionAttribute>((fld, att) => fld.InferredDescription = att.Description);
            WhenPropertyHas<DisplayAttribute>((fld, att) => fld.InferredHeader = att.Name);
            WhenPropertyHas<DisplayAttribute>((fld, att) => fld.InferredDescription = att.Description);
            WhenPropertyHas<RequiredAttribute>((fld, att) => fld.InferredIsRequired = true);
        }

        /// <summary>
        /// Registers a handler for a specific attribute type. For example, 
        /// <code>WhenPropertyHas&lt;MyAttribute&gt;((att,fld) =&gt; fld.InferredHeader = att.Bar)</code>.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="setterCallback">The setter callback.</param>
        public void WhenPropertyHas<TAttribute>(Action<Field, TAttribute> setterCallback) where TAttribute : Attribute
        {
            attributeMatchers.Add(new KeyValuePair<Type, Action<Field, Attribute>>(typeof(TAttribute), (f, a) => setterCallback(f, (TAttribute)a)));
        }

        /// <summary>
        /// Configures the field using all of the information in the <paramref name="fieldInfo"/> parameter.
        /// </summary>
        /// <param name="fieldInfo">The field info.</param>
        public void Configure(FieldContext fieldInfo)
        {
            var attributes = fieldInfo.PropertyDescriptor.Attributes.Cast<Attribute>().ToList();
            foreach (var attribute in attributes)
            {
                foreach (var recognizer in attributeMatchers)
                {
                    if (recognizer.Key.IsAssignableFrom(attribute.GetType()))
                    {
                        recognizer.Value(fieldInfo.Field, attribute);
                    }
                }
            }

            if (fieldInfo.Field.InferredHeader == null)
            {
                fieldInfo.Field.InferredHeader = fieldInfo.PropertyName;
            }
            
            var editor = editorStrategies.GetEditor(fieldInfo);
            if (fieldInfo.Field.Content == null)
            {
                fieldInfo.Field.Content = editor;
            }
        }
    }
}