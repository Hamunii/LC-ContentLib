using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using ContentLib.Item_Module.Events;
using ContentLib.Item_Module.Model;
using On.GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.Item_Module.Patches;

public class ItemPatches
{
    public void init()
    {
        On.GameNetcodeStuff.PlayerControllerB.GrabObjectClientRpc += PlayerControllerBOnGrabObjectClientRpc; 
        
    }

    private void PlayerControllerBOnGrabObjectClientRpc(PlayerControllerB.orig_GrabObjectClientRpc orig, GameNetcodeStuff.PlayerControllerB self, bool grabvalidated, NetworkObjectReference grabbedobject)
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
        public override IGameEntity GrabbingEntity { get; }
    }
}