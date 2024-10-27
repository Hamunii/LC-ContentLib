using ContentLib.API.Model.Entity.Enemy;
using ContentLib.API.Model.Entity.Enemy.Vanilla.OldBird;
using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class OldBirdPatches
{
    public static void Init()
    {
        //TODO This is NOT actually the spawning, it is the waking up due to zeekers being an unforgiveable twat who
        //TODO makes my life a living hell. 
        On.RadMechAI.Start += RadMechAIOnStart;
    }

    private static void RadMechAIOnStart(On.RadMechAI.orig_Start originalMethod, RadMechAI self)
    {
        originalMethod(self);
        IEnemy enemy = new BaseOldBirdEnemy(self);
        EnemyManager.Instance.RegisterEnemy(enemy); 
        GameEventManager.Instance.Trigger(new OldBirdSpawnEvent(enemy));
    }

    private class BaseOldBirdEnemy(RadMechAI oldBirdAI) : IOldBird
    {
        public ulong Id => oldBirdAI.NetworkObjectId;
      
        public bool IsAlive => !oldBirdAI.isEnemyDead;

        public int Health => 100;
        public Vector3 Position => oldBirdAI.transform.position;
        public IEnemyProperties EnemyProperties { get; }
        public bool IsSpawned => oldBirdAI.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => oldBirdAI.targetedThreat != null;
        public GameObject DormantPrefab => oldBirdAI.enemyType.nestSpawnPrefab;
        public GameObject AwakePrefab => oldBirdAI.enemyType.enemyPrefab;
        public bool IsAwake { get; set; }
        public bool IsRoaming() => throw new System.NotImplementedException();

        public bool IsHoldingPlayer() => throw new System.NotImplementedException();

        public bool IsAlerted() => throw new System.NotImplementedException();

        public void StartFlying(Vector3 landingPosition) => throw new System.NotImplementedException();

        public void StopFlying() => throw new System.NotImplementedException();

        public void SetChargingForward(bool isCharging) => throw new System.NotImplementedException();

        public void FireMissilesAtTargetPosition(Vector3 targetPosition) => throw new System.NotImplementedException();

        public void BlowTorchPlayer() => throw new System.NotImplementedException();
    }

    private class OldBirdSpawnEvent(IEnemy oldBird) : MonsterSpawnEvent
    {
        public override IEnemy Enemy => oldBird;
    }
    
}