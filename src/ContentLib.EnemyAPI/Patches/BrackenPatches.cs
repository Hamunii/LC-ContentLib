using System;
using ContentLib.API.Model.Event;
using ContentLib.EnemyAPI.Events;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using ContentLib.entityAPI.Model.entity;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class BrackenPatches
{
    public static void Init()
    {
        Debug.Log("Bracken Patches");
        On.FlowermanAI.Start += FlowermanAI_Start;
        On.FlowermanAI.OnCollideWithPlayer += FlowerManAI_OnCollideWithPlayer;

    }
    private static void FlowermanAI_Start(On.FlowermanAI.orig_Start orig,FlowermanAI self)
    {
        orig(self);
        Debug.Log("BrackenSpawnPatch");
        IEnemy vanillaBrackenEnemy = new LocalBracken(self);
        Debug.Log("Bracken registration");
        EntityManager.Instance.RegisterEntity(vanillaBrackenEnemy);
    }
    private static void FlowerManAI_OnCollideWithPlayer(On.FlowermanAI.orig_OnCollideWithPlayer orig, FlowermanAI self, Collider other)
    {
        orig(self, other);
        var enemy = (IEnemy) EntityManager.Instance.GetEntity(self.NetworkObjectId);
        GameEventManager.Instance.Trigger(new LocalBrackenMonsterKillPlayerEvent(enemy));
    }
    private class LocalBracken(FlowermanAI brackenAi) : IBracken
    {
        // IEnemy / IEntity METHODS
        public ulong Id => brackenAi.NetworkObjectId;
        public bool IsAlive => !brackenAi.isEnemyDead;
        public int Health => brackenAi.enemyHP;
        public Vector3 Position => brackenAi.transform.position;
        public IEnemyProperties EnemyProperties => new BrackenProperties(brackenAi);
        public bool IsSpawned => brackenAi.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => brackenAi.movingTowardsTargetPlayer & brackenAi.isInAngerMode;
        
        //IBracken METHODS:
        //-----------------------------------------------------------------------------
        
        public void AddToAngerMeter(float amountToAdd) => brackenAi.AddToAngerMeter(amountToAdd);

        public float Anger => brackenAi.angerMeter;
        public bool IsAngry => brackenAi.isInAngerMode;
        public void EnterAngerModeServerRpc(float angerTime) => brackenAi.EnterAngerModeServerRpc(angerTime);

        public void EnterAngerModeClientRpc(float angerTime) => brackenAi.EnterAngerModeClientRpc(angerTime);

        public void AvoidClosestPlayer() => brackenAi.AvoidClosestPlayer();

        public void LookAtTrigger(int playerId) => brackenAi.LookAtFlowermanTrigger(playerId);

        public void ResetStealthTimerServerRpc(int playerObj) => brackenAi.ResetFlowermanStealthTimerServerRpc(playerObj);

        public void ResetStealthTimerClientRpc(int playerObj) => brackenAi.ResetFlowermanStealthClientRpc(playerObj);


        public void Kill(Vector3 bodyVelocity, bool spawnBody = true, CauseOfDeath causeOfDeath = CauseOfDeath.Unknown,
            int deathAnimation = 0, Vector3 positionOffset = default(Vector3)) =>
            brackenAi.KillEnemy();
    }

    private class BrackenProperties(FlowermanAI flowermanAI) : IEnemyProperties
    {
        private EnemyType _type = flowermanAI.enemyType;
        public string Name { get; set; }
        public bool IsCustom => false;
        public Type EnemyClassType => typeof(IBracken);
        public bool SpawningDisabled { get; }
        public AnimationCurve ProbabilityCurve { get; }

        public GameObject EnemyPrefab
        {
            get => _type.enemyPrefab;
            set => _type.enemyPrefab = value;
        }

        public bool IsOutsideEnemy { get; set; }
        public bool IsDaytimeEnemy { get; set; }
        public bool SpawnFromWeeds { get; set; }

        public AnimationCurve SpawnWeightMultiplier
        {
            get => _type.probabilityCurve;
            set => _type.probabilityCurve = value;
        }

        public int MaxCount
        {
            get => _type.MaxCount;
            set => _type.MaxCount = value;
        }

        public float PowerLevel
        {
            get => _type.PowerLevel;
            set => _type.PowerLevel = value; 
        }

        public bool CanBeStunned { get; set; }
        public bool CanDie { get; set; }
        public bool DestroyOnDeath { get; set; }
        public float StunTimeMultiplier { get; set; }
        public float DoorSpeedMultiplier { get; set; }
        public float StunGameDifficultyMultiplier { get; set; }
        public bool CanSeeThroughFog { get; set; }
        public float PushPlayerForce { get; set; }
        public float PushPlayerDistance { get; set; }
        public AudioClip HitBodySFX { get; set; }
        public AudioClip HitEnemyVoiceSFX { get; set; }
        public AudioClip DeathSFX { get; set; }
        public AudioClip StunSFX { get; set; }
        public MiscAnimation[] MiscAnimations { get; set; }
        public AudioClip[] AudioClips { get; set; }
        public float TimeToPlayAudio { get; set; }
        public float LoudnessMultiplier { get; set; }
        public AudioClip OverrideVentSFX { get; set; }
        public IEnemyHordeProperties? HordeProperties { get; }
    }

    private class LocalBrackenMonsterKillPlayerEvent(IEnemy enemy) : MonsterKillsPlayerEvent
    {
        public override IEnemy Enemy => enemy;
    }
    
    private class LocalBrackenSpawnEvent(IEnemy enemy): MonsterSpawnEvent
    {
        public override IEnemy Enemy => enemy;
    }
}

