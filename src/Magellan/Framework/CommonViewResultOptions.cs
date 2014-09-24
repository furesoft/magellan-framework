namespace Magellan.Framework
{
    /// <summary>
    /// Extension methods for accessing common <see cref="ViewResultOptions"/> values.
    /// </summary>
    public static class CommonViewResultOptions
    {
        /// <summary>
        /// Gets the type of the view.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static string GetViewType(this ViewResultOptions options)
        {
            return options.GetOrDefault<string>("ViewType");
        }

        /// <summary>
        /// Sets the type of the view.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="viewType">Type of the view.</param>
        public static void SetViewType(this ViewResultOptions options, string viewType)
        {
            options["ViewType"] = viewType;
        }

        /// <summary>
        /// Gets the model that will be bound to the view.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static object GetModel(this ViewResultOptions options)
        {
            return options.GetOrDefault<object>("Model");
        }

        /// <summary>
        /// Sets the model that will be bound to the view.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="model">The model.</param>
        public static void SetModel(this ViewResultOptions options, object model)
        {
            options["Model"] = model;
        }

        /// <summary>
        /// Gets whether to reset history.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static bool GetResetHistory(this ViewResultOptions options)
        {
            return options.GetOrDefault<bool>("ResetNavigationHistory");
        }

        /// <summary>
        /// Sets whether to reset history.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="resetHistory">if set to <c>true</c> [reset history].</param>
        public static void SetResetHistory(this ViewResultOptions options, bool resetHistory)
        {
            options["ResetNavigationHistory"] = resetHistory;
        }
    }
}