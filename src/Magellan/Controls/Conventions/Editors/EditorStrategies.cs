namespace Magellan.Controls.Conventions.Editors
{
    /// <summary>
    /// A registry for the available <see cref="IEditorStrategy">editor strategies</see>.
    /// </summary>
    public static class EditorStrategies
    {
        private static readonly EditorStrategyCollection editors = new EditorStrategyCollection(new TextBoxEditorStrategy());

        /// <summary>
        /// Initializes the <see cref="EditorStrategies"/> class.
        /// </summary>
        static EditorStrategies()
        {
            editors.Add(new CheckBoxEditorStrategy());
            editors.Add(new ComboBoxEditorStrategy());
        }

        /// <summary>
        /// Gets the configured list of editors.
        /// </summary>
        /// <value>The editors.</value>
        public static EditorStrategyCollection Strategies
        {
            get { return editors; }
        }
    }
}