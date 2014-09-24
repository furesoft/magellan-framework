using System;
using System.ComponentModel;

namespace Magellan.Diagnostics
{
    [Flags]
    public enum SourceLevels
    {
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        ActivityTracing = 0xff00,
        All = -1,
        Critical = 1,
        Error = 3,
        Information = 15,
        Off = 0,
        Verbose = 0x1f,
        Warning = 7
    }
}