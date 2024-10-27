using System.Linq;
using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Entity.Enemy;
using ContentLib.API.Model.Entity.Enemy.Vanilla.OldBird;
using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Event;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;
using Random = System.Random;

namespace ContentLib.EnemyAPI.Patches;

public class OldBirdPatches
{
    public static void Init()
    {
        CLLogger.Instance.Log("Patching Old Bird");
        On.RoundManager.SpawnNestObjectForOutsideEnemy += RoundManagerOnSpawnNestObjectForOutsideEnemy;
        On.EnemyAI.UseNestSpawnObject += EnemyAIOnUseNestSpawnObject;
    }
  
    private static void RoundManagerOnSpawnNestObjectForOutsideEnemy(On.RoundManager.orig_SpawnNestObjectForOutsideEnemy orig, RoundManager self, EnemyType enemytype, Random randomseed)
    {
        orig(self, enemytype, randomseed);
        //TODO get the actual name of the EnemyType name "Old Bird"
        if (enemytype.enemyName == "radMech")
        {
            EnemyAINestSpawnObject? lastSpawnedNest = self.enemyNestSpawnObjects.LastOrDefault();
            if (lastSpawnedNest == null) return;
            var oldBird = new BaseOldBirdEnemy(lastSpawnedNest);
            EnemyManager.Instance.RegisterEnemy(oldBird);
            GameEventManager.Instance.Trigger(new OldBirdSpawnEvent(oldBird));

        }

    }
    private static void EnemyAIOnUseNestSpawnObject(On.EnemyAI.orig_UseNestSpawnObject orig, EnemyAI self, EnemyAINestSpawnObject nestSpawnObject)
    {
        orig(self, nestSpawnObject);
        if (self is not RadMechAI radMechAI) return;
        var enemyId = (ulong)nestSpawnObject.GetInstanceID();
        if (EnemyManager.Instance.GetEnemy(enemyId) is not BaseOldBirdEnemy oldBirdEnemy) return;
        oldBirdEnemy.SetAwakeAI(radMechAI);
        EnemyManager.Instance.UnRegisterEnemy(enemyId);
        EnemyManager.Instance.RegisterEnemy(oldBirdEnemy);
        GameEventManager.Instance.Trigger(new OldBirdWakeEvent(oldBirdEnemy));
    }


    private class BaseOldBirdEnemy : IOldBird
{
    private RadMechAI? _oldBirdAI;
    private ulong _oldBirdID;
    public BaseOldBirdEnemy(EnemyAINestSpawnObject oldBirdNest)
    {
        DormantPrefab = oldBirdNest.gameObject;
        IsAwake = false;
        Id = (ulong) oldBirdNest.gameObject.GetInstanceID();
    }

    public void SetAwakeAI(RadMechAI radMechAI) => _oldBirdAI = radMechAI;

    public ulong Id
    {
        get => _oldBirdAI != null ? _oldBirdAI.NetworkObjectId : _oldBirdID;
        private set => _oldBirdID = value;
    }

    public bool IsAlive => _oldBirdAI != null && !_oldBirdAI.isEnemyDead;

    public int Health => 100;

    public Vector3 Position => _oldBirdAI?.transform.position ?? Vector3.zero;

    public IEnemyProperties EnemyProperties { get; }

    public bool IsSpawned => _oldBirdAI != null && _oldBirdAI.IsSpawned;

    public bool IsHostile => true;

    public bool IsChasing => _oldBirdAI != null && _oldBirdAI.targetedThreat != null;

    public GameObject DormantPrefab { get; }

    public GameObject? AwakePrefab => _oldBirdAI?.enemyType.enemyPrefab;

    public bool IsAwake { get; set; }

    public IGameEntity? CurrentlyHeldEntity { get; set; }

    public IGameEntity? CurrentTarget { get; set; }

    public Transform CurrentTargetNode
    {
        get => _oldBirdAI.targetNode;
        set=>_oldBirdAI.targetNode = value; }

    public bool IsRoaming() => throw new System.NotImplementedException();

    public bool IsHoldingPlayer() => _oldBirdAI != null && _oldBirdAI.inSpecialAnimationWithPlayer != null;

    public bool IsAlerted() => _oldBirdAI != null && _oldBirdAI.isAlerted;

    public bool IsFlying() => _oldBirdAI != null && _oldBirdAI.inFlyingMode;

    public void StartFlying() => throw new System.NotImplementedException();

    public void StopFlying(Vector3 landingPosition) => throw new System.NotImplementedException();

    public void SetChargingForward(IGameEntity chargingTarget) => throw new System.NotImplementedException();

    public void FireMissilesAtTargetPosition(Vector3 targetPosition, Vector3 startRotation)
    {
        if (_oldBirdAI != null)
        {
            _oldBirdAI.ShootGun(targetPosition, startRotation);
        }
    }

    public void BlowTorchPlayer() => throw new System.NotImplementedException();

}


    private class OldBirdSpawnEvent(IEnemy oldBird) : MonsterSpawnEvent
    {
        public override IEnemy Enemy => oldBird;
    }

    private class OldBirdWakeEvent(IEnemy oldBird) : WakeableMonsterWakeEvent
    {
        public override IEnemy Enemy => oldBird;
    }
    
}