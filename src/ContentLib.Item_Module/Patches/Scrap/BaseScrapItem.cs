using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap;

public abstract class BaseScrapItem(GrabbableObject grabbableObject) : IScrap
{
    
    public ulong Id => grabbableObject.NetworkObjectId;
    public string Name => ScrapName;
    public float Weight
    {
        get => grabbableObject.itemProperties.weight;
        set => grabbableObject.itemProperties.weight = value;
        
    }

    public bool IsOnShip => grabbableObject.isInShipRoom;
    public Vector3 Location => grabbableObject.transform.position;
    public IGameEntity? Owner { get; set; }
    public int ScrapValue => grabbableObject.scrapValue;
    protected abstract string ScrapName { get; }
}