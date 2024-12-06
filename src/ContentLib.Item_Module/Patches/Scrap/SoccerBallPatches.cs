using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item.Scrap;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap;

public class SoccerBallPatches
{
    public static void Init()
    {
        On.SoccerBallProp.Start += SoccerBallPropOnStart;
    }

    private static void SoccerBallPropOnStart(On.SoccerBallProp.orig_Start orig, SoccerBallProp self)
    {
        orig(self);
        ISoccerBall soccerBall = new BaseSoccerBall(self);
        try
        {
            ItemManager.Instance.RegisterItem(soccerBall);
        }
        catch (InvalidItemRegistrationException ex)
        {
            CLLogger.Instance.DebugLog(ex.ToString());
        }
        
    }
    
    private class BaseSoccerBall(SoccerBallProp soccerBallProp) : ISoccerBall
    {
        public ulong Id => soccerBallProp.NetworkObjectId;
        public string Name => soccerBallProp.name;
        public float Weight
        {
            get => soccerBallProp.itemProperties.weight;
            set => soccerBallProp.itemProperties.weight = value;
        }

        public bool IsOnShip => soccerBallProp.isInShipRoom;
        public Vector3 Location => soccerBallProp.transform.position;
        public IGameEntity? Owner { get; set; }
        public int ScrapValue => soccerBallProp.scrapValue;
        public ScrapType Type => ScrapType.SoccerBall;
        public Vector3 Velocity => soccerBallProp.transform.forward;
    }
}