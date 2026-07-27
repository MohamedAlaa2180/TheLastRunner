using UnityEngine;

namespace Genesis.Core.Events
{
    public readonly struct PlayerJumpedEvent : IEvent
    {
        public readonly double Timestamp;

        public PlayerJumpedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct PlayerSlidedEvent : IEvent
    {
        public readonly double Timestamp;

        public PlayerSlidedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct PlayerGroundedChangedEvent : IEvent
    {
        public readonly bool IsGrounded;
        public readonly double Timestamp;

        public PlayerGroundedChangedEvent(bool isGrounded, double timestamp = 0)
        {
            IsGrounded = isGrounded;
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }
}
