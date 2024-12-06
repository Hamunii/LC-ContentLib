using System;
using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Scrap;
using ContentLib.Core.Model.Managers;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap;

public class ScrapPatches
{
    public static void Init()
    {
        On.GrabbableObject.Start += GrabbableObjectOnStart;
    }

    private static void GrabbableObjectOnStart(On.GrabbableObject.orig_Start orig, GrabbableObject self)
    {
        orig(self);
        ScrapType? scrapType = GetScrapType(self.itemProperties.itemName);
        if (!scrapType.HasValue)
            return;
        IVanillaScrap vanillaScrap = new VanillaScrapItem(self, scrapType.Value);
        ItemManager.Instance.RegisterItem(vanillaScrap);
        GameEventManager.Instance.Trigger(new VanillaScrapSpawnEvent(vanillaScrap));
    }


    private class VanillaScrapItem(GrabbableObject grabbableObject, ScrapType type) : IVanillaScrap
    {
        public ulong Id => grabbableObject.NetworkObjectId;
        public string Name => grabbableObject.name;
        public float Weight
        {
            get => grabbableObject.itemProperties.weight;
            set => throw new NotImplementedException();
        }

        public bool IsOnShip => grabbableObject.isInShipRoom;
        public Vector3 Location => grabbableObject.transform.position;
        public IGameEntity? Owner { get; set; }
        public int ScrapValue => grabbableObject.scrapValue;
        public ScrapType Type => type;
    }

    private static ScrapType? GetScrapType(string itemName)
    {
        var refactoredName = itemName.Replace(" ", "").Replace("-", "").ToLower();
        
        Array scrapTypes = Enum.GetValues(typeof(ScrapType));

        for (var i = 0; i < scrapTypes.Length; i++)
        {
            var type = (ScrapType)scrapTypes.GetValue(i);
            if(type.ToString().ToLower().Contains(refactoredName)){return type;}
        }
        return null;
    }

    private class VanillaScrapSpawnEvent(IVanillaScrap scrap) : ItemSpawnedEvent
    {
        public override Vector3 Position => scrap.Location;
        public override IGameItem? Item => scrap;
    }
}