using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Entity.Enemy;
using ContentLib.Core.Model.Managers;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class EnemyAIPatches
{
    public static void Init()
    {
        On.EnemyAI.Start += EnemyAIOnStart;
        On.EnemyAI.KillEnemy += EnemyAIOnKillEnemy;
    }
    

    private static void EnemyAIOnStart(On.EnemyAI.orig_Start orig, EnemyAI self)
    {
        orig(self);
        try
        {
            if (self is CustomEnemyAI customAI)
                EntityManager.Instance.RegisterEntity(customAI);
        }
        catch (InvalidEntityRegistrationException exception)
        {
            CLLogger.Instance.DebugLog(exception.Message);
        }
     
    }

    private static void EnemyAIOnKillEnemy(On.EnemyAI.orig_KillEnemy orig, EnemyAI self, bool destroy)
    {
        orig(self, destroy);
        EntityManager.Instance.UnRegisterEntity(self.NetworkObjectId);
    }
    

    
}