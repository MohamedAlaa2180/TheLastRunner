using UnityEngine;

namespace Genesis.Core.Events
{
    public readonly struct LivesChangedEvent : IEvent
    {
        public readonly int Lives;
        public readonly double Timestamp;

        public LivesChangedEvent(int lives, double timestamp = 0)
        {
            Lives = lives;
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }
}
