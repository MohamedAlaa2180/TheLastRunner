using UnityEngine;

namespace Genesis.Core.Events
{
    public readonly struct CoinCollectedEvent : IEvent
    {
        public readonly int Amount;
        public readonly double Timestamp;

        public CoinCollectedEvent(int amount, double timestamp = 0)
        {
            Amount = amount;
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }
}
