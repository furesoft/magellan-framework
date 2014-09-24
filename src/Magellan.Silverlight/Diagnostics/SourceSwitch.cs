namespace Magellan.Diagnostics
{
    public class SourceSwitch
    {
        public bool ShouldTrace(TraceEventType eventType)
        {
            return (((int)Level & (int)eventType) != 0);
        }

        // Properties
        public SourceLevels Level { get; set; }
    }
}