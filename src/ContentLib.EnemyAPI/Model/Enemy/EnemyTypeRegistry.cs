using System;
using System.Collections.Generic;
using ContentLib.API.Model.Entity.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Factories;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;

namespace ContentLib.EnemyAPI.Model.Enemy;

/// <summary>
/// Registery for all the various enemy types within the game space. This allows for both the registration of vanilla
/// and custom enemies at startup, and then having a collective registry where said enemies can be called from for
/// the purposes of construction. 
/// </summary>
public class EnemyTypeRegistry
{
    /// <summary>
    /// The dictionary of Enemy property values based on their Type as keys.
    /// </summary>
    private Dictionary<Type, IEnemyProperties> _registry = new();


    /// <summary>
    /// Registers a new enemy with the registry. This is done for all vanilla enemies at startup and is typically called
    /// to register custom enemies via the AssetBundle Handler. 
    /// </summary>
    /// <param name="properties">The properties of the enemy to register.</param>
    /// <typeparam name="T">The enemy's type</typeparam>
    /// <exception cref="ArgumentException">Throws if the enemy type has already been registered to the
    /// dictionary.</exception>
    public void RegisterEnemyType<T>(IEnemyProperties properties) where T : IEnemy
    {
        var type = typeof(T);
        if (_registry.ContainsKey(type))
        {
            throw new ArgumentException($"The type {type} is already registered.");
        }
        _registry.Add(type, properties);
      
    }
/// <summary>
///Creates an enemy, based on the given IEnemy type. 
/// </summary>
/// <typeparam name="T">The type of enemy to create.</typeparam>
/// <returns>Instance of the given enemy type.</returns>
/// <exception cref="ArgumentException">Throws if the given type is not registered or if, somehow the Register and Enemy
/// Properties do not have matching types.</exception>
    public T CreateEnemy<T>() where T : IEnemy
    {
        var type = typeof(T);
        if (!_registry.TryGetValue(type, out IEnemyProperties? properties))
        {
            throw new ArgumentException($"The type {type} is not registered.");
        }

        try
        {
            return (T) new EnemyFactory(properties).Create();
        }
        catch (InvalidCastException e)
        {
            throw new ArgumentException($"The type {type} is not a valid type.", e);
        }
    }
}