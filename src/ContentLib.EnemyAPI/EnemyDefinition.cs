using System;
using System.Collections.Generic;
using ContentLib.API.Exceptions.Entity;
using ContentLib.Core;
using ContentLib.Core.Networking;
using ContentLib.Core.Tags;
using ContentLib.EnemyAPI.Internal;
using UnityEngine;

namespace ContentLib.EnemyAPI;

/// <summary>
/// Base class for custom content that can be registered.
/// </summary>
[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "ContentLib/EnemyAPI/EnemyDefinition", order = 0)]
public class EnemyDefinition : ContentDefinition
{
    /// <summary>
    /// The Vanilla EnemyType ScriptableObject.
    /// </summary>
    [field: SerializeField] public EnemyType EnemyType { get; set; } = null!;

    /// <summary>
    /// Tags for matching and getting a weight for injecting this enemy to inside enemies spawn pool.
    /// </summary>
    /// <remarks>
    /// If no tags match, the enemy won't be injected into this spawn pool.
    /// </remarks>
    [field: SerializeField] public List<LevelMatchingTags> InsideLevelMatchingTags { get; set; } = [];

    /// <summary>
    /// Tags for matching and getting a weight for injecting this enemy to outside enemies spawn pool.
    /// </summary>
    /// <inheritdoc cref="InsideLevelMatchingTags"/>
    [field: SerializeField] public List<LevelMatchingTags> OutsideLevelMatchingTags { get; set; } = [];

    /// <summary>
    /// Tags for matching and getting a weight for injecting this enemy to daytime enemies spawn pool.
    /// </summary>
    /// <inheritdoc cref="InsideLevelMatchingTags"/>
    [field: SerializeField] public List<LevelMatchingTags> DaytimeLevelMatchingTags { get; set; } = [];
    
    /// <inheritdoc cref="IgnoreEnemyDefinitionValidationFlags"/>
    [field: SerializeField] public IgnoreEnemyDefinitionValidationFlags IgnoreValidationFlags { get; set; } = 0;

    /// <summary>
    /// Creates a new instance of <see cref="EnemyDefinition"/>.
    /// </summary>
    /// <param name="enemyType">A vanilla EnemyType.</param>
    /// <returns>A new instance of <see cref="EnemyDefinition"/>.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static EnemyDefinition Create(EnemyType enemyType)
    {
        EnemyDefinition enemyDefinition = CreateInstance<EnemyDefinition>();
        enemyDefinition.EnemyType = enemyType ?? throw new ArgumentNullException(nameof(enemyType));

        if (string.IsNullOrEmpty(enemyType.name))
            throw new ArgumentException($"{nameof(enemyType)}.{nameof(enemyType.name)} is null!");

        enemyDefinition.name = $"{enemyType.name} EnemyDefinition";
        return enemyDefinition;
    }

    /// <inheritdoc/>
    public override (bool isValid, string? message) Validate()
    {
        (bool isValid, string? message) result = base.Validate();

        ValidateEnemyType(ref result);

        // We need the above stuff to be valid for the next part.
        if (!result.isValid)
            return result;

        ValidateEnemyPrefab(ref result);

        if (EnemyDefinitionInjector.s_lateForRegister)
            MarkAsInvalid(ref result, $"Registration window has closed!");

        if (IsRegistered)
            MarkAsInvalid(ref result, $"EnemyDefinition '{name}' has already been registered!");

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="EnemyDefinitionRegistrationException"></exception>
    public override void Register()
    {
        (var isValid, var message) = Validate();

        if (!isValid)
            throw new EnemyDefinitionRegistrationException(message!);

        NetworkPrefabManager.RegisterNetworkPrefab(EnemyType.enemyPrefab);

        EnemyDefinitionInjector.Register(this);
        IsRegistered = true;
    }

    private void ValidateEnemyType(ref (bool isValid, string? message) result) 
    {
        if (EnemyType == null)
        {
            MarkAsInvalid(ref result, $"{nameof(EnemyType)} is null!");
            return;
        }

        if (EnemyType.enemyPrefab == null)
        {
            MarkAsInvalid(ref result, $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)} is null!");
            return;
        }

        ValidateLayerAndTag(ref result, EnemyType.enemyPrefab, "Enemies", "Enemy", nameof(EnemyType.enemyPrefab));
        
        if (string.IsNullOrEmpty(EnemyType.enemyName))
        {
            MarkAsInvalid(ref result, $"{nameof(EnemyType)}{nameof(EnemyType.enemyName)} is null or empty!");
            return;
        }
    }

    private void ValidateEnemyPrefab(ref (bool isValid, string? message) result)
    {
        var enemyPrefabPath = $"{nameof(EnemyType)}.{nameof(EnemyType.enemyPrefab)}";

        // The enemy spawning explodes without an EnemyAI component.
        EnemyAI enemyAI = EnemyType.enemyPrefab.GetComponent<EnemyAI>();
        if (enemyAI == null)
        {
            MarkAsInvalid(ref result, $"{enemyPrefabPath}.{nameof(enemyAI)} is null!");
            return;
        }

        // Needed for the enemy to reference important base values.
        if (enemyAI.enemyType == null)
            MarkAsInvalid(ref result, $"{enemyPrefabPath}.{nameof(enemyAI)}.{nameof(enemyAI.enemyType)} is null!");

        // Needed for the enemy to move around the world.
        if (enemyAI.agent == null)
            MarkAsInvalid(ref result, $"{enemyPrefabPath}.{nameof(enemyAI)}.{nameof(enemyAI.agent)} is null!");

        // Needed to animate the enemy.
        if (enemyAI.creatureAnimator == null)
            MarkAsInvalid(ref result, $"{enemyPrefabPath}.{nameof(enemyAI)}.{nameof(enemyAI.creatureAnimator)} is null!");

        // Needed so that the game knows what states you're switching when using SwitchBehaviour.
        // Not an issue if not using the game's Behaviour system, so should this be checked?
        // if (enemyAI.enemyBehaviourStates == null || enemyAI.enemyBehaviourStates.Length == 0)
        //     WarnBySeverity($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAI)}{nameof(enemyAI.enemyBehaviourStates)} is null!",
        //     (message) => throw new NullReferenceException(message));

        // Needed so enemies can be scanned.
        ScanNodeProperties[] scanNodeProperties = EnemyType.enemyPrefab.GetComponentsInChildren<ScanNodeProperties>();
        if (scanNodeProperties.Length == 0)
        {
            MarkAsInvalid(ref result, IgnoreEnemyDefinitionValidationFlags.NoScanNode,
                $"{nameof(EnemyType.enemyPrefab)} '{EnemyType.enemyPrefab.name}' doesn't have any {nameof(ScanNodeProperties)} components! It can't be scanned.");
        }
        else
        {
            // Needed so it can be scanned and not scanned when holding (needs tag to be untagged).
            foreach (ScanNodeProperties scanNode in scanNodeProperties)
            {
                ValidateLayerAndTag(ref result, scanNode.gameObject, "ScanNode", "Untagged",
                    $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(scanNodeProperties)}");
            }
        }

        // Need a better way for checking for a map dot than an object with a specific name.
        // Transform mapDot = EnemyType.enemyPrefab.transform.Find("MapDot");
        // if (mapDot == null)
        //     throw new NullReferenceException($"{nameof(EnemyType)} '{EnemyType.enemyPrefab.name}' doesn't have a 'MapDot' GameObject!"); // Needed to display on the radars in-game.

        // ValidateLayerAndTag(mapDot.gameObject, "MapRadar", "DoNotSet", $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(EnemyType.enemyPrefab.transform)} MapDot"); // Needed if the MapDot does exist.
        
        // Needed for collision with enemies, players and doors.
        EnemyAICollisionDetect[] collisionDetectComponents = EnemyType.enemyPrefab.GetComponentsInChildren<EnemyAICollisionDetect>();
        if (collisionDetectComponents.Length == 0)
        {
            MarkAsInvalid(ref result, IgnoreEnemyDefinitionValidationFlags.NoCollisionDetect,
                $"Enemy '{EnemyType.enemyName}' doesn't reference any EnemyAI Collision Detect Scripts!");
        }
        foreach (EnemyAICollisionDetect collisionDetect in collisionDetectComponents)
        {
            if (collisionDetect.mainScript == null)
            {
                MarkAsInvalid(ref result,
                    $"An Enemy AI Collision Detect Script on GameObject '{collisionDetect.gameObject.name}' of enemy '{EnemyType.enemyName}' does not reference a 'Main Script', and could cause Null Reference Exceptions.");
            }

            ValidateLayerAndTag(ref result, collisionDetect.gameObject, "Enemies", "Enemy",
                $"Enemy '{EnemyType.enemyName}' has invalid layer or tag on a GameObject with an {nameof(EnemyAICollisionDetect)} Script!");

            // Needed for opening doors and stuff.
            if (collisionDetect.gameObject.GetComponent<Rigidbody>() == null)
            {
                MarkAsInvalid(ref result, IgnoreEnemyDefinitionValidationFlags.NoRigidBodyOnCollisionDetect,
                    $"An Enemy AI Collision Detect Script on GameObject '{collisionDetect.gameObject.name}' of enemy '{EnemyType.enemyName}' does not have a {nameof(Rigidbody)} Component attached, which prevents the enemy from opening doors!");
            }
        }

    
        // Considered adding a check for a box collider in the root GameObject/prefab, would be needed so that other enemies can see your enemy in the case of inheriting IVisibleThreat interface.
    }

    private void ValidateLayerAndTag(
        ref (bool isValid, string? message) result,
        GameObject gameObject,
        string requiredLayer,
        string requiredTag,
        string errorMessagePrefix
    )
    {
        if (gameObject.layer != LayerMask.NameToLayer(requiredLayer) || gameObject.tag != requiredTag)
        {
            MarkAsInvalid(ref result,
                $"{errorMessagePrefix} is not set on the correct layer ('{requiredLayer}') or tag ('{requiredTag}')!");
        }
    }

    private void MarkAsInvalid(ref (bool isValid, string? message) result, IgnoreEnemyDefinitionValidationFlags thisCheckFlag, string message)
    {
        if (IgnoreValidationFlags.HasFlag(thisCheckFlag))
            return;

        MarkAsInvalid(ref result, message);
    }
}
