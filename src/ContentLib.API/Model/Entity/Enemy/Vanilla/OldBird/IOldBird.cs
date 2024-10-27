using ContentLib.API.Model.Entity.Util;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy.Vanilla.OldBird;

/// <summary>
/// Interface representing the general functionality of a Vanilla Old-Bird Enemy.
/// </summary>
public interface IOldBird : IEnemy, IAwakeable
{
    #region Fields
    /// <summary>
    /// The game entity that is currently held in the hands of the Old Bird. 
    /// </summary>
    IGameEntity? CurrentlyHeldEntity { get; set; }
    //TODO this might be worth adding to the IEnemy interface in some form instead.
    /// <summary>
    /// The Game Entity the Old Bird is currently Targeting. 
    /// </summary>
    IGameEntity? CurrentTarget { get; set; }
    /// <summary>
    /// The current node that the Old Bird is attempting to move to. 
    /// </summary>
    Transform CurrentTargetNode { get; set; }
    #endregion
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

    /// <summary>
    /// Checks to see if the Old Bird is currently in its flying state.
    /// </summary>
    /// <returns>True if the Old Bird is flying, False otherwise. </returns>
    bool IsFlying();
    
    #endregion
    
    #region Flying Methods
    /// <summary>
    /// Starts the Old Bird's flying animation, flying to a specified position in the game space.
    /// </summary>
    void StartFlying();
    
    /// <summary>
    /// Stops the Old Bird flying, landing at the specified position in the game space.
    /// <param name="landingPosition">The target position within the game space to land at.</param>>
    /// </summary>
    void StopFlying(Vector3 landingPosition);

    #endregion
    
    #region Attacking Methods
    /// <summary>
    /// Sets the old bird to perform its "boosted" charge animation, propelling towards its specified target.
    /// </summary>
    /// <param name="chargingTarget">The target to charge towards.</param>
    void SetChargingForward(IGameEntity chargingTarget);
    
    //TODO the start rotation param needs more elaboration. 
    /// <summary>
    /// Fires the Old Bird's missiles at a specified target position within the game world.
    /// </summary>
    /// <param name="targetPosition">The target position within the game world at which to fire the missiles at.</param>
    /// <param name="startRotation">The initial rotation of the ## of the Old Bird.</param>
    void FireMissilesAtTargetPosition(Vector3 targetPosition, Vector3 startRotation);
    
    /// <summary>
    /// Perform the killing "blow torch attack" to a Player that is currently within the grasp of an Old-Bird. 
    /// </summary>
    void BlowTorchPlayer();
    #endregion
   
}