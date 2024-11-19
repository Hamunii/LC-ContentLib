using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.entityAPI.Model.entity;
using ContentLib.Item_Module.Events;
using ContentLib.Item_Module.Model;
using On.GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.Item_Module.Patches;
/// <summary>
/// Patches related to general actions involving IGameItems, such as picking up and dropping of items, placement of
/// items etc.
/// </summary>
public class ItemPatches
{
    public static void Init()
    {
        On.GameNetcodeStuff.PlayerControllerB.GrabObjectServerRpc += PlayerControllerBOnGrabObjectServerRpc; 
        On.GameNetcodeStuff.PlayerControllerB.ThrowObjectServerRpc += PlayerControllerBOnThrowObjectServerRpc;
    }

    /// <summary>
    /// Patch for registering item drop events. <i> Maintainer Note: The "throw" method is called regardless of the
    /// type of drop, whereas the poorly labeled "discard" method merely resets certain client params, hence is not
    /// always called via RPC. </i>
    /// </summary>
    /// <param name="orig">Original PlayerControllerB method for "throwing" an item.</param>
    /// <param name="self">The PlayerControllerB instance.</param>
    /// <param name="grabbedobject">A Network Reference to the object that has been "thrown"</param>
    /// <param name="droppedinelevator">If the object has been dropped in an elevator.</param>
    /// <param name="droppedinshiproom">If the object has been dropped in the ship.</param>
    /// <param name="targetfloorposition">The position in the game world in which the dropped item will land.</param>
    /// <param name="flooryrot">Not entirely sure? But could be the rotation of the floor relative to the plane?</param>
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



    /// <summary>
    /// Patch for registering item grab events.
    /// </summary>
    /// <param name="orig">Original PlayerControllerB method for grabbing an item.</param>
    /// <param name="self">The PlayerControllerB instance.</param>
    /// <param name="grabbedobject">A Network Reference to the object that has been grabbed.</param>
    private static void PlayerControllerBOnGrabObjectServerRpc(PlayerControllerB.orig_GrabObjectServerRpc orig, GameNetcodeStuff.PlayerControllerB self, NetworkObjectReference grabbedobject)
    {
        var isServerCall = IsServerCall(self);
        orig(self, grabbedobject);
      
        if (!isServerCall)
            return;

        if (!grabbedobject.TryGet(out NetworkObject networkObject))
            return;
        //TODO apparently this might be busted and calling root
        GrabbableObject grabbedObjectActual = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();
      
        GameEventManager.Instance.Trigger(new BaseItemPickupEvent(grabbedObjectActual, self));
    }
    
    /// <summary>
    /// Implementation of the ItemPickupEvent abstract class. 
    /// </summary>
    /// <param name="item">The Grabbable Object instance that has been picked up.</param>
    /// <param name="playerController">The Instance of the Player Controller whom has picked the item up.</param>
    private class BaseItemPickupEvent(GrabbableObject item, GameNetcodeStuff.PlayerControllerB playerController) : ItemPickUpEvent
    {
        public override Vector3 Position => item.transform.position;
        public override IGameItem Item => ItemManager.Instance.GetItem(item.NetworkObjectId);
        public override IGameEntity GrabbingEntity => EntityManager.Instance.GetEntity(playerController.NetworkObjectId);
    }
    /// <summary>
    /// Implementation of the ItemDroppedEvent abstract class. 
    /// </summary>
    /// <param name="item">The Grabbable Object instance that has been dropped.</param>
    /// <param name="playerController">The Instance of the Player Controller whom has dropped the item.</param>
    private class BaseItemDropEvent(GrabbableObject item, GameNetcodeStuff.PlayerControllerB? playerController): ItemDroppedEvent
    {
        public override Vector3 Position => item.transform.position;
        public override IGameItem Item => ItemManager.Instance.GetItem(item.NetworkObjectId);
        public override IGameEntity DroppingEntity => EntityManager.Instance.GetEntity(playerController.NetworkObjectId);
    }

    /// <summary>
    /// Method for checking if the current execution stage a given Network Behaviour instance is server-side. Used to
    /// ensure no double firing of events.
    /// </summary>
    /// <param name="networkBehaviour">The executing Network Behaviour to check.</param>
    /// <returns>True if the Network Behaviour's execution stage is server-side, False otherwise.</returns>
    private static bool IsServerCall(NetworkBehaviour networkBehaviour)
    {
        //TODO needs to have the logs added back in when the debug system is fully setup
        if (networkBehaviour.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Server)
        {
            CLLogger.Instance.DebugLog($"IsServerCall returned True",DebugLevel.ItemEvent);
            return true;
        }

        CLLogger.Instance.DebugLog($"IsServerCall returned false",DebugLevel.ItemEvent);
        return false;
    }
}