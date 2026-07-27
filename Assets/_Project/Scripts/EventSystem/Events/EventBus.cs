using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Core.Events
{
    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, Delegate> _handlers = new();
        private readonly object _lock = new();

        public void Publish<T>(T evt) where T : struct, IEvent
        {
            var key = typeof(T);
            List<Action<T>> toInvoke;
            lock (_lock)
            {
                if (!_handlers.TryGetValue(key, out var d) || d == null)
                    return;
                var combined = (Delegate)d;
                toInvoke = new List<Action<T>>();
                foreach (var h in combined.GetInvocationList())
                    toInvoke.Add((Action<T>)h);
            }
            foreach (var h in toInvoke)
            {
                try { h(evt); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
        }

        public IDisposable Subscribe<T>(Action<T> handler) where T : struct, IEvent
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            var key = typeof(T);
            lock (_lock)
            {
                if (_handlers.TryGetValue(key, out var existing))
                    _handlers[key] = Delegate.Combine(existing, handler);
                else
                    _handlers[key] = handler;
            }
            return new EventSubscription<T>(this, handler);
        }

        internal void Unsubscribe<T>(Action<T> handler) where T : struct, IEvent
        {
            var key = typeof(T);
            lock (_lock)
            {
                if (_handlers.TryGetValue(key, out var existing))
                {
                    var removed = Delegate.Remove(existing, handler);
                    if (removed == null)
                        _handlers.Remove(key);
                    else
                        _handlers[key] = removed;
                }
            }
        }
    }

    internal sealed class EventSubscription<T> : IDisposable where T : struct, IEvent
    {
        private EventBus _bus;
        private Action<T> _handler;

        public EventSubscription(EventBus bus, Action<T> handler)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public void Dispose()
        {
            if (_bus == null) return;
            _bus.Unsubscribe(_handler);
            _bus = null;
            _handler = null;
        }
    }
}