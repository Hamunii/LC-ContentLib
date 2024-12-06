using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Event;
using ContentLib.API.Model.Item;
using ContentLib.API.Model.Item.Scrap;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.Item_Module.Patches.Scrap;

public class SoccerBallPatches
{
    public static void Init()
    {
        On.SoccerBallProp.Start += SoccerBallPropOnStart;
        On.SoccerBallProp.KickBallServerRpc += SoccerBallPropOnKickBallServerRpc;
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
    
    //TODO this has brought up an issue where Zeekers sometimes uses ints to represent which player does a thing, need
    //TODO overload the method in EntityManager to grab player entities when presenting with an int param.
    private static void SoccerBallPropOnKickBallServerRpc(On.SoccerBallProp.orig_KickBallServerRpc orig, SoccerBallProp self, Vector3 dest, int playerwhokicked, bool setinelevator, bool setinshiproom)
    {
        var isServerCall = IsServerCall(self);
        orig(self, dest, playerwhokicked, setinelevator, setinshiproom);
        if (!isServerCall)
            return;
        if (ItemManager.Instance.GetItem(self.NetworkObjectId) is not ISoccerBall soccerBall)
            return;
        ItemMovedEvent movedEvent = new KickSoccerBallEvent(soccerBall, dest);
        GameEventManager.Instance.Trigger(movedEvent);
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

    private class KickSoccerBallEvent(ISoccerBall soccerBall, Vector3 destination) : ItemMovedEvent
    {
        public override Vector3 Position => soccerBall.Location;
        public override IGameItem? Item => soccerBall;
        public override Vector3 Destination => destination;
    }
    
    private static bool IsServerCall(NetworkBehaviour networkBehaviour)
    {
        if (networkBehaviour.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Server)
        {
            CLLogger.Instance.DebugLog($"IsServerCall returned True",DebugLevel.CoreEvent);
            return true;
        }

        CLLogger.Instance.DebugLog($"IsServerCall returned false",DebugLevel.CoreEvent);
        return false;
    }
}