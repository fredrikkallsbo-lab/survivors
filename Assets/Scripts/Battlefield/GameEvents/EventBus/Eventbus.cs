namespace System.Collections.Generic
{
    public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();
    private readonly object _lock = new();

    public void Publish<T>(T evt) where T : IGameEvent
    {
        if (evt == null) throw new ArgumentNullException(nameof(evt));

        List<Delegate> handlersCopy = null;
        lock (_lock)
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
            {
                // Make a copy to avoid modification during iteration
                handlersCopy = new List<Delegate>(handlers);
            }
        }

        if (handlersCopy == null) return;

        foreach (var handler in handlersCopy)
        {
            try
            {
                ((Action<T>)handler)?.Invoke(evt);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"EventBus handler exception ({typeof(T).Name}): {ex}");
            }
        }
    }

    public IDisposable Subscribe<T>(Action<T> handler) where T : IGameEvent
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Delegate>();
                _handlers[typeof(T)] = handlers;
            }
            handlers.Add(handler);
        }

        // Return an IDisposable that unsubscribes when disposed
        return new Subscription<T>(this, handler);
    }

    private void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
    {
        lock (_lock)
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                    _handlers.Remove(typeof(T));
            }
        }
    }

    private class Subscription<T> : IDisposable where T : IGameEvent
    {
        private EventBus _bus;
        private Action<T> _handler;

        public Subscription(EventBus bus, Action<T> handler)
        {
            _bus = bus;
            _handler = handler;
        }

        public void Dispose()
        {
            if (_bus != null && _handler != null)
            {
                _bus.Unsubscribe(_handler);
                _bus = null;
                _handler = null;
            }
        }
    }
}
}