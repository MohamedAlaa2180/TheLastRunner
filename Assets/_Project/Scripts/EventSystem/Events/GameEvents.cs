using UnityEngine;

namespace Genesis.Core.Events
{
    public readonly struct GameStartedEvent : IEvent
    {
        public readonly double Timestamp;

        public GameStartedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct GamePausedEvent : IEvent
    {
        public readonly double Timestamp;

        public GamePausedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct GameResumedEvent : IEvent
    {
        public readonly double Timestamp;

        public GameResumedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct GameOverEvent : IEvent
    {
        public readonly double Timestamp;

        public GameOverEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct StartGameRequestedEvent : IEvent
    {
        public readonly double Timestamp;

        public StartGameRequestedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct CountdownStartedEvent : IEvent
    {
        public readonly double Timestamp;

        public CountdownStartedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct CountdownTickEvent : IEvent
    {
        public readonly int Value;
        public readonly double Timestamp;

        public CountdownTickEvent(int value, double timestamp = 0)
        {
            Value = value;
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct ResumeGameRequestedEvent : IEvent
    {
        public readonly double Timestamp;

        public ResumeGameRequestedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct GameHitEvent : IEvent
    {
        public readonly double Timestamp;

        public GameHitEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct ContinueRequestedEvent : IEvent
    {
        public readonly double Timestamp;

        public ContinueRequestedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }

    public readonly struct RestartRequestedEvent : IEvent
    {
        public readonly double Timestamp;

        public RestartRequestedEvent(double timestamp = 0)
        {
            Timestamp = timestamp == 0 ? Time.timeAsDouble : timestamp;
        }

        double IEvent.Timestamp => Timestamp;
    }
}
