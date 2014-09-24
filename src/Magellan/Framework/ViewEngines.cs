namespace Magellan.Framework
{
    /// <summary>
    /// Manages the global registry of view engines that are available for rendering a view.
    /// </summary>
    public class ViewEngines
    {
        /// <summary>
        /// Creates the default view engines.
        /// </summary>
        /// <returns></returns>
        public static ViewEngineCollection CreateDefaults()
        {
            return CreateDefaults(null);
        }

        /// <summary>
        /// Creates the default view engines.
        /// </summary>
        /// <param name="viewActivator">The view activator.</param>
        /// <returns></returns>
        public static ViewEngineCollection CreateDefaults(IViewActivator viewActivator)
        {
            viewActivator = viewActivator ?? new DefaultViewActivator();
            
            var engines = new ViewEngineCollection();
            engines.Add(new PageViewEngine(viewActivator));
            engines.Add(new WindowViewEngine(viewActivator));
            return engines;
        }
    }
}
