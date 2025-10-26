namespace System.Collections.Generic
{
    public interface IEventBus
    {
        void Publish<T>(T evt) where T : IGameEvent;
        IDisposable Subscribe<T>(Action<T> handler) where T : IGameEvent;
    }
}