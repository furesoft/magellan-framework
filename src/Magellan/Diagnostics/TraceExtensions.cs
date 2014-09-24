using System;
using System.Diagnostics;

namespace Magellan.Diagnostics
{
    /// <summary>
    /// This is a trace helper class to that wraps the Trace class for writting information to and
    /// from the event log.
    /// </summary>
    internal static class TraceExtensions
    {
        /// <summary>
        /// Indents the trace level and returns an IDisposable that can be used within a using block.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <returns>An <see cref="IDisposable"/> that allows indentation to be controlled with a using block.</returns>
        public static IDisposable Indent(this TraceSource traceSource)
        {
            return new TraceIndentation();
        }

        /// <summary>
        /// Unindents the trace level.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        public static void Unindent(this TraceSource traceSource)
        {
            Trace.Unindent();
        }

        public static void TraceVerbose(this TraceSource traceSource, int eventId, string message) { TraceInternal(traceSource, eventId, TraceLevel.Verbose, message); }
        public static void TraceVerbose(this TraceSource traceSource, int eventId, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Verbose, string.Format(messageFormat, arguments)); }
        public static void TraceVerbose(this TraceSource traceSource, int eventId, Exception exception) { TraceInternal(traceSource, eventId, TraceLevel.Verbose, exception.ToString()); }
        public static void TraceVerbose(this TraceSource traceSource, int eventId, Exception exception, string message) { TraceInternal(traceSource, eventId, TraceLevel.Verbose, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceVerbose(this TraceSource traceSource, int eventId, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Verbose, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceVerbose(this TraceSource traceSource, string message) { TraceInternal(traceSource, TraceLevel.Verbose, message); }
        public static void TraceVerbose(this TraceSource traceSource, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Verbose, string.Format(messageFormat, arguments)); }
        public static void TraceVerbose(this TraceSource traceSource, Exception exception) { TraceInternal(traceSource, TraceLevel.Verbose, exception.ToString()); }
        public static void TraceVerbose(this TraceSource traceSource, Exception exception, string message) { TraceInternal(traceSource, TraceLevel.Verbose, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceVerbose(this TraceSource traceSource, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Verbose, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        
        public static void TraceInformation(this TraceSource traceSource, int eventId, string message) { TraceInternal(traceSource, eventId, TraceLevel.Info, message); }
        public static void TraceInformation(this TraceSource traceSource, int eventId, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Info, string.Format(messageFormat, arguments)); }
        public static void TraceInformation(this TraceSource traceSource, int eventId, Exception exception) { TraceInternal(traceSource, eventId, TraceLevel.Info, exception.ToString()); }
        public static void TraceInformation(this TraceSource traceSource, int eventId, Exception exception, string message) { TraceInternal(traceSource, eventId, TraceLevel.Info, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceInformation(this TraceSource traceSource, int eventId, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Info, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceInformation(this TraceSource traceSource, string message) { TraceInternal(traceSource, TraceLevel.Info, message); }
        public static void TraceInformation(this TraceSource traceSource, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Info, string.Format(messageFormat, arguments)); }
        public static void TraceInformation(this TraceSource traceSource, Exception exception) { TraceInternal(traceSource, TraceLevel.Info, exception.ToString()); }
        public static void TraceInformation(this TraceSource traceSource, Exception exception, string message) { TraceInternal(traceSource, TraceLevel.Info, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceInformation(this TraceSource traceSource, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Info, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        
        public static void TraceWarning(this TraceSource traceSource, int eventId, string message) { TraceInternal(traceSource, eventId, TraceLevel.Warning, message); }
        public static void TraceWarning(this TraceSource traceSource, int eventId, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Warning, string.Format(messageFormat, arguments)); }
        public static void TraceWarning(this TraceSource traceSource, int eventId, Exception exception) { TraceInternal(traceSource, eventId, TraceLevel.Warning, exception.ToString()); }
        public static void TraceWarning(this TraceSource traceSource, int eventId, Exception exception, string message) { TraceInternal(traceSource, eventId, TraceLevel.Warning, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceWarning(this TraceSource traceSource, int eventId, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Warning, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceWarning(this TraceSource traceSource, string message) { TraceInternal(traceSource, TraceLevel.Warning, message); }
        public static void TraceWarning(this TraceSource traceSource, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Warning, string.Format(messageFormat, arguments)); }
        public static void TraceWarning(this TraceSource traceSource, Exception exception) { TraceInternal(traceSource, TraceLevel.Warning, exception.ToString()); }
        public static void TraceWarning(this TraceSource traceSource, Exception exception, string message) { TraceInternal(traceSource, TraceLevel.Warning, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceWarning(this TraceSource traceSource, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Warning, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        
        public static void TraceError(this TraceSource traceSource, int eventId, string message) { TraceInternal(traceSource, eventId, TraceLevel.Error, message); }
        public static void TraceError(this TraceSource traceSource, int eventId, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Error, string.Format(messageFormat, arguments)); }
        public static void TraceError(this TraceSource traceSource, int eventId, Exception exception) { TraceInternal(traceSource, eventId, TraceLevel.Error, exception.ToString()); }
        public static void TraceError(this TraceSource traceSource, int eventId, Exception exception, string message) { TraceInternal(traceSource, eventId, TraceLevel.Error, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceError(this TraceSource traceSource, int eventId, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, eventId, TraceLevel.Error, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceError(this TraceSource traceSource, string message) { TraceInternal(traceSource, TraceLevel.Error, message); }
        public static void TraceError(this TraceSource traceSource, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Error, string.Format(messageFormat, arguments)); }
        public static void TraceError(this TraceSource traceSource, Exception exception) { TraceInternal(traceSource, TraceLevel.Error, exception.ToString()); }
        public static void TraceError(this TraceSource traceSource, Exception exception, string message) { TraceInternal(traceSource, TraceLevel.Error, message + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        public static void TraceError(this TraceSource traceSource, Exception exception, string messageFormat, params object[] arguments) { TraceInternal(traceSource, TraceLevel.Error, string.Format(messageFormat, arguments) + Environment.NewLine + Environment.NewLine + exception.ToString()); }
        
        private static void TraceInternal(TraceSource traceSource, TraceLevel traceLevel, string message)
        {
            TraceInternal(traceSource, 0, traceLevel, message);
        }

        private static void TraceInternal(TraceSource traceSource, int eventId, TraceLevel traceLevel, string message)
        {
            if (traceSource == null) 
                return;

            var traceEventType = ConvertTraceLevelToTraceEventType(traceLevel);
            traceSource.TraceEvent(traceEventType, eventId, message);
        }

        private static TraceEventType ConvertTraceLevelToTraceEventType(TraceLevel traceLevel)
        {
            var traceEventType = TraceEventType.Verbose;
            switch (traceLevel)
            {
                case TraceLevel.Verbose:
                    traceEventType = TraceEventType.Verbose;
                    break;
                case TraceLevel.Info:
                    traceEventType = TraceEventType.Information;
                    break;
                case TraceLevel.Warning:
                    traceEventType = TraceEventType.Warning;
                    break;
                case TraceLevel.Error:
                    traceEventType = TraceEventType.Error;
                    break;
            }
            return traceEventType;
        }

        private sealed class TraceIndentation : IDisposable
        {
            public TraceIndentation()
            {
                Trace.Indent();
            }

            public void Dispose()
            {
                Trace.Unindent();
            }
        }
    }
}