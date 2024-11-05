using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class RoundPatches
{
    //TODO Hamunii pop ur patch here, but remember to branch off
    public static void Init()
    {
        Debug.Log("Patching Round Methods");
        On.StartOfRound.ShipLeave += StartOfRoundOnShipLeave;
    }
    
    private static void StartOfRoundOnShipLeave(On.StartOfRound.orig_ShipLeave orig, StartOfRound self)
    {
        EnemyManager.Instance.UnRegisterAllEnemies();
        orig(self);
    }
    
}