using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using UnityEngine;

namespace ContentLib.Item_Module.Events;
public interface IItemEvent : IGameEvent
{
    Vector3 Position { get; }
    IGameItem Item { get; }
}

public abstract class OnItemActivationEvent() : IItemEvent
{
    public abstract Vector3 Position { get; }
    public abstract IGameItem Item { get; }
    public bool IsCancelled { get; set; }
}
