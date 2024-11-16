using System.Reflection;
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
        On.GameNetcodeStuff.PlayerControllerB.DiscardHeldObject += PlayerControllerBOnDiscardHeldObject;
        On.GrabbableObject.DiscardItemClientRpc += GrabbableObjectOnDiscardItemClientRpc;
    }

    private static void GrabbableObjectOnDiscardItemClientRpc(On.GrabbableObject.orig_DiscardItemClientRpc orig, GrabbableObject self)
    {
        //TODO this needs a lot more logic, boomboxes dont work for some reason, and placements of objects still trigger drop events too.
        orig(self);
        
        if (self.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Client)
            return;
        GameEventManager.Instance.Trigger(new BaseItemDropEvent(self,null));
        CLLogger.Instance.Log($"The Player dropped {self.name}.");
        
    }

    private static void PlayerControllerBOnDiscardHeldObject(PlayerControllerB.orig_DiscardHeldObject orig, GameNetcodeStuff.PlayerControllerB self, bool placeobject, NetworkObject parentobjectto, Vector3 placeposition, bool matchrotationofparent)
    {
      orig(self, placeobject, parentobjectto, placeposition, matchrotationofparent);
      if (!placeobject)
          return;
    }

    private static void PlayerControllerBOnDropAllHeldItems(PlayerControllerB.orig_DropAllHeldItems orig, GameNetcodeStuff.PlayerControllerB? self, bool itemsfall, bool disconnecting)
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
            return;
        }

        if (self.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Client)
            return;
        
        GrabbableObject grabbedObjectActual = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();
      
        GameEventManager.Instance.Trigger(new BaseItemPickupEvent(grabbedObjectActual, self));
    }


    private class BaseItemPickupEvent(GrabbableObject item, GameNetcodeStuff.PlayerControllerB playerController) : ItemPickUpEvent
    {
        public override Vector3 Position => item.transform.position;
        public override IGameItem Item => ItemManager.Instance.GetItem(item.NetworkObjectId);
        public override IGameEntity GrabbingEntity => EnemyManager.Instance.GetEnemy(playerController.NetworkObjectId);
    }

    private class BaseItemDropEvent(GrabbableObject item, GameNetcodeStuff.PlayerControllerB? playerController): ItemDroppedEvent
    {
        public override Vector3 Position => item.transform.position;
        public override IGameItem Item => ItemManager.Instance.GetItem(item.NetworkObjectId);
        public override IGameEntity DroppingEntity => EnemyManager.Instance.GetEnemy(playerController.NetworkObjectId);
    }
}