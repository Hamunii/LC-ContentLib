using System;
using ContentLib.API.Model.Entity.Enemy;
using ContentLib.API.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;

namespace ContentLib.Core.Model.Event;
/// <summary>
/// Interface that represents the general functionality of an event that was triggered by an action relating to an
/// in-game monster. 
/// </summary>
public interface IMonsterEvents : IGameEvent
{
    /// <summary>
    /// Gets the AI of the monster triggering the event. 
    /// </summary>
    /// <returns>The event triggering monster's AI</returns>
    IEnemy Enemy { get; }
}

public abstract class MonsterCollideWithPlayerEvent : IMonsterEvents
{
    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }

    /// <inheritdoc />
    public bool IsCancelled { get; set; }

}

/// <summary>
/// Event triggered when a monster kills a player.
/// </summary>
public abstract class MonsterKillsPlayerEvent : IMonsterEvents
{
    
    /// <inheritdoc />
    public bool IsCancelled { get; set; } = false;

    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }
}

/// <summary>
/// Event triggered when a monster is spawned. (SHOULD NOT BE CANCELLABLE FOR THE TIME BEING) 
/// </summary>
public abstract class MonsterSpawnEvent : IMonsterEvents
{
    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }

    public bool IsCancelled { get; set; }
}

/// <summary>
/// Event triggered when an IWakeable Enemy is awakened. 
/// </summary>
public abstract class WakeableMonsterWakeEvent : IMonsterEvents
{
    public bool IsCancelled { get; set; }
    public abstract IEnemy Enemy { get; }
}

