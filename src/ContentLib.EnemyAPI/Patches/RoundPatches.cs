using ContentLib.Core.Model.Managers;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class RoundPatches
{
    //TODO Hamunii pop ur patch here, but remember to branch off
    public static void Init()
    {
        On.StartOfRound.ShipLeave += StartOfRoundOnShipLeave;
    }
    
    private static void StartOfRoundOnShipLeave(On.StartOfRound.orig_ShipLeave orig, StartOfRound self)
    {
        EntityManager.Instance.UnRegisterAllEntities();
        orig(self);
    }
    
}