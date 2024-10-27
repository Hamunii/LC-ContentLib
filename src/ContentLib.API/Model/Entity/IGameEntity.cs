using UnityEngine;

namespace ContentLib.API.Model.Entity;

/// <summary>
/// An interface representing the general functionality of an In-Game Entity. This can be an AI or Player. 
/// </summary>
public interface IGameEntity
{
    /// <summary>
    /// The id of game entity.
    /// </summary>
    ulong Id { get; }
    
    /// <summary>
    /// Check determining if the entity is alive in the game world.
    /// </summary>
    bool IsAlive { get; }
    /// <summary>
    /// The current health of the enemy.
    /// </summary>
    int Health { get; }
    
    /// <summary>
    /// The current in-game positon of the enemy as a Unity Engine Vector 3. 
    /// </summary>
    Vector3 Position { get; }
}