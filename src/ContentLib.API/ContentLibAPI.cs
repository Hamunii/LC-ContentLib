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
    internal void InitializeAPI(IAPILoader loader)
    {
        _eventManager = loader.GameEventManager;
        _entityManager = loader.EntityManager;
        _itemManager = loader.ItemManager;
    }
    public IGameEntity GetGameEntity(ulong id)
    {
        return _entityManager.GetEntity(id);
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
    {
        _eventManager.Subscribe(handler);
    }

    public void RegisterListener(IListener listener)
    {
        _eventManager.RegisterListener(listener);
    }

    




}