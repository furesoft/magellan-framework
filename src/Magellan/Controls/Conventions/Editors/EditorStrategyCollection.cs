using Magellan.ComponentModel;
using Magellan.Utilities;

namespace Magellan.Controls.Conventions.Editors
{
    /// <summary>
    /// Represents a collection of <see cref="IEditorStrategy">editor strategies</see>.
    /// </summary>
    public class EditorStrategyCollection : Set<IEditorStrategy>
    {
        private readonly IEditorStrategy fallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorStrategyCollection"/> class.
        /// </summary>
        /// <param name="fallback">The fallback.</param>
        public EditorStrategyCollection(IEditorStrategy fallback)
        {
            Guard.ArgumentNotNull(fallback, "fallback");
            this.fallback = fallback;
        }

        /// <summary>
        /// Asks each <see cref="IEditorStrategy">editor strategy</see> in the list whether it can create a 
        /// control for editing the given field, returning the first non-null result. If none of the editors 
        /// can provide an editor, the fallback editor is asked.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public object GetEditor(FieldContext context)
        {
            foreach (var selector in this)
            {
                var editor = selector.CreateEditor(context);
                if (editor != null)
                {
                    return editor;
                }
            }
            return fallback.CreateEditor(context);
        }

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, IEditorStrategy item)
        {
            Edit(x =>
                     {
                         if (x.Count == 0)
                             x.Add(item);
                         else x.Insert(index, item);
                     });
        }
    }
}