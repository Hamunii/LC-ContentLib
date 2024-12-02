using ContentLib.API.Model.Entity.Util;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy.Vanilla.OldBird;

/// <summary>
///     Interface representing the general functionality of an Old Bird Vanilla Enemy.
/// </summary>
public interface IOldBird : IEnemy, IAwakeable
{
    /// <summary>
    ///     The current Behaviour Phase of the Old Bird.
    /// </summary>
    OldBirdPhase CurrentBehaviourPhase { get; }

    /// <summary>
    ///     Causes the old-bird to enter a flight to a specified destination within the game-world.
    /// </summary>
    /// <param name="flightDestination">The destination of the flight within the game-world.</param>
    void EnterFlight(Vector3 flightDestination);
}

/// <summary>
///     Enum representing the various behaviour phases the Old Bird's AI can currently be in.
/// </summary>
public enum OldBirdPhase
{
    /// <summary>
    ///     The Old Bird Phase in which it is currently attempting to travel to every node within 36 units of
    ///     the position it was in at the beginning of this phase.
    /// </summary>
    Roaming,

    /// <summary>
    ///     The Old Bird is currently within flight between its take-off location and destination. Whilst in this phase,
    ///     the Old Bird will be continuously scanning for players.
    /// </summary>
    Flying,

    /// <summary>
    ///     The Old Bird has observed a triggering Game Entity, but are not yet committed to hostile action against
    ///     the Entity. Whilst in this phase they will move towards the location of the spotted Entity. Once reached, the
    ///     Old Bird will then search the area surrounding the location.
    /// </summary>
    Investigating,

    /// <summary>
    ///     The Old Bird is currently pursuing a targeted Game Entity in a hostile manner. It will use this phase to attack
    ///     the Entity with any means at its disposal.
    /// </summary>
    Alerted
}