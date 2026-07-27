using System;

namespace Genesis.Core.Events
{
    public interface IEventBus
    {
        void Publish<T>(T evt) where T : struct, IEvent;

        IDisposable Subscribe<T>(Action<T> handler) where T : struct, IEvent;
    }
}