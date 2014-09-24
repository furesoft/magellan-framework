using System.Diagnostics;

namespace Magellan.Diagnostics
{
    /// <summary>
    /// Contains all Magellan trace sources.
    /// </summary>
    public static class TraceSources
    {
        private static TraceSource magellanSource;

        /// <summary>
        /// Refreshes the trace settings from configuration.
        /// </summary>
        public static void Refresh()
        {
            Trace.Refresh();
        }

        /// <summary>
        /// Gets the Magellan trace source (named 'Magellan' in configuration).
        /// </summary>
        /// <value>The magellan source.</value>
        public static TraceSource MagellanSource
        {
            get { return magellanSource ?? (magellanSource = CreateTraceSource("Magellan")); }
        }

        private static TraceSource CreateTraceSource(string name)
        {
            var source = new TraceSource(name, SourceLevels.Warning);
            return source;
        }
    }
}
