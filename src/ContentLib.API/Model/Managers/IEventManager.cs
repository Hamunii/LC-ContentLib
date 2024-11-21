using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Event.Listener;

namespace System.Runtime.CompilerServices.Model.Managers;

public interface IGameEventManager
{
    void Trigger<TEvent>(TEvent gameEvent) where TEvent : IGameEvent;
    void RegisterListener(IListener listener);
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent;
}