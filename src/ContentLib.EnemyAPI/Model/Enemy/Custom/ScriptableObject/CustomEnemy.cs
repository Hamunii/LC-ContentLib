using ContentLib.API.Model.Entity.Player;
using ContentLib.Core.Model.Managers;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Custom.ScriptableObject;

internal class CustomEnemy(API.Model.Entity.Enemy.ICustomEnemy enemy) : EnemyAI
{
    public override void Start() => base.Start();

    public override void Update() => base.Update();

    public override void OnDestroy() => base.OnDestroy();

    public override void OnDrawGizmos() => base.OnDrawGizmos();

    public override void SetEnemyStunned(bool setToStunned, float setToStunTime = 1,
        PlayerControllerB setStunnedByPlayer = null) =>
        enemy.SetEnemyStunned(setToStunned, setToStunTime, 
            (IPlayer) EntityManager.Instance.GetEntity(setStunnedByPlayer.NetworkObjectId));


    public override void UseNestSpawnObject(EnemyAINestSpawnObject nestSpawnObject) =>
        base.UseNestSpawnObject(nestSpawnObject);

    public override void OnCollideWithPlayer(Collider other) => enemy.OnCollideWithPlayer(other);

    public override void OnCollideWithEnemy(Collider other, EnemyAI collidedEnemy = null) =>
        enemy.OnCollideWithEnemy(other, (IEnemy)EntityManager.Instance.GetEntity(collidedEnemy.NetworkObjectId));

    public override void DoAIInterval() => enemy.DoAIInterval();

    public override void ReachedNodeInSearch() => enemy.ReachedNodeInSearch();

    public override void FinishedCurrentSearchRoutine() => enemy.FinishedCurrentSearchRoutine();

    public override void KillEnemy(bool destroy = false) => enemy.KillEnemy(destroy);

    public override void CancelSpecialAnimationWithPlayer() => base.CancelSpecialAnimationWithPlayer();

    public override void OnSynchronize<T>(ref BufferSerializer<T> serializer) => base.OnSynchronize(ref serializer);

    public override void OnSyncPositionFromServer(Vector3 newPos) => base.OnSyncPositionFromServer(newPos);

    public override void EnableEnemyMesh(bool enable, bool overrideDoNotSet = false) =>
        base.EnableEnemyMesh(enable, overrideDoNotSet);

    public override void SetEnemyOutside(bool outside = false) => base.SetEnemyOutside(outside);

    public override void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesPlayedInOneSpot = 0,
        int noiseID = 0) => base.DetectNoise(noisePosition, noiseLoudness, timesPlayedInOneSpot, noiseID);

    public override void HitFromExplosion(float distance) => base.HitFromExplosion(distance);

    public override void HitEnemy(int force = 1, PlayerControllerB playerWhoHit = null, bool playHitSFX = false,
        int hitID = -1) => base.HitEnemy(force, playerWhoHit, playHitSFX, hitID);

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
}