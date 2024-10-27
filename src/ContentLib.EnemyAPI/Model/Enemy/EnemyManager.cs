using System;
using System.Collections.Generic;
using System.Linq;
using ContentLib.API.Model.Entity.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy;

public class EnemyManager
{
    /// <summary>
    /// Returns , or creates the singleton instance of the Enemy Manager. 
    /// </summary>
    /// <returns>The singleton instance.</returns>
    public static EnemyManager Instance { get; } = new();
    /// <summary>
    /// Dictionary of every registered enemy within the gameworld. 
    /// </summary>
    private Dictionary<ulong,IEnemy> _enemies;

    /// <summary>
    /// Private constructor that initialises the _enemies Dictionary. <i>(Developer Note: Keep private to ensure there
    /// is no way to construct this manager other than the singleton getter.)</i>
    /// </summary>
    private EnemyManager()
    {
        _enemies = new Dictionary<ulong, IEnemy>();
    }


    /// <summary>
    /// Registers an enemy to the manager, allowing for it to be managed via the api during game.
    /// </summary>
    /// <param name="enemyToRegister">The enemy to register.</param>
    public void RegisterEnemy(IEnemy enemyToRegister) => _enemies.Add(enemyToRegister.Id,enemyToRegister);
    
    /// <summary>
    /// Unregisters an enemy from the manager, typically done on-death. 
    /// </summary>
    /// <param name="id">The id of the enemy to unregister.</param>
    public void UnRegisterEnemy(ulong id) => _enemies.Remove(id);
    
    /// <summary>
    /// Unregisters all the enemies from the Manager, typically called at the end of a Round. 
    /// </summary>
    public void UnRegisterAllEnemies()
    {
        Debug.Log("UnRegistering All Enemies in Level");
        _enemies.Clear();
    }

    //TODO Probably needs some logic for invalid id's
    /// <summary>
    /// Gets the enemy specified with the given id.
    /// </summary>
    /// <param name="id">The id of the enemy to get.</param>
    /// <returns>The enemy with the corresponding id</returns>
    public IEnemy GetEnemy(ulong id)
    {
        if (_enemies.TryGetValue(id, out var enemy))
        {
            return enemy;
        }
        throw new KeyNotFoundException($"Enemy with ID {id} was not found.");
    }
    
    /// <summary>
    /// Checks ot see if an enemy with the given id is registered within the manager.
    /// </summary>
    /// <param name="id">The id to check.</param>
    /// <returns>True if the id corresponds to a registered enemy, False otherwise.</returns>
    public bool IsRegistered(ulong id) => _enemies.ContainsKey(id);
    
    /// <summary>
    /// Check to see if the enemy with the given id is currently spawned in the game world.
    /// </summary>
    /// <param name="id">The id to check.</param>
    /// <returns>True if the id corresponds to a registered enemy that is currently spawned, False otherwise.</returns>
    public bool IsSpawned(ulong id) => _enemies[id].IsSpawned;

    /// <summary>
    /// Checks to see if any player is in a given radius of the enemy. 
    /// </summary>
    /// <param name="enemyId">The id of the enemy to check.</param>
    /// <param name="radius">The radius to check.</param>
    /// <returns>True if 1 or more players are within the given radius of the enemy with the given id,
    /// False otherwise.</returns>
    public bool PlayerWithinRadius(ulong enemyId, float radius)
    {
        throw new NotImplementedException();
        
    }

    /// <summary>
    /// Returns how many players are currently in radius of the enemy with the given id.
    /// </summary>
    /// <param name="enemyId">The id of the enemy to check.</param>
    /// <param name="radius">The radius to check.</param>
    /// <returns>The amount of players within the given radius of enemy.</returns>
    /// <exception cref="NotImplementedException"></exception>
    int PlayersWithinRadius(ulong enemyId, float radius)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks to see if the enemy is of a specific type.
    /// </summary>
    /// <param name="enemy">The enemy to check.</param>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the enemy is of the given enemy type, False if either the typer is not a subclass of enemy
    /// or if the enemy is not of the given type. </returns>
    /// <exception cref="ArgumentException">Exception thrown if the type is not a subclass of "IEnemy".</exception>
    public bool IsEnemyTypeOf(IEnemy enemy, Type type)
    {
        try
        {
            if (!typeof(IEnemy).IsAssignableFrom(type))
                throw new ArgumentException($"$ The Type {type} is not a sub class of {typeof(IEnemy)}");

            return type.IsAssignableFrom(enemy.EnemyProperties.EnemyClassType);
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    /// <summary>
    /// Returns all enemies currently registered to the manager that are not Vanilla (i.e. AssetBundle related enemies)
    /// </summary>
    /// <returns>List of all custom enemies.</returns>
    public List<IEnemy> GetCustomEnemies() => _enemies
        .Values
        .Where(enemy => enemy.EnemyProperties.IsCustom)
        .ToList();
    
    /// <summary>
    /// Returns all enemies currently registered to the manager that are Vanilla (i.e. base game enemies).
    /// </summary>
    /// <returns>List of all the vanilla enemies.</returns>
    public List<IEnemy> GetVanillaEnemies() => _enemies
        .Values
        .Where(enemy => !enemy.EnemyProperties.IsCustom)
        .ToList();
}