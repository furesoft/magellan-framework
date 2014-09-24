namespace Magellan.Diagnostics
{
    public class TraceSource
    {
        private IsolatedStorageTracer _tracer;

        public TraceSource(string name)
        {
            Switch = new SourceSwitch();
        }

        public SourceSwitch Switch { get; set; }

        public void TraceEvent(TraceEventType traceEventType, int eventId, string message)
        {
            if (!Switch.ShouldTrace(traceEventType)) return;
            
            // We lazily initialize this, so that isolated stoage isn't used unless required
            if (_tracer == null)
            {
                _tracer = new IsolatedStorageTracer();
            }
            _tracer.Write(string.Format("{0}: {1}", traceEventType, message));
        }
    }
}
