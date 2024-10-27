using ContentLib.API.Model.Entity.Util;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy.Vanilla.OldBird;

/// <summary>
/// Interface representing the general functionality of a Vanilla Old-Bird Enemy.
/// </summary>
public interface IOldBird : IEnemy, IAwakeable
{
    #region Condition Checks
    /// <summary>
    /// Checks to see if the Old Bird is currently roaming, i.e. patrolling from node to node, with no specified target.
    /// </summary>
    /// <returns>True if the Old Bird is in a Roaming State, False otherwise.</returns>
    bool IsRoaming();
    
    /// <summary>
    /// Checks to see if the Old Bird is currently holding a player.
    /// </summary>
    /// <returns>True if the Old Bird is holding a player, False otherwise.</returns>
    bool IsHoldingPlayer();
    
    /// <summary>
    /// Checks to see if the Old Bird is currently alerted, i.e. has spotted a Target.
    /// </summary>
    /// <returns>True if the Old Bird is alerted, False otherwise.</returns>
    bool IsAlerted();
    
    #endregion
    
    #region Flying Methods
    /// <summary>
    /// Starts the Old Bird's flying animation, flying to a specified position in the game space.
    /// </summary>
    /// <param name="landingPosition">The target position within the game space to land at post-flight.</param>
    void StartFlying(Vector3 landingPosition);
    
    /// <summary>
    /// Stops the Old Bird flying, landing at the specified position in the game space.
    /// <param name="landingPosition">The target position within the game space to land at.</param>>
    /// </summary>
    void StopFlying(Vector3 landingPosition);

    #endregion
    /// <summary>
    /// Sets the old bird to perform its "boosted" charge animation, propelling towards its specified target.
    /// </summary>
    /// <param name="chargingTarget">The target to charge towards.</param>
    void SetChargingForward(IGameEntity chargingTarget);
    
    /// <summary>
    /// Fires the Old Bird's missiles at a specified target position within the game world.
    /// </summary>
    /// <param name="targetPosition">The target position within the game world at which to fire the missiles at.</param>
    void FireMissilesAtTargetPosition(Vector3 targetPosition);
    
    /// <summary>
    /// Perform the killing "blow torch attack" to a Player that is currently within the grasp of an Old-Bird. 
    /// </summary>
    void BlowTorchPlayer();
}