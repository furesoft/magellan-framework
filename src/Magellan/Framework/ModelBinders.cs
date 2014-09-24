namespace Magellan.Framework
{
    /// <summary>
    /// Manages the default store of <see cref="IModelBinder">model binders</see>.
    /// </summary>
    public static class ModelBinders
    {
        /// <summary>
        /// Gets the default registry of model binders.
        /// </summary>
        /// <value>The model binders.</value>
        public static ModelBinderDictionary CreateDefaults()
        {
            return new ModelBinderDictionary(new DefaultModelBinder());
        }
    }
}