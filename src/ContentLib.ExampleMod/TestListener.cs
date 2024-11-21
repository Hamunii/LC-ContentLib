using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Tools.Types;
using ContentLib.Core.Model.Event.Listener;

namespace ContentLib.ExampleMod;

public class TestListener : IListener
{
    [EventDelegate]
    private void OnItemActivation(ItemActivationEvent itemActivationEvent)
    {
        IGameItem item = itemActivationEvent.Item;
        if (item is IFlashlight)
        {
            ((IPlayer)item.Owner).TeleportToShip();
        }
    }
}