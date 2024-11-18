using UnityEngine;

namespace ContentLib.API.Model.Entity.Util;

/// <summary>
/// Interface representing the general functionality of something that can be killed. Typically added to an IGameEntity.
/// </summary>
public interface IKillable
{
    /// <summary>
    /// Kills the subject.
    /// </summary>
    void Kill(Vector3 bodyVelocity,
        bool spawnBody = true,
        CauseOfDeath causeOfDeath = CauseOfDeath.Unknown,
        int deathAnimation = 0,
        Vector3 positionOffset = default (Vector3));
}