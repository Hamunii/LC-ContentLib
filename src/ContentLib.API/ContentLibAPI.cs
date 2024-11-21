using System;
using System.Runtime.CompilerServices.Model.Managers;
using ContentLib.API.Model;
using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Entity.Player;
using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Event.Listener;
using ContentLib.EnemyAPI.Model.Enemy;

namespace ContentLib.API;

public class ContentLibAPI
{
    /// <summary>
    /// Singleton pattern call via a lazy implementation of the API.
    /// </summary>
    private static readonly Lazy<ContentLibAPI> instance = new Lazy<ContentLibAPI>(() => new ContentLibAPI());

    /// <summary>
    /// Gets the singleton instance of the API.
    /// </summary>
    public static ContentLibAPI Instance => instance.Value;
    private IEntityManager _entityManager;
    private IGameEventManager _eventManager;
    private IItemManager _itemManager;
    
    /// <summary>
    /// Internal class (hence not accessible to end-user) that initializes the API with its required Content-Lib Core
    /// Module functionality. 
    /// </summary>
    /// <param name="loader">The API Loader instance that will initialise the API.</param>
    internal void InitializeAPI(IAPILoader loader)
    {
        _eventManager = loader.GameEventManager;
        _entityManager = loader.EntityManager;
        _itemManager = loader.ItemManager;
    }
    /// <summary>
    /// Gets the Game Entity, if it exists, with the corresponding id.
    /// </summary>
    /// <param name="id">The id of the Game Entity.</param>
    /// <returns>The Game Entity with the corresponding id.</returns>
    public IGameEntity GetGameEntity(ulong id)
    {
        return _entityManager.GetEntity(id);
    }

    /// <summary>
    /// Subscribe to an event with a method (Action) that takes in a Content-Lib Game Event as a parameter.
    /// </summary>
    /// <param name="handler">The method which will handle the event.</param>
    /// <typeparam name="TEvent">The type parameter of the Game Event to subscribe to.</typeparam>
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
    {
        _eventManager.Subscribe(handler);
    }

    /// <summary>
    /// Registers an IListener instance 
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterListener(IListener listener)
    {
        _eventManager.RegisterListener(listener);
    }
}