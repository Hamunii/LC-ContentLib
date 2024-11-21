using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Item;
using ContentLib.Core.Utils;
using ContentLib.entityAPI.Model.entity;
using ContentLib.Item_Module.Model;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap;

public class RemotePatches
{
    public static void Init()
    {
        On.GrabbableObject.Start += GrabbableObjectOnStart;
    }

    private static void GrabbableObjectOnStart(On.GrabbableObject.orig_Start orig, GrabbableObject self)
    {
        orig(self);
        if (self is not RemoteProp remoteItem)
        {
            CLLogger.Instance.DebugLog("Remote item check was found to be null");
            return;
        }

        IRemoteControlScrap remoteControlScrap = new BaseRemoteControlScrap(self, remoteItem);
        ItemManager.Instance.RegisterItem(remoteControlScrap);

    }

    private class BaseRemoteControlScrap(GrabbableObject grabbableObject, RemoteProp remoteItem): IRemoteControlScrap
    {
        public ulong Id => grabbableObject.NetworkObjectId;
        public string Name => "Remote Control";
        public float Weight { get; set; }
        public bool IsOnShip => grabbableObject.isInShipRoom;
        public Vector3 Location => grabbableObject.transform.position;
        public IGameEntity? Owner { get; set; }

        public int ScrapValue => grabbableObject.scrapValue;
    }
}