using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
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
        On.GameNetcodeStuff.PlayerControllerB.GrabObjectServerRpc += PlayerControllerBOnGrabObjectServerRpc; 
        On.GameNetcodeStuff.PlayerControllerB.ThrowObjectServerRpc += PlayerControllerBOnThrowObjectServerRpc;
    }

    private static void PlayerControllerBOnThrowObjectServerRpc(PlayerControllerB.orig_ThrowObjectServerRpc orig, GameNetcodeStuff.PlayerControllerB self, NetworkObjectReference grabbedobject, bool droppedinelevator, bool droppedinshiproom, Vector3 targetfloorposition, int flooryrot)
    {        
        var isServerCall = IsServerCall(self);
        orig(self, grabbedobject, droppedinelevator, droppedinshiproom, targetfloorposition, flooryrot);
        
        if (!isServerCall)
            return;

        if (!grabbedobject.TryGet(out NetworkObject networkObject))
            return;
        
        GrabbableObject droppedObject = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();

        GameEventManager.Instance.Trigger(new BaseItemDropEvent(droppedObject,self));
    }



    private static void PlayerControllerBOnGrabObjectServerRpc(PlayerControllerB.orig_GrabObjectServerRpc orig, GameNetcodeStuff.PlayerControllerB self, NetworkObjectReference grabbedobject)
    {
        var isServerCall = IsServerCall(self);
        orig(self, grabbedobject);
      
        if (!isServerCall)
            return;

        if (!grabbedobject.TryGet(out NetworkObject networkObject))
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

    private static bool IsServerCall(NetworkBehaviour networkBehaviour)
    {
        //TODO needs to have the logs added back in when the debug system is fully setup
        if (networkBehaviour.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Server)
        {
            //CLLogger.Instance.Log($"[ItemPatches] IsServerCall returned true");
            return true;
        }

        //CLLogger.Instance.Log($"[ItemPatches] IsServerCall returned false");
        return false;
    }
}