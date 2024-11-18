using UnityEngine;

namespace ContentLib.API.Model.Entity.Util;

/// <summary>
/// Interface representing the general functionality of an instance that can be "Revived" after death. 
/// </summary>
public interface IRevivable
{
    /// <summary>
    /// Revives the instance to a given position within the game-world. 
    /// </summary>
    /// <param name="revivePosition">The position within the game-world to revive the instance.</param>
    void Revive(Vector3 revivePosition);
}