using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Scrap;
using ContentLib.API.Model.Item.Tools.Types;
using ContentLib.Core.Model.Event.Listener;
using UnityEngine;

namespace ContentLib.ExampleMod;

public class TestListener : IListener
{
    [EventDelegate]
    private void OnItemActivation(ItemActivationEvent itemActivationEvent)
    {
        IGameItem? item = itemActivationEvent.Item;
        if (item is IRemoteControlScrap && item.Owner is IPlayer player)
        {
            
            player.TeleportToShip();
        }
    }
}