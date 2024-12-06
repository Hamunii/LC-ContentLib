using System;
using ContentLib.API.Model.Mods;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Custom.ScriptableObject;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyType", order = 1)]
public class CustomEnemyTypeSO : UnityEngine.ScriptableObject, IEnemyProperties, ICustomContentProperties
{
    [SerializeField]
    private string classPath;
    [SerializeField] private string name;
    public string Name { get => name; set => name = value; }
    public bool IsCustom => true;
    public Type EnemyClassType { get; }
    
    [SerializeField] private bool spawningDisabled;
    public bool SpawningDisabled => spawningDisabled; // Public property with only a get accessor

    [SerializeField] private AnimationCurve probabilityCurve;
    public AnimationCurve ProbabilityCurve => probabilityCurve; // Public property with only a get accessor


    [SerializeField] private GameObject enemyPrefab;
    public GameObject EnemyPrefab { get => enemyPrefab; set => enemyPrefab = value; }

    [SerializeField] private bool isOutsideEnemy;
    public bool IsOutsideEnemy { get => isOutsideEnemy; set => isOutsideEnemy = value; }

    [SerializeField] private bool isDaytimeEnemy;
    public bool IsDaytimeEnemy { get => isDaytimeEnemy; set => isDaytimeEnemy = value; }

    [SerializeField] private bool spawnFromWeeds;
    public bool SpawnFromWeeds { get => spawnFromWeeds; set => spawnFromWeeds = value; }

    [SerializeField] private AnimationCurve spawnWeightMultiplier;
    public AnimationCurve SpawnWeightMultiplier { get => spawnWeightMultiplier; set => spawnWeightMultiplier = value; }

    [SerializeField] private int maxCount;
    public int MaxCount { get => maxCount; set => maxCount = value; }

    [SerializeField] private float powerLevel;
    public float PowerLevel { get => powerLevel; set => powerLevel = value; }

    [SerializeField] private bool canBeStunned;
    public bool CanBeStunned { get => canBeStunned; set => canBeStunned = value; }

    [SerializeField] private bool canDie;
    public bool CanDie { get => canDie; set => canDie = value; }

    [SerializeField] private bool destroyOnDeath;
    public bool DestroyOnDeath { get => destroyOnDeath; set => destroyOnDeath = value; }

    [SerializeField] private float stunTimeMultiplier;
    public float StunTimeMultiplier { get => stunTimeMultiplier; set => stunTimeMultiplier = value; }

    [SerializeField] private float doorSpeedMultiplier;
    public float DoorSpeedMultiplier { get => doorSpeedMultiplier; set => doorSpeedMultiplier = value; }

    [SerializeField] private float stunGameDifficultyMultiplier;
    public float StunGameDifficultyMultiplier { get => stunGameDifficultyMultiplier; set => stunGameDifficultyMultiplier = value; }

    [SerializeField] private bool canSeeThroughFog;
    public bool CanSeeThroughFog { get => canSeeThroughFog; set => canSeeThroughFog = value; }

    [SerializeField] private float pushPlayerForce;
    public float PushPlayerForce { get => pushPlayerForce; set => pushPlayerForce = value; }

    [SerializeField] private float pushPlayerDistance;
    public float PushPlayerDistance { get => pushPlayerDistance; set => pushPlayerDistance = value; }

    // Audio Properties
    [SerializeField] private AudioClip hitBodySFX;
    public AudioClip HitBodySFX { get => hitBodySFX; set => hitBodySFX = value; }

    [SerializeField] private AudioClip hitEnemyVoiceSFX;
    public AudioClip HitEnemyVoiceSFX { get => hitEnemyVoiceSFX; set => hitEnemyVoiceSFX = value; }

    [SerializeField] private AudioClip deathSFX;
    public AudioClip DeathSFX { get => deathSFX; set => deathSFX = value; }

    [SerializeField] private AudioClip stunSFX;
    public AudioClip StunSFX { get => stunSFX; set => stunSFX = value; }

    [SerializeField] private MiscAnimation[] miscAnimations;
    public MiscAnimation[] MiscAnimations { get => miscAnimations; set => miscAnimations = value; }

    [SerializeField] private AudioClip[] audioClips;
    public AudioClip[] AudioClips { get => audioClips; set => audioClips = value; }

    [SerializeField] private float timeToPlayAudio;
    public float TimeToPlayAudio { get => timeToPlayAudio; set => timeToPlayAudio = value; }

    [SerializeField] private float loudnessMultiplier;
    public float LoudnessMultiplier { get => loudnessMultiplier; set => loudnessMultiplier = value; }

    [SerializeField] private AudioClip overrideVentSFX;
    public AudioClip OverrideVentSFX { get => overrideVentSFX; set => overrideVentSFX = value; }

    public IEnemyHordeProperties? HordeProperties => hordeProperties;

    [SerializeField]
    public EnemyHordeTypeSO? hordeProperties;

    public string ClassPath => classPath;
}