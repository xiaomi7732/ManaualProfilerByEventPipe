using System.Diagnostics.Tracing;

namespace ServiceProfiler.EventPipe.UserApp30
{
    [EventSource(Name = ProfilerTestEventSource.DisplayName)]
    public class ProfilerTestEventSource : EventSource
    {
        public const string DisplayName = "Profiler-Test-EventPipe-EventSource-Provider";

        // This is a bitvector
        public class Keywords
        {
            public const EventKeywords HeartBeat = (EventKeywords)0x1;
        }

        [Event(1, Keywords = Keywords.HeartBeat)]
        public void HeartBeat(string message)
        {
            WriteEvent(1, message);
        }
        public static ProfilerTestEventSource Log { get; } = new ProfilerTestEventSource();
    }
}
