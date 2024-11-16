using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.Item_Module.Events;
using ContentLib.Item_Module.Model;
using On.GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.Item_Module.Patches;

public class ItemPatches
{
    public static void Init()
    {
        On.GameNetcodeStuff.PlayerControllerB.GrabObjectClientRpc += PlayerControllerBOnGrabObjectClientRpc; 
        On.GameNetcodeStuff.PlayerControllerB.DropAllHeldItems += PlayerControllerBOnDropAllHeldItems;
        On.GameNetcodeStuff.PlayerControllerB.DropItemAheadOfPlayer += PlayerControllerBOnDropItemAheadOfPlayer;
    }

    private static Vector3 PlayerControllerBOnDropItemAheadOfPlayer(PlayerControllerB.orig_DropItemAheadOfPlayer orig, GameNetcodeStuff.PlayerControllerB self)
    {
        GrabbableObject itemToDrop = self.currentlyHeldObject;
        GameEventManager.Instance.Trigger(new BaseItemDropEvent(itemToDrop,self));
        return orig(self);
    }

    private static void PlayerControllerBOnDropAllHeldItems(PlayerControllerB.orig_DropAllHeldItems orig, GameNetcodeStuff.PlayerControllerB self, bool itemsfall, bool disconnecting)
    {
        GameEventManager gameEventManager = GameEventManager.Instance;
        foreach (GrabbableObject itemToDrop in self.ItemSlots)
        {
            if (itemToDrop == null)
                continue;
            gameEventManager.Trigger(new BaseItemDropEvent(itemToDrop, self));
        }

        orig(self, itemsfall, disconnecting);
    }

    private static void PlayerControllerBOnGrabObjectClientRpc(PlayerControllerB.orig_GrabObjectClientRpc orig, GameNetcodeStuff.PlayerControllerB self, bool grabvalidated, NetworkObjectReference grabbedobject)
    {
        orig(self, grabvalidated, grabbedobject);
        
        if (!grabbedobject.TryGet(out NetworkObject networkObject))
        {
            CLLogger.Instance.Log($"[ItemPatches] GrabObjectClientRpc returned no network object");
        }
        GrabbableObject grabbedObjectActual = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();
        
        GameEventManager.Instance.Trigger(new BaseItemPickupEvent(grabbedObjectActual,self));
    }

    private class BaseItemPickupEvent(GrabbableObject item, GameNetcodeStuff.PlayerControllerB playerController) : ItemPickUpEvent
    {
        public override Vector3 Position => item.transform.position;
        public override IGameItem Item => ItemManager.Instance.GetItem(item.NetworkObjectId);
        public override IGameEntity GrabbingEntity => EnemyManager.Instance.GetEnemy(playerController.NetworkObjectId);
    }

    private class BaseItemDropEvent(GrabbableObject item, GameNetcodeStuff.PlayerControllerB playerController): ItemDroppedEvent
    {
        public override Vector3 Position => item.transform.position;
        public override IGameItem Item => ItemManager.Instance.GetItem(item.NetworkObjectId);
        public override IGameEntity DroppingEntity => EnemyManager.Instance.GetEnemy(playerController.NetworkObjectId);
    }
}