using ContentLib.API.Model.Entity.Enemy;
using Unity.Netcode;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Vanilla.Coilhead;
/// <summary>
/// Interface representing the general functionality of a Coilhead. 
/// </summary>
public interface ICoilhead : IEnemy
{
    void OnCollideWithPlayer(Collider other);

    // Animation Control Methods
    [ServerRpc]
    void SetAnimationStopServerRpc();

    [ClientRpc]
    void SetAnimationStopClientRpc();

    [ServerRpc]
    void SetAnimationGoServerRpc();

    [ClientRpc]
    void SetAnimationGoClientRpc();

    // Search and Chase Methods
    AISearchRoutine SearchForPlayers();

    CoilheadBehaviourState State { get;}

}

public enum CoilheadBehaviourState
{
    Idle, Frozen, SearchingForPlayers
}