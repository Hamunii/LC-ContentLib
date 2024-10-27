using ContentLib.EnemyAPI.Model.Enemy;

namespace ContentLib.API.Model.Entity.Enemy;
/// <summary>
/// Interface representing the general functionality of an Enemy Game Entity within the gameworld. 
/// </summary>
public interface IEnemy : IGameEntity
{
    /// <summary>
    /// The properties of the enemy.
    /// </summary>
    IEnemyProperties EnemyProperties { get; }
    /// <summary>
    /// Checks if the Enemy is currently spawned or not.
    /// </summary>
    bool IsSpawned { get; }
    
    /// <summary>
    /// Check of if the Enemy is hostile or not. 
    /// </summary>
    bool IsHostile { get; }
    
    /// <summary>
    /// Check of if the Enemy is currently chasing another IEntity.
    /// </summary>
    bool IsChasing { get; }
}

