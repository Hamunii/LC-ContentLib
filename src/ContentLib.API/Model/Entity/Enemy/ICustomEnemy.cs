using ContentLib.API.Model.Entity.Player;
using ContentLib.EnemyAPI.Model.Enemy;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy;

public interface ICustomEnemy : IEnemy
{
    void SetEnemyStunned(bool setToStunned, float stunTime, IPlayer? stunningPlayer = null);
    void UseNestSpawnedObject(Object nestObject);
    void OnCollideWithPlayer(Collider colider);
    void OnCollideWithEnemy(Collider colider, IEnemy enemy);
    void DoAIInterval();
    void ReachedNodeInSearch();
    void FinishedCurrentSearchRoutine();
    void Update();
    void KillEnemy(bool destroy = false);
    void CancelSpecialAnimationWithPlayer();
    void OnSynchronize<T>(ref BufferSerializer<T> serializer) where T : IReaderWriter;
    void OnDestroy();
    void OnSyncPositionFromServer(Vector3 newPos);
    void OnDrawGizmos();
    void EnableEnemyMesh(bool enable, bool DoNotSet = false);
    void SetEnemyOutside(bool outside = false);
    void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesPlayedInOneSpot = 0, int noiseID = 0);
    void HitFromExplosion(float distance);
    void HitEnemy(int force = 1, IPlayer playerWhoHit = null, bool playHitSFX = false, int hitID = -1);
    void ReceiveLoudNoiseBlast(Vector3 position, float angle);
    void DaytimeEnemyLeave();
    void AnimationEventA();
    void AnimationEventB();
    void ShipTeleportEnemy();
    void OnNetworkSpawn();
    void OnNetworkDespawn();
    void OnGainedOwnership();
    void OnLostOwnership();
    void OnNetworkObjectParentChanged(NetworkObject parentNetworkObject);
    void __initializeVariables();
    string __getTypeName();
    int GetHashCode();
    string ToString();
    bool Equals(object? other);

}