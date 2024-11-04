using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;

namespace ContentLib.Item_Module.Events;

public class ItemEvents
{
    public interface IItemEvent : IGameEvent
    {
        IGameItem Item { get; }
    }
}