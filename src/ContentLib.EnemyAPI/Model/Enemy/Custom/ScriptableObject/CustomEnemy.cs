using System;
using System.Reflection;
using ContentLib.API.Model.Entity.Enemy;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Mods.Content.Types;
using ContentLib.Core.Model.Managers;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Custom.ScriptableObject;

internal class CustomEnemy(ICustomEnemy enemy) : EnemyAI
{
    public override void Start() => base.Start();

    public override void Update() => base.Update();

    public override void OnDestroy() => base.OnDestroy();

    public override void OnDrawGizmos() => InvokeOrFallback<ICustomEnemy>(customEnemy => customEnemy.OnDrawGizmos(),
        () => base.OnDrawGizmos(), nameof(ICustomEnemy.OnDrawGizmos));

    public override void SetEnemyStunned(bool setToStunned, float setToStunTime = 1,
        PlayerControllerB setStunnedByPlayer = null) =>
        InvokeOrFallback<ICustomEnemy>(
            e => e.SetEnemyStunned(setToStunned, setToStunTime,
                (IPlayer)EntityManager.Instance.GetEntity(setStunnedByPlayer.NetworkObjectId)),
            () => base.SetEnemyStunned(setToStunned, setToStunTime, setStunnedByPlayer),
            nameof(ICustomEnemy.SetEnemyStunned)
        );

    public override void UseNestSpawnObject(EnemyAINestSpawnObject nestSpawnObject) =>
        InvokeOrFallback<ICustomEnemy>(customEnemy => customEnemy.UseNestSpawnedObject(nestSpawnObject),
            () => base.UseNestSpawnObject(nestSpawnObject), nameof(ICustomEnemy.UseNestSpawnedObject));

    public override void OnCollideWithPlayer(Collider other) => InvokeOrFallback<ICustomEnemy>(customEnemy =>
            customEnemy.OnCollideWithPlayer(other),
        () => base.OnCollideWithPlayer(other),
        nameof(ICustomEnemy.OnCollideWithPlayer));

    public override void OnCollideWithEnemy(Collider other, EnemyAI collidedEnemy = null) =>
        InvokeOrFallback<ICustomEnemy>(customEnemy =>
                customEnemy.OnCollideWithEnemy(other,
                    (IEnemy)EntityManager.Instance.GetEntity(collidedEnemy.NetworkObjectId)),
            () => base.OnCollideWithEnemy(other, collidedEnemy = null), nameof(ICustomEnemy.OnCollideWithEnemy));

    public override void DoAIInterval() => InvokeOrFallback<ICustomEnemy>(customEnemy =>
            customEnemy.DoAIInterval(),
        () => base.DoAIInterval(), nameof(ICustomEnemy.DoAIInterval));

    public override void ReachedNodeInSearch() => InvokeOrFallback<ICustomEnemy>(
        customEnemy => customEnemy.DoAIInterval(), () => base.ReachedNodeInSearch(), nameof(DoAIInterval));

    public override void FinishedCurrentSearchRoutine() => enemy.FinishedCurrentSearchRoutine();

    public override void KillEnemy(bool destroy = false) => enemy.KillEnemy(destroy);

    public override void CancelSpecialAnimationWithPlayer() => enemy.CancelSpecialAnimationWithPlayer();

    public override void OnSynchronize<T>(ref BufferSerializer<T> serializer) => enemy.OnSynchronize(ref serializer);

    public override void OnSyncPositionFromServer(Vector3 newPos) => enemy.OnSyncPositionFromServer(newPos);

    public override void EnableEnemyMesh(bool enable, bool overrideDoNotSet = false) =>
        enemy.EnableEnemyMesh(enable, overrideDoNotSet);

    public override void SetEnemyOutside(bool outside = false) => enemy.SetEnemyOutside(outside);

    public override void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesPlayedInOneSpot = 0,
        int noiseID = 0) =>
        InvokeOrFallback<ICustomEnemy>(
            e => e.DetectNoise(noisePosition, noiseLoudness, timesPlayedInOneSpot),
            () => base.DetectNoise(noisePosition, noiseLoudness, timesPlayedInOneSpot),
            nameof(ICustomEnemy.DetectNoise)
        );

    public override void HitFromExplosion(float distance) =>
        InvokeOrFallback<ICustomEnemy>(customEnemy => customEnemy.HitFromExplosion(distance),
            () => base.HitFromExplosion(distance), nameof(ICustomEnemy.HitFromExplosion));

    public override void HitEnemy(int force = 1, PlayerControllerB playerWhoHit = null, bool playHitSFX = false,
        int hitID = -1) => InvokeOrFallback<ICustomEnemy>(customEnemy =>
            customEnemy.HitEnemy(force, (IPlayer)EntityManager.Instance.GetEntity(playerWhoHit.NetworkObjectId)),
        () => base.HitEnemy(force, playerWhoHit, playHitSFX, hitID), nameof(ICustomEnemy.HitEnemy));

    public override void ReceiveLoudNoiseBlast(Vector3 position, float angle) =>
        base.ReceiveLoudNoiseBlast(position, angle);

    public override void DaytimeEnemyLeave() => base.DaytimeEnemyLeave();

    public override void AnimationEventA() => base.AnimationEventA();

    public override void AnimationEventB() => base.AnimationEventB();

    public override void ShipTeleportEnemy() => base.ShipTeleportEnemy();

    public override void OnNetworkSpawn() => base.OnNetworkSpawn();

    public override void OnNetworkDespawn() => base.OnNetworkDespawn();

    public override void OnGainedOwnership() => base.OnGainedOwnership();

    public override void OnLostOwnership() => base.OnLostOwnership();

    public override void OnNetworkObjectParentChanged(NetworkObject parentNetworkObject) =>
        base.OnNetworkObjectParentChanged(parentNetworkObject);

    public override void __initializeVariables() => base.__initializeVariables();

    public override string __getTypeName() => base.__getTypeName();

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => base.ToString();

    public override bool Equals(object? other) => base.Equals(other);

    private void InvokeOrFallback<T>(Action<T> enemyAction, Action baseAction, string methodName)
    {
        MethodInfo? method = enemy.GetType().GetMethod(methodName);
        if (method?.DeclaringType == typeof(ICustomEnemyContent))
            baseAction();
        else
            enemyAction((T)enemy);
    }
}