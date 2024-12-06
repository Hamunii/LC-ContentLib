using System.Diagnostics.Tracing;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Scrap;
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

    [EventDelegate]
    private void OnPlayerJump(PlayerJumpEvent playerJumpEvent)
    {
        playerJumpEvent.Player.TeleportToShip();
    }

    [EventDelegate]
    private void OnItemSpawn(ItemSpawnedEvent itemSpawnedEvent)
    {
        if (itemSpawnedEvent.Item is not IVanillaScrap scrap)
        {
            return;
        }

        if (scrap.Type == ScrapType.Airhorn)
        {
            Debug.Log("AIRHORN WAS SPAWNED!");
        }
        else
        {
            Debug.Log($"{scrap.Type} WAS SPAWNED!");
        }
    }
}