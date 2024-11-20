using System;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Coilhead;
using ContentLib.entityAPI.Model.entity;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class CoilheadPatches
{
    //TODO find the best way to get the SpringManAI (God damnit zeekers)
    public static void Init() => On.EnemyAI.Start += EnemyAI_Start;

    private static void EnemyAI_Start(On.EnemyAI.orig_Start orig, EnemyAI self)
    {
        orig(self);
        if (self is SpringManAI springManAI)
        {
            CoilheadSpawn(springManAI);
        }
    }

    private static void CoilheadSpawn(SpringManAI springManAI)
    {
        var coilhead = new LocalCoilhead(springManAI);
        EntityManager.Instance.RegisterEntity(coilhead);
    }

    private class LocalCoilhead(SpringManAI springManAI) : ICoilhead
    {
        public ulong Id => springManAI.NetworkObjectId;
        public bool IsAlive => !springManAI.isEnemyDead;
        public int Health => springManAI.enemyHP;
        public Vector3 Position => springManAI.serverPosition;
        public IEnemyProperties EnemyProperties => new CoilheadProperties(springManAI.enemyType);
        public bool IsSpawned => springManAI.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => springManAI.movingTowardsTargetPlayer;
        public void OnCollideWithPlayer(Collider other) => springManAI.OnCollideWithPlayer(other);

        public void SetAnimationStopServerRpc() => springManAI.SetAnimationStopServerRpc();

        public void SetAnimationStopClientRpc() => springManAI.SetAnimationStopClientRpc();

        public void SetAnimationGoServerRpc() => springManAI.SetAnimationGoServerRpc();

        public void SetAnimationGoClientRpc() => springManAI.SetAnimationGoClientRpc();

        public AISearchRoutine SearchForPlayers() => springManAI.searchForPlayers;

        public CoilheadBehaviourState State => throw new NotImplementedException();
    }

    private class CoilheadProperties(EnemyType type) : IEnemyProperties
    {
        public string Name { get; set; }
        public bool IsCustom { get; }
        public Type EnemyClassType => typeof(ICoilhead);
        public bool SpawningDisabled => type.spawningDisabled;
        public AnimationCurve ProbabilityCurve => type.probabilityCurve;

        public GameObject EnemyPrefab
        {
            get => type.enemyPrefab;
            set => type.enemyPrefab = value;
        }

        public bool IsOutsideEnemy => type.isOutsideEnemy;
        public bool IsDaytimeEnemy => type.isDaytimeEnemy;
        public bool SpawnFromWeeds => type.spawnFromWeeds;

        public AnimationCurve SpawnWeightMultiplier
        {
            get => type.probabilityCurve;
            set => type.probabilityCurve = value;
        }
    

    public int MaxCount
    {
        get => type.MaxCount;
        set => type.MaxCount = value;
        
    }

    public float PowerLevel
    {
        get => type.PowerLevel;
        set => type.PowerLevel = value;
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
}