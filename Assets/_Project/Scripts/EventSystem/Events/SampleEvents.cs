using UnityEngine;

namespace Genesis.Core.Events
{
    public readonly struct TestEvent : IEvent
    {
        public readonly double Timestamp;

        public TestEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }
}