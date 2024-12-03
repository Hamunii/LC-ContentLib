using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices.Model.Managers;
using ContentLib.API.Exceptions.Core.Manager;
using ContentLib.API.Model.Entity;
using ContentLib.Core.Utils;

namespace ContentLib.Core.Model.Managers;

public class EntityManager : IEntityManager
{
    /// <summary>
    /// Returns , or creates the singleton instance of the Entity Manager. 
    /// </summary>
    /// <returns>The singleton instance.</returns>
    public static EntityManager Instance { get; } = new();
    /// <summary>
    /// Dictionary of every registered entity within the gameworld. 
    /// </summary>
    private Dictionary<ulong,IGameEntity> _entities;
    
    /// <summary>
    /// Private constructor that initialises the _enemies Dictionary. <i>(Developer Note: Keep private to ensure there
    /// is no way to construct this manager other than the singleton getter.)</i>
    /// </summary>
    private EntityManager()
    {
        _entities = new Dictionary<ulong, IGameEntity>();
        CLLogger.Instance.DebugLog("Entity Manager initialized successfully", DebugLevel.CoreEvent);
    }


    /// <summary>
    /// Registers an entity to the manager, allowing for it to be managed via the api during game.
    /// </summary>
    /// <param name="entityToRegister">The entity to register.</param>
    public void RegisterEntity(IGameEntity entityToRegister)
    {
        CLLogger.Instance.DebugLog($"Attempting to register Game Entity with id of {entityToRegister.Id}"
            , DebugLevel.EntityEvent);
        try
        {
            _entities.Add(entityToRegister.Id, entityToRegister);
        }
        catch (Exception e)
        {
            throw new InvalidEntityRegistrationException(entityToRegister,e);
        }
        
    }
    
    /// <summary>
    /// Unregisters an entity from the manager, typically done on-death. 
    /// </summary>
    /// <param name="id">The id of the entity to unregister.</param>
    public void UnRegisterEntity(ulong id) => _entities.Remove(id);
    
    /// <summary>
    /// Unregisters all the enemies from the Manager, typically called at the end of a Round. 
    /// </summary>
    public void UnRegisterAllEntities()
    {
        CLLogger.Instance.DebugLog("Unregistering all entities in level!", DebugLevel.EntityEvent);
        _entities.Clear();
    }

    //TODO Probably needs some logic for invalid id's
    /// <summary>
    /// Gets the entity specified with the given id.
    /// </summary>
    /// <param name="id">The id of the entity to get.</param>
    /// <returns>The entity with the corresponding id</returns>
    public IGameEntity GetEntity(ulong id)
    {
        CLLogger.Instance.DebugLog($"Attempting to register Game Entity with id of {id}", DebugLevel.EntityEvent);
        if (_entities.TryGetValue(id, out var entity))
        {
            CLLogger.Instance.DebugLog($"Found Game Entity with id of {id}", DebugLevel.EntityEvent);
            return entity;
        }
        throw new KeyNotFoundException($"entity with ID {id} was not found.");
    }
    
    /// <summary>
    /// Checks ot see if an entity with the given id is registered within the manager.
    /// </summary>
    /// <param name="id">The id to check.</param>
    /// <returns>True if the id corresponds to a registered entity, False otherwise.</returns>
    public bool IsRegistered(ulong id) => _entities.ContainsKey(id);

}