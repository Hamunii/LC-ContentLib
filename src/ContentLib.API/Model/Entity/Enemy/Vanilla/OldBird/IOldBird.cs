using ContentLib.API.Model.Entity.Util;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy.Vanilla.OldBird;

/// <summary>
/// Interface representing the general functionality of a Vanilla Old-Bird Enemy.
/// </summary>
public interface IOldBird : IEnemy, IAwakeable
{
    #region Condition Checks
    bool IsRoaming();
    bool IsHoldingPlayer();
    bool IsAlerted();
    
    #endregion
    
    #region Flying Methods
    void StartFlying(Vector3 landingPosition);
    void StopFlying();

    #endregion
    void SetChargingForward(bool isCharging);
    void FireMissilesAtTargetPosition(Vector3 targetPosition);
    void BlowTorchPlayer();
}