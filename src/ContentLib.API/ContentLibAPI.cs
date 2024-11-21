using System;
using System.Runtime.CompilerServices.Model.Managers;
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

    internal void InitializeAPI()
    {
        //TODO add methods for getting the managers from the core.
    }
    public IGameEntity GetGameEntity(ulong id)
    {
        throw new NotImplementedException("Getting the game entity is not implemented.");
    }

    public IGameEntity GetEntity(ulong id)
    {
        return _entityManager.GetEntity(id);
    }

    public int AlivePlayersCount
    {
        get => throw new NotImplementedException("Getting the player count is not yet implemented");
    }

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
    {
        throw new NotImplementedException("Subscribing to game events is not yet implemented");
    }

    public void RegisterListener(IListener listener)
    {
        throw new NotImplementedException("Registering listeners is not yet implemented");
    }

    




}