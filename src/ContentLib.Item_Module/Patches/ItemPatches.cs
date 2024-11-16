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
        On.GameNetcodeStuff.PlayerControllerB.PlaceObjectClientRpc += PlayerControllerBOnPlaceObjectClientRpc;

        //TODO Figure out why this doesnt trigger for non-host clients. 
        On.GrabbableObject.DiscardItemOnClient += GrabbableObjectOnDiscardItemOnClient;
        On.GrabbableObject.DiscardItemClientRpc += GrabbableObjectOnDiscardItemClientRpc;
    }

    //TODO this triggers both a placement and a drop (regardless of who is doing it)
    private static void GrabbableObjectOnDiscardItemClientRpc(On.GrabbableObject.orig_DiscardItemClientRpc orig, GrabbableObject self)
    {
        GameNetcodeStuff.PlayerControllerB droppingPlayer = self.playerHeldBy;
        orig(self);
        CLLogger.Instance.Log("GRABBLE CLIENT RPC WORKED!");
        if (self.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Client)
            return;
        GameEventManager.Instance.Trigger(new BaseItemDropEvent(self, droppingPlayer));
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
    private static void GrabbableObjectOnDiscardItemOnClient(On.GrabbableObject.orig_DiscardItemOnClient orig, GrabbableObject self)
    {
        GameNetcodeStuff.PlayerControllerB droppingPlayer = self.playerHeldBy;
        CLLogger.Instance.Log(droppingPlayer == null
            ? "The object's dropping player is null."
            : $"Player: {droppingPlayer.name} dropped item");
        orig(self);
        if (self.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Client)
            return;
        GameEventManager.Instance.Trigger(new BaseItemDropEvent(self,droppingPlayer));
        CLLogger.Instance.Log($"Item Dropped {self.name}");

    }
 
    //TODO fully works but needs to be a place event instead... maybe...
    private static void PlayerControllerBOnPlaceObjectClientRpc(PlayerControllerB.orig_PlaceObjectClientRpc orig, GameNetcodeStuff.PlayerControllerB self, NetworkObjectReference parentobjectreference, Vector3 placepositionoffset, bool matchrotationofparent, NetworkObjectReference grabbedobject)
    {
        
        orig(self, parentobjectreference, placepositionoffset, matchrotationofparent, grabbedobject);
        if (self.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Client)
            return;
        if (!grabbedobject.TryGet(out NetworkObject networkObject))
            return;
        GrabbableObject droppedObject = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();

        GameEventManager.Instance.Trigger(new BaseItemDropEvent(droppedObject,self));
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