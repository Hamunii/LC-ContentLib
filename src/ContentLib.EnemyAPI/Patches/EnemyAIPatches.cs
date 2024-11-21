using ContentLib.API.Model.Entity.Enemy;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.entityAPI.Model.entity;
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
        if(self is CustomEnemyAI customAI)
            EntityManager.Instance.RegisterEntity(customAI);
    }

    private static void EnemyAIOnKillEnemy(On.EnemyAI.orig_KillEnemy orig, EnemyAI self, bool destroy)
    {
        orig(self, destroy);
        EntityManager.Instance.UnRegisterEntity(self.NetworkObjectId);
    }
    

    
}