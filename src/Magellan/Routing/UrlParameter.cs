namespace Magellan.Routing
{
    /// <summary>
    /// Factory class used to create specific value objects to represent options in a path.
    /// </summary>
    public sealed class UrlParameter
    {
        /// <summary>
        /// Specifies that the route value is optional - if matched it will be used, but otherwise it will 
        /// not be looked for when matching a path.
        /// </summary>
        public static readonly UrlParameter Optional = new UrlParameter();
        internal static readonly UrlParameter NotSpecified = new UrlParameter();

        private UrlParameter()
        {
        }
    }
}
